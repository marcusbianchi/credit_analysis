using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IScorePolicy
    {
        /// <summary>
        /// Process Score Policy async.
        /// </summary>
        /// <param name="loan">Received Loan from queue</param>
        /// <returns>Loan enriched with Policy Result</returns>
        Task<Loan> ProcessScorePolicy(Loan loan);

    }
}