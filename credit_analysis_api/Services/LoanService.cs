using System;
using System.Text.Json;
using System.Threading.Tasks;
using credit_analysis_api;

public class LoanService : ILoanService
{
    private readonly IQueueService _queueService;
    private readonly IDBService _DBService;
    public LoanService(IQueueService queueService, IDBService DBService)
    {
        _queueService = queueService;
        _DBService = DBService;
    }

    public async Task<string> AddLoanRequestToQueue(Loan loan)
    {
        var loanJSON = JsonSerializer.Serialize(loan);
        var addedToQueue = await _queueService.AddToQueue(loanJSON);
        if (addedToQueue)
        {
            LoanRequest loanRequest = new LoanRequest
            {
                id = loan.id,
                status = RequestStatus.processing,
                result = null,
                refused_policy = null,
                amount = null,
                terms = null,
            };
            await _DBService.Insert(loanRequest);
            return loan.id;
        }
        return "";

    }
}