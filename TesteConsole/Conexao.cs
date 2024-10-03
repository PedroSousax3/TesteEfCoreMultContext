using Microsoft.EntityFrameworkCore;
using PessoaLib.Models;
using SistemaLib.Models;
using Middleware;
using System.Data.SqlClient;
using Utilitario.Models.erro;
using Microsoft.EntityFrameworkCore.Storage;

namespace TesteConsole;

public class Conexao : IConexao
{
    private IDbContextTransaction? _transacao = null;
    private readonly Dictionary<EnumContext, DbContext> _contexts;
    public DbContext this[EnumContext enumContext] => _contexts[enumContext] ?? throw new HttpErroException(System.Net.HttpStatusCode.InternalServerError, "Não foi possível conectar ao servidor.");
    public T GetDbContext<T>(EnumContext enumContext) where T : DbContext
    {
        return _contexts[enumContext] as T ?? throw new HttpErroException(System.Net.HttpStatusCode.InternalServerError, "Não foi possível conectar ao servidor.");
    }
    public IDbContextTransaction? BeginTransaction()
    {
        try 
        {
            if (_transacao != null) 
                return null;
            else
            {
                _transacao = _contexts.First().Value.Database.BeginTransaction();
                return _transacao;
            }
        }
        finally
        {
            if (_transacao != null)
                foreach (KeyValuePair<EnumContext, DbContext> contextItem in _contexts)
                {
                    if (contextItem.Value.Database.CurrentTransaction == null)
                        contextItem.Value.Database.UseTransaction(_transacao.GetDbTransaction());
                }
        }    
    }
    public IDbContextTransaction? BeginTransaction(DbContext context)
    {
        if (_transacao != null) 
        {
            if (context.Database.CurrentTransaction == null)
                context.Database.UseTransaction(_transacao.GetDbTransaction());
            return null;
        }
        else
        {
            _transacao = context.Database.BeginTransaction();
            return _transacao;
        }
    }
    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        try 
        {
            if (_transacao != null) 
                return null;
            else
            {
                _transacao = await _contexts.First().Value.Database.BeginTransactionAsync();
                return _transacao;
            }
        }
        finally
        {
            if (_transacao != null)
                foreach (KeyValuePair<EnumContext, DbContext> contextItem in _contexts)
                {
                    if (contextItem.Value.Database.CurrentTransaction == null)
                        contextItem.Value.Database.UseTransaction(_transacao.GetDbTransaction());
                }
        }
    }
    public async Task<IDbContextTransaction?> BeginTransactionAsync(DbContext context)
    {
        if (_transacao != null) 
        {
            if (context.Database.CurrentTransaction == null)
                await context.Database.UseTransactionAsync(_transacao.GetDbTransaction());
            return null;
        }
        else
        {
            _transacao = await context.Database.BeginTransactionAsync();
            return _transacao;
        }
    }
    public Conexao()
    {
        #region Usando Transaction Scope
        // Usando SqlConnection
        // SqlConnection sqlConnection = new SqlConnection("Server=<servidor>;Database=<banco>;User Id=<usuario>;Password=<senha>;TrustServerCertificate=True;MultiSubnetFailover=True;");
        // var optionsPessoa = new DbContextOptionsBuilder<PessoaContext>().UseSqlServer(sqlConnection).Options;
        // var optionsSistema = new DbContextOptionsBuilder<SistemaContext>().UseSqlServer(sqlConnection).Options;

        // var pessoaContext = new PessoaContext(optionsPessoa);
        // var sistemaContext = new SistemaContext(optionsSistema);

        // _contexts = new Dictionary<EnumContext, DbContext>()
        // {
        //     { EnumContext.Pessoa, pessoaContext },
        //     { EnumContext.Sistema, sistemaContext }
        // };

        // Ef core nativo
        // var options = new DbContextOptionsBuilder<DbContext>().UseSqlServer("Server=<servidor>;Database=<banco>;User Id=<usuario>;Password=<senha>;TrustServerCertificate=True;MultiSubnetFailover=True;").Options;
        // _contexts = new Dictionary<EnumContext, DbContext>()
        // {
        //     { EnumContext.Pessoa, new PessoaContext(options) },
        //     { EnumContext.Sistema, new SistemaContext(options) }
        // };
        #endregion

        #region BeginTransaction
        SqlConnection sqlConnection = new SqlConnection("Server=<servidor>;Database=<banco>;User Id=<usuario>;Password=<senha>;TrustServerCertificate=True;MultiSubnetFailover=True;");
        var contextConfig = new DbContextOptionsBuilder<DbContext>().UseSqlServer(sqlConnection);
        var options = contextConfig.Options;

        var pessoaContext = new PessoaContext(options);
        var sistemaContext = new SistemaContext(options);
        
        _contexts = new Dictionary<EnumContext, DbContext>()
        {
            { EnumContext.Pessoa, pessoaContext },
            { EnumContext.Sistema, sistemaContext }
        };
        #endregion
    }
} 