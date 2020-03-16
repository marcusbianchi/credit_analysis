using System.Threading.Tasks;

public interface IQueueService
{
    /// <summary>
    /// Add Loan to processing Queue async
    /// </summary>
    /// <param name="loanJSON">Loan JSON String</param>
    /// <returns>Request status</returns>
    Task<bool> AddToQueue(string loanJSON);
}