using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IProcessRequestService
    {
        Task ProcessRequestsFromQueue();
        Task<(Loan, RequestPolicyResult?)> ProcessLoan(Loan loan);
        Task UpdateLoanRequest(RequestResult result, RequestPolicyResult? refused_policy, decimal amount, int terms, string id);
    }
}