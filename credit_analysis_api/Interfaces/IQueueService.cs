using System.Threading.Tasks;

public interface IQueueService
{
    Task<bool> AddToQueue(string loanJSON);
}