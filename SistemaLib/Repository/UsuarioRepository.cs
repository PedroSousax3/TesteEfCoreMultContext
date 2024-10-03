using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Middleware;
using PessoaLib.Models;
using PessoaLib.Repository;
using SistemaLib.Models;

namespace SistemaLib.Repository;

public class UsuarioRepository
{
    private IConexao _conexao;
    private readonly SistemaContext context;
    private readonly PessoaRepository pessoaRepository;
    public UsuarioRepository(IConexao conexao)
    {
        _conexao = conexao;
        context = conexao.GetDbContext<SistemaContext>(EnumContext.Sistema);
        pessoaRepository = new PessoaRepository(conexao);
    }
    public async Task inserirEfTrasaction(Usuario usuario)
    {
        using (var trasaction = _conexao.BeginTransaction())
        {
            try 
            {
                await pessoaRepository.inserirEfTrasaction(usuario.pessoa!);

                usuario.codPessoa = usuario.pessoa!.codigo;
                usuario.pessoa = null;
                
                // Simular erro
                // throw new ArgumentException("Erro Teste.");
                
                await context.Usuarios.AddAsync(usuario);

                await context.SaveChangesAsync();

                await trasaction.ConfirmarAsync();
            }
            catch (Exception)
            {
                await trasaction.ReverterAsync();
                
                throw;
            }
        }
    }
    public async Task inserir(Usuario usuario)
    {
        try 
        {
            using (var trasaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            try 
            {
                await pessoaRepository.inserir(usuario.pessoa!);

                usuario.codPessoa = usuario.pessoa!.codigo;
                usuario.pessoa = null;

                //throw new ArgumentException("Erro Teste.");

                await context.Usuarios.AddAsync(usuario);

                await context.SaveChangesAsync();

                trasaction.Complete();
            }
            catch (Exception)
            {
                trasaction.Dispose();
                throw;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            throw;
        }
    }
}