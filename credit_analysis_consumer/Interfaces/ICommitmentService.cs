using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface ICommitmentService
    {
        Task<double?> GetCommitment(string cpf);
    }
}