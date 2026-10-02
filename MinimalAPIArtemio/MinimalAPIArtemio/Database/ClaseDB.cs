using Dapper;
using MySql.Data.MySqlClient;

namespace MinimalAPIArtemio.Database;

public class ClaseDB<T>
{
    private readonly string _connectionString;

    public ClaseDB()
        : this(DbConnectionAccessor.ConnectionString)
    {
    }

    public ClaseDB(IDbConnectionFactory factory)
        : this(factory.ConnectionString)
    {
    }

    public ClaseDB(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    private MySqlConnection CreateConnection() => new(_connectionString);

    public async Task<List<T>> GetAllAsync(string sql, object? param = null)
    {
        await using var connection = CreateConnection();
        var lista = await connection.QueryAsync<T>(sql, param);
        return lista.ToList();
    }

    public async Task<T?> GetAsync(string sql, object? param = null)
    {
        await using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
    }

    public async Task<int> GetLastIdAsync(string sql, object? param = null)
    {
        await using var connection = CreateConnection();
        return await connection.QuerySingleAsync<int>(sql, param);
    }

    public async Task ExcecuteQueryAsync(T obj, string sql)
    {
        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, obj);
    }

    public async Task ExcecuteQueryAsync(string sql, object? param = null)
    {
        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql, param);
    }

    public async Task<int> ExecuteInsertAsync(T obj, string sql)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, obj);
        return await connection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID();");
    }

    public async Task<int> ExecuteInsertSqlAsync(string sql, object? param = null)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, param);
        return await connection.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID();");
    }
}

public static class DbConnectionAccessor
{
    public static string ConnectionString { get; set; } = string.Empty;
}
