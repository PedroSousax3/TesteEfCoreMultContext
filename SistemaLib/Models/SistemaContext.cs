using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PessoaLib.Models;

namespace SistemaLib.Models;

public class SistemaContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public SistemaContext() { }
    public SistemaContext(DbContextOptions options) : base(options) { }
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
        PessoaContext.ConfigurarEntidades(modelBuilder);

        modelBuilder.Entity<Usuario>(entidade => {
            entidade.ToTable("USUARIO");

            entidade.HasKey(e => e.codigo);

            entidade.HasOne(e => e.pessoa).WithMany().HasForeignKey(e => e.codPessoa);
        });
    }
}
