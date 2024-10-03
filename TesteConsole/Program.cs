using Microsoft.EntityFrameworkCore;
using SistemaLib.Models;
using SistemaLib.Repository;

namespace TesteConsole;

class Program
{
    static async Task Main(string[] args)
    {        
        Console.WriteLine("Iniciar");

        Conexao conexao = new Conexao();

        UsuarioRepository usuarioRepository = new UsuarioRepository(conexao);

        Usuario usuario = new Usuario() {
            pessoa = new PessoaLib.Models.Pessoa() {
                nome = "Teste 001"
            },
            usuario = "Teste 001 - " + Guid.NewGuid().ToString(),
            senha = Guid.NewGuid().ToString()
        };

        await usuarioRepository.inserirEfTrasaction(usuario);
    }
}
