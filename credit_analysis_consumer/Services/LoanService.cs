using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;
using Microsoft.Extensions.Logging;

namespace credit_analysis_consumer.Services
{
    public class LoanService : ILoanService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _loanURL;
        private readonly ILogger<LoanService> _logger;
        public LoanService(IHttpClientFactory clientFactory, ILogger<LoanService> logger)
        {
            _loanURL = Environment.GetEnvironmentVariable("LOAN_URL");
            _clientFactory = clientFactory;
            _logger = logger;
        }
        public async Task UpdateLoanRequest(RequestResult result, RequestPolicyResult? refused_policy, decimal amount, int? terms, string id)
        {
            LoanRequest loanRequest = new LoanRequest
            {
                id = id,
                status = RequestStatus.completed,
                result = result,
                refused_policy = refused_policy,
                amount = amount,
                terms = terms
            };
            var request = new HttpRequestMessage(HttpMethod.Put, _loanURL + "/api/" + id);

            var json = JsonSerializer.Serialize(loanRequest);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                _logger.LogError("Error Loan: {Loan}", json);

        }
    }
}