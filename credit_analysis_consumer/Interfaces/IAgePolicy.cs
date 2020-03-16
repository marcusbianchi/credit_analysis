using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IAgePolicy
    {
        /// <summary>
        /// Process Age Policy async.
        /// </summary>
        /// <param name="loan">Received Loan from queue</param>
        /// <returns>Loan enriched with Policy Result</returns>
        Loan ProcessAgePolicy(Loan loan);
    }
}