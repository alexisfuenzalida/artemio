using System.Data;
using MySql.Data.MySqlClient;

namespace MinimalAPIArtemio.Database;

public interface IDbConnectionFactory
{
    IDbConnection Create();
    string ConnectionString { get; }
}

public sealed class MySqlConnectionFactory : IDbConnectionFactory
{
    public string ConnectionString { get; }

    public MySqlConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection Create() => new MySqlConnection(ConnectionString);
}
