using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IScoreService
    {
        /// <summary>
        /// Get Score value from external API async.
        /// </summary>
        /// <param name="cpf">Requester CPF</param>
        /// <returns>Score value or null</returns>
        Task<int?> GetScore(string cpf);
    }
}