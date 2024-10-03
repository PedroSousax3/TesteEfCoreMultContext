using PessoaLib.Models;

namespace SistemaLib.Models;

public class Usuario
{
    public int codigo { get; set; }
    public int codPessoa { get; set; }
    public virtual Pessoa? pessoa { get; set; }
    public string usuario { get; set; } = string.Empty;
    public string senha { get; set; } = string.Empty;
}
