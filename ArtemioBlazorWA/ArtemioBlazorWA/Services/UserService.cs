using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class UserService
{
    private readonly ApiClient _api;

    public UserService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<UserModel>> GetAllAsync()
    {
        return await _api.GetListAsync<UserModel>("users") ?? new List<UserModel>();
    }

    public async Task<UserModel> GetAsync(int id)
    {
        return await _api.GetAsync<UserModel>($"users/{id}");
    }

    public async Task<UserModel> PostAsync(UserModel body)
    {
        return await _api.PostAsync<UserModel>("users", body);
    }

    public async Task<LoginResponseModel> PostLoginAsync(LoginModel body)
    {
        return await _api.PostAsync<LoginResponseModel>("login", body);
    }

    public async Task<UserModel> PutAsync(UserModel body)
    {
        return await _api.PutAsync<UserModel>("users", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"users/{id}");
    }

}
