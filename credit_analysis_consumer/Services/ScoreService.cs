using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;

namespace credit_analysis_consumer.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _scoreURL;
        private readonly string _scoreKey;

        public ScoreService(IHttpClientFactory clientFactory)
        {
            _scoreURL = Environment.GetEnvironmentVariable("SCORE_URL");
            _scoreKey = Environment.GetEnvironmentVariable("SCORE_KEY");
            _clientFactory = clientFactory;

        }
        public async Task<int?> GetScore(string cpf)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _scoreURL);
            request.Headers.Add("x-api-key", _scoreKey);

            var json = JsonSerializer.Serialize(new { cpf = cpf });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ScoreReponse>(responseStream);
                return result.score;
            }
            return null;
        }
    }

    class ScoreReponse
    {
        public int score { get; set; }
    }
}