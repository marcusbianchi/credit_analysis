using System.Threading.Tasks;
using credit_analysis_api;

public interface ILoanService
{
    /// <summary>
    /// Add Loan to processing Queue and to Database async
    /// </summary>
    /// <param name="loan">Loan Object</param>
    /// <returns>Loan Object ID</returns>
    Task<string> AddLoanRequestToQueue(Loan loan);
}