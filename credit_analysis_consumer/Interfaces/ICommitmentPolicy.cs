using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface ICommitmentPolicy
    {
        Task<Loan> ProcessCommitmentPolicy(Loan loan);
    }
}