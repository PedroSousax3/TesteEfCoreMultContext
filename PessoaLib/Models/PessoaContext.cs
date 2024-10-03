
using Microsoft.EntityFrameworkCore;

namespace PessoaLib.Models;

public class PessoaContext : DbContext
{
    public DbSet<Pessoa> Pessoas { get; set; }
    public PessoaContext() { }
    public PessoaContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurarEntidades(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);

        base.OnConfiguring(options);
    }
    public static void ConfigurarEntidades(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>(entidade => {
            entidade.ToTable("PESSOA");
            
            entidade.HasKey(e => e.codigo);
        });
    }
}
