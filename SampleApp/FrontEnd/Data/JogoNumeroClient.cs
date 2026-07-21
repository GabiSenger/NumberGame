namespace FrontEnd.Data.jogoNumero;
public class JogoNumeroClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<JogoNumeroClient> _logger;

    public JogoNumeroClient(HttpClient httpClient, ILogger<JogoNumeroClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<RespostaIniciar?> inicarJogoAsync() {
        try{
            var resposta = await _httpClient.PostAsJsonAsync("/api/iniciar", new { });
            return await resposta.Content.ReadFromJsonAsync<RespostaIniciar>();
        }catch(Exception e)
        {
            _logger.LogError(e, "Erro ao tentar iniciar um novo jogo no servidor.");
            return null;
        }
    }

        public async Task<RespostaPalpite?> enviarPalpiteAsync(string jogoId, int valor) {
        try{
            return await _httpClient.GetFromJsonAsync<RespostaPalpite>($"api/palpite?jogoId={jogoId}&valor={valor}");
        }catch(Exception e)
        {
            _logger.LogError(e, "Erro ao tentar analisar palpite.");
            return null;
        }
    }
}