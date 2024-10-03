using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Middleware;
using PessoaLib.Models;

namespace PessoaLib.Repository;

public class PessoaRepository
{
    private readonly IConexao _conexao;
    private readonly PessoaContext _pessoaContext;
    public PessoaRepository(IConexao conexao)
    {
        _conexao = conexao;
        _pessoaContext = conexao.GetDbContext<PessoaContext>(EnumContext.Pessoa);
    }
    public async Task inserirEfTrasaction(Pessoa pessoa)
    {
        using (var trasaction = _conexao.BeginTransaction())
        {
            try 
            {
                await _pessoaContext.Pessoas.AddAsync(pessoa);

                await _pessoaContext.SaveChangesAsync();

                // Simular erro
                // throw new ArgumentException("Erro Teste.");
                
                await trasaction.ConfirmarAsync();
            }
            catch (Exception)
            {
                await trasaction.ReverterAsync();
                
                throw;
            }
        }
    }
    public async Task inserir(Pessoa pessoa)
    {
        using (var trasaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try 
            {
                await _pessoaContext.Pessoas.AddAsync(pessoa);

                await _pessoaContext.SaveChangesAsync();
                
                // Simular erro
                // throw new ArgumentException("Erro Teste.");
                
                trasaction.Complete();
            }
            catch (Exception)
            {
                trasaction.Dispose();
                throw;
            }
        }
    }
}