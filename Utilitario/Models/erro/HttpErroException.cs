using System.Net;
using System.Text;
using System.Text.Json;

namespace Utilitario.Models.erro;

/// <summary>
/// Essa class define um objeto de erro que caso acinado deve-se ser exibido diretamente ao cliente.
/// </summary>
public class HttpErroException : Exception
{
    /// <summary>
    /// Status code da exception
    /// </summary>
    public HttpStatusCode statusCode { get; set; }
    /// <summary>
    /// Dados adicionais que desejá informa no erro, como a descrição dos campo que acionaram esse erro.
    /// </summary>
    public object? data { get; set; }
    /// <summary>
    /// Tipo do objeto de erro
    /// </summary>
    /// <value></value>
    public string tipo { get; set; }
    public HttpErroException(
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError, 
        string mensagem = "Ocorreu um erro, tente novamente mais tarde.", 
        object? data = null,
        string tipo = "HttpErroException"
    ) : base(mensagem) {
        this.statusCode = statusCode;
        this.data = data;
        this.tipo = tipo;
    }
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("{");
        stringBuilder.AppendLine($" \"status\" : {(int)statusCode},");
        stringBuilder.AppendLine($" \"statusCode\" : \"{statusCode}\",");
        stringBuilder.AppendLine($" \"mensagem\" : \"{Message}\"");
        
        if (data != null)
            stringBuilder.AppendLine($", \"informacoes\" : {(data.GetType().IsPrimitive || data is string ? data : JsonSerializer.Serialize(data))}");

        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}
