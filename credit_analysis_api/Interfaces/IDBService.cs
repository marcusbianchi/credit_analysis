using System.Threading.Tasks;
using credit_analysis_api;

public interface IDBService
{
    /// <summary>
    /// Insert LoanRequest to Database Async
    /// </summary>
    /// <param name="loan">Loan Request</param>
    /// <returns></returns>
    Task Insert(LoanRequest loan);

    /// <summary>
    /// Read LoanRequest from Database Async
    /// </summary>
    /// <param name="uuid">Get LoanRequest ID from DB</param>
    /// <returns>LoanRequest or null</returns>
    Task<LoanRequest> Read(string uuid);

    /// <summary>
    /// Update LoanRequest from Database Async
    /// </summary>
    /// <param name="uuid">Get LoanRequest ID from DB</param>
    /// <param name="loan">Loan Request</param>
    /// <returns>Updated LoanRequest</returns>
    Task<LoanRequest> Update(string uuid, LoanRequest loan);
}