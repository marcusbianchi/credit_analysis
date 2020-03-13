using System.Threading.Tasks;
using credit_analysis_consumer.Interfaces;

namespace credit_analysis_consumer.Services
{
    public class ProcessRequestService : IProcessRequestService
    {
        private readonly IAgePolicy _agePolicy;
        private readonly IScorePolicy _scorePolicy;
        private readonly ICommitmentPolicy _commitmentPolicy;
        private readonly IQueueService _queueService;
        public ProcessRequestService(IAgePolicy agePolicy, IScorePolicy scorePolicy, ICommitmentPolicy commitmentPolicy, IQueueService queueService)
        {
            _agePolicy = agePolicy;
            _commitmentPolicy = commitmentPolicy;
            _scorePolicy = scorePolicy;
            _queueService = queueService;
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

                await UpdateLoanRequest(result, requestPolicyResult, (decimal)loan.amount, (int)loan.commitment_terms_result, loan.id);
            }
        }

        public Task UpdateLoanRequest(RequestResult result, RequestPolicyResult? refused_policy, decimal amount, int terms, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}