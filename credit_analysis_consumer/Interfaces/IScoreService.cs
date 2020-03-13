using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IScoreService
    {
        Task<int?> GetScore(string cpf);
    }
}