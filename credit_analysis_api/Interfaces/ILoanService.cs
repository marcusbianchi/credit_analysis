using System.Threading.Tasks;
using credit_analysis_api;

public interface ILoanService
{
    Task<string> AddLoanRequestToQueue(Loan loan);
}