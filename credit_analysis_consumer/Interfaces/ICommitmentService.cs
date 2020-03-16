using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface ICommitmentService
    {
        /// <summary>
        /// Get Commitment value from external API async.
        /// </summary>
        /// <param name="cpf">Requester CPF</param>
        /// <returns>Commitment value or null</returns>
        Task<double?> GetCommitment(string cpf);
    }
}