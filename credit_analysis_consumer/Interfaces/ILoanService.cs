using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface ILoanService
    {
        /// <summary>
        /// Update Loan Request on the database async.
        /// </summary>
        /// <param name="result">Request Result</param>
        /// <param name="refused_policy">Request Policy Result  or null</param>
        /// <param name="amount">Amount requested</param>
        /// <param name="terms">number of terms approved after processing or null</param>
        /// <param name="id">Request ID</param>
        /// <returns></returns>
        Task UpdateLoanRequest(RequestResult result, RequestPolicyResult? refused_policy, decimal amount, int? terms, string id);
    }
}