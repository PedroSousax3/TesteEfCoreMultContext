using Microsoft.EntityFrameworkCore;
using PessoaLib.Models;
using SistemaLib.Models;
using System.Data.SqlClient;

namespace TesteConsole;

public class MainContext : DbContext
{
    private readonly SqlConnection? _sqlConnection;
    public MainContext() {}
    public MainContext(SqlConnection? sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        PessoaContext.ConfigurarEntidades(modelBuilder);
        SistemaContext.ConfigurarEntidades(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SqlConnection sqlConnection;
        if (_sqlConnection == null) 
        {
            sqlConnection = new SqlConnection("Server=<servidor>;Database=<banco>;User Id=<usuario>;Password=<senha>;TrustServerCertificate=True;MultiSubnetFailover=True;");
        }
        else 
        {
            sqlConnection = _sqlConnection;
        }
        optionsBuilder.UseSqlServer(sqlConnection);

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);

        base.OnConfiguring(optionsBuilder);
    }
}