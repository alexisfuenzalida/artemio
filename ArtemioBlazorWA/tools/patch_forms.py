from pathlib import Path

forms = Path(r"D:\repos\Artemio\ArtemioBlazorWA\ArtemioBlazorWA\Forms")
needle = "        _processing = false;\n        MudDialog.Close(true);"
insert = """        _processing = false;
        if (x == null)
        {
            Snackbar.Add("Could not save. Please try again.", Severity.Error);
            return;
        }
        MudDialog.Close(true);"""

for path in forms.glob("*.razor"):
    text = path.read_text(encoding="utf-8")
    if needle not in text:
        continue
    if "if (x == null)" in text:
        continue
    if "@inject ISnackbar" not in text:
        text = text.replace("@inject IDialogService DialogService", "@inject IDialogService DialogService\n@inject ISnackbar Snackbar")
        if "@inject ISnackbar" not in text:
            # insert after first @inject
            text = text.replace("@inject ", "@inject ISnackbar Snackbar\n@inject ", 1)
    text = text.replace(needle, insert)
    path.write_text(text, encoding="utf-8")
    print("patched", path.name)
