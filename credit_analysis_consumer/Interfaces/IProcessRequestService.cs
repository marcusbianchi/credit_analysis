using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IProcessRequestService
    {
        /// <summary>
        /// Get task from Queue and process it
        /// </summary>
        /// <returns></returns>
        Task ProcessRequestsFromQueue();

        /// <summary>
        /// Process all policies configured
        /// </summary>
        /// <param name="loan">Loan received from queue</param>
        /// <returns>Processing result</returns>
        Task<(Loan, RequestPolicyResult?)> ProcessLoan(Loan loan);
    }
}