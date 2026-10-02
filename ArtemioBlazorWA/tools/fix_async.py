from pathlib import Path

p = Path(r"D:\repos\Artemio\ArtemioBlazorWA\ArtemioBlazorWA\Services")
for f in p.glob("*.cs"):
    if f.name == "ApiClient.cs":
        continue
    t = f.read_text(encoding="utf-8")
    t = t.replace("    public Task<", "    public async Task<")
    t = t.replace(
        'PostAsync<BookingModel>($"copybooking/{id}", id)',
        'PostAsync<BookingModel>($"copybooking/{id}", null)',
    )
    t = t.replace("Task<UserModel> PostLoginAsync", "Task<LoginResponseModel> PostLoginAsync")
    t = t.replace('PostAsync<UserModel>("login"', 'PostAsync<LoginResponseModel>("login"')
    f.write_text(t, encoding="utf-8")
    print("fixed", f.name)
