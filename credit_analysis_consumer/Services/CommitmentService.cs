using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;

namespace credit_analysis_consumer.Services
{
    public class CommitmentService : ICommitmentService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _URL;
        private readonly string _Key;

        public CommitmentService(IHttpClientFactory clientFactory)
        {
            _URL = Environment.GetEnvironmentVariable("COMMITMENT_URL");
            _Key = Environment.GetEnvironmentVariable("COMMITMENT_KEY");
            _clientFactory = clientFactory;

        }
        public async Task<double?> GetCommitment(string cpf)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _URL);
            request.Headers.Add("x-api-key", _Key);

            var json = JsonSerializer.Serialize(new { cpf = cpf });
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<CommitmentReponse>(responseStream);
                return result.commitment;
            }
            return null;
        }
    }

    class CommitmentReponse
    {
        public double commitment { get; set; }
    }
}