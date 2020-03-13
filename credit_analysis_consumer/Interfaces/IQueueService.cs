using System.Collections.Generic;
using System.Threading.Tasks;

namespace credit_analysis_consumer.Interfaces
{
    public interface IQueueService
    {
        Task<IList<Loan>> GetFromQueue();
    }
}