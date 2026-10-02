using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Services;

public class ContractAppService
{
    public Task<List<ContractModel>> GetAllAsync()
    {
        var db = new ClaseDB<ContractModel>();
        return db.GetAllAsync("SELECT * FROM Contracts;");
    }

    public Task<ContractModel?> GetAsync(int id)
    {
        var db = new ClaseDB<ContractModel>();
        return db.GetAsync("SELECT * FROM Contracts WHERE ContractId = @id;", new { id });
    }
}
