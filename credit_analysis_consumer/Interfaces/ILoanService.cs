using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface ILoanService
    {
        Task UpdateLoanRequest(RequestResult result, RequestPolicyResult? refused_policy, decimal amount, int? terms, string id);
    }
}