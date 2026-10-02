import re
from pathlib import Path

SERVICES = Path(r"D:\repos\Artemio\ArtemioBlazorWA\ArtemioBlazorWA\Services")

SKIP = {"ApiClient.cs"}

GET_LIST = re.compile(
    r"var client = new HttpClient\(\);\s*var result = await client\.GetAsync\(\$\"\{BaseURL\.Url\}/(?P<url>[^\"]+)\"\);\s*var content = await result\.Content\.ReadAsStringAsync\(\);\s*(?:List<(?P<list>[^>]+)> data = JsonSerializer\.Deserialize<List<(?P=list)>>\(content\);|(?:var| (?P<t>\w+(?:\.\w+)*) ) data = JsonSerializer\.Deserialize<(?P<t2>[^>]+)>\(content\);)",
    re.S,
)


def rewrite_file(path: Path) -> str:
    text = path.read_text(encoding="utf-8")
    class_name = re.search(r"public class (\w+)", text).group(1)
    methods = []
    for m in re.finditer(
        r"public async Task<(?P<ret>[^>]+(?:<[^>]+>)?)>\s+(?P<name>\w+)\((?P<args>[^)]*)\)\s*\{(?P<body>.*?)\n        \}",
        text,
        re.S,
    ):
        methods.append(m.groupdict())

    lines = [
        "using ArtemioBlazorWA.Models;",
        "",
        "namespace ArtemioBlazorWA.Services;",
        "",
        f"public class {class_name}",
        "{",
        "    private readonly ApiClient _api;",
        "",
        f"    public {class_name}(ApiClient api)",
        "    {",
        "        _api = api;",
        "    }",
        "",
    ]

    for method in methods:
        ret = method["ret"].strip()
        name = method["name"]
        args = method["args"].strip()
        body = method["body"]
        converted = convert_body(ret, args, body)
        arglist = args
        lines.append(f"    public Task<{ret}> {name}({arglist})")
        lines.append("    {")
        lines.append(f"        {converted}")
        lines.append("    }")
        lines.append("")

    lines.append("}")
    return "\n".join(lines) + "\n"


def convert_body(ret: str, args: str, body: str) -> str:
    get = re.search(r'GetAsync\(\$"\{BaseURL\.Url\}/(?P<url>[^"]+)"\)', body)
    post = re.search(r'PostAsync\(\$"\{BaseURL\.Url\}/(?P<url>[^"]+)"', body)
    put = re.search(r'PutAsync\(\$"\{BaseURL\.Url\}/(?P<url>[^"]+)"', body)
    delete = re.search(r'DeleteAsync\(\$"\{BaseURL\.Url\}/(?P<url>[^"]+)"\)', body)
    stream = "ReadAsStreamAsync" in body

    url = None
    verb = None
    if get:
        url, verb = get.group("url"), "get"
    elif post:
        url, verb = post.group("url"), "post"
    elif put:
        url, verb = put.group("url"), "put"
    elif delete:
        url, verb = delete.group("url"), "delete"

    if not url:
        return "return default;"

    url_cs = to_interpolated(url)
    inner = unwrap_task(ret)
    is_list = inner.startswith("List<")
    body_arg = first_arg_name(args)

    if verb == "get":
        if inner == "int" or inner == "bool":
            return f"return await _api.GetAsync<{inner}>({url_cs}) ?? default;"
        if is_list:
            return f"return await _api.GetListAsync<{inner[5:-1]}>({url_cs}) ?? new {inner}();"
        return f"return await _api.GetAsync<{inner}>({url_cs});"
    if verb == "post":
        if stream:
            if "StringResult" in ret or "tuple" in ret.lower() or ret.startswith("("):
                return (
                    f"var stream = await _api.PostStreamAsync({url_cs}, {body_arg});\n"
                    "        return stream is null ? default : (stream, null);"
                )
            return f"return await _api.PostStreamAsync({url_cs}, {body_arg});"
        if body_arg:
            return f"return await _api.PostAsync<{inner}>({url_cs}, {body_arg});"
        return f"return await _api.PostAsync<{inner}>({url_cs}, null);"
    if verb == "put":
        return f"return await _api.PutAsync<{inner}>({url_cs}, {body_arg});"
    if verb == "delete":
        return f"return await _api.DeleteAsync<{inner}>({url_cs});"
    return "return default;"


def unwrap_task(ret: str) -> str:
    return ret


def first_arg_name(args: str) -> str | None:
    if not args:
        return None
    parts = args.split(",")[0].strip().split()
    return parts[-1] if parts else None


def to_interpolated(url: str) -> str:
    if "{" in url:
        return f"$\"{url}\""
    return f"\"{url}\""


def main() -> None:
    for path in SERVICES.glob("*.cs"):
        if path.name in SKIP:
            continue
        try:
            new = rewrite_file(path)
            path.write_text(new, encoding="utf-8")
            print("rewrote", path.name)
        except Exception as ex:
            print("FAILED", path.name, ex)


if __name__ == "__main__":
    main()
