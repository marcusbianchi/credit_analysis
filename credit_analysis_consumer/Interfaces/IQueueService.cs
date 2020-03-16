using System.Collections.Generic;
using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IQueueService
    {
        /// <summary>
        /// Get Items from queue
        /// </summary>
        /// <returns>List of Loan from queues.</returns>
        Task<IList<Loan>> GetFromQueue();
    }
}