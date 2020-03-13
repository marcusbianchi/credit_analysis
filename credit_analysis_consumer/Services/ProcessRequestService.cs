using System;
using System.Text.Json;
using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;
using Microsoft.Extensions.Logging;

namespace credit_analysis_consumer.Services
{
    public class ProcessRequestService : IProcessRequestService
    {
        private readonly IAgePolicy _agePolicy;
        private readonly IScorePolicy _scorePolicy;
        private readonly ICommitmentPolicy _commitmentPolicy;
        private readonly IQueueService _queueService;
        private readonly ILoanService _loanService;
        private readonly ILogger<ProcessRequestService> _logger;

        public ProcessRequestService(IAgePolicy agePolicy, IScorePolicy scorePolicy, ICommitmentPolicy commitmentPolicy,
            IQueueService queueService, ILoanService loanService, ILogger<ProcessRequestService> logger)
        {
            _agePolicy = agePolicy;
            _commitmentPolicy = commitmentPolicy;
            _scorePolicy = scorePolicy;
            _queueService = queueService;
            _loanService = loanService;
            _logger = logger;
        }

        public async Task<(Loan, RequestPolicyResult?)> ProcessLoan(Loan loan)
        {
            loan = _agePolicy.ProcessAgePolicy(loan);
            if (!loan.age_policy_result)
                return (loan, RequestPolicyResult.age);
            loan = await _scorePolicy.ProcessScorePolicy(loan);
            if (loan.score_policy_result == false || loan.score_policy_result == null)
                return (loan, RequestPolicyResult.score);
            loan = await _commitmentPolicy.ProcessCommitmentPolicy(loan);
            if (loan.commitment_policy_result == false || loan.commitment_policy_result == null)
                return (loan, RequestPolicyResult.commitment);
            return (loan, null);
        }

        public async Task ProcessRequestsFromQueue()
        {
            var loans = await _queueService.GetFromQueue();
            foreach (var item in loans)
            {
                var (loan, requestPolicyResult) = await ProcessLoan(item);
                RequestResult result = requestPolicyResult == null ? RequestResult.approved : RequestResult.refused;
                try
                {
                    await _loanService.UpdateLoanRequest(result, requestPolicyResult, (decimal)loan.amount, loan.commitment_terms_result, loan.id);
                }
                catch (Exception ex)
                {
                    var json = JsonSerializer.Serialize(loan);
                    _logger.LogError("Loan: {Loan}", json);
                    _logger.LogError("Error: {Message}", ex.ToString());
                }
            }
        }
    }
}