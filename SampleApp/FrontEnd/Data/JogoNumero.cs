using System.Text.Json.Serialization;
namespace FrontEnd.Data.jogoNumero;


public class RespostaIniciar
{
    [JsonPropertyName("jogoId")]
    public string? JogoId { get; set; }
}

public class RespostaPalpite
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("mensagem")]
    public string? Mensagem { get; set; }
}