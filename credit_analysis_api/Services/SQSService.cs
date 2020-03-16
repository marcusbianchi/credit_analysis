using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;

/// <summary>
/// Queue Service for AWS SQS
/// </summary>
public class SQSService : IQueueService
{
    private readonly AmazonSQSClient _amazonSQSClient;
    private readonly string _queueURL;

    public SQSService()
    {
        var sqsConfig = new AmazonSQSConfig();
        sqsConfig.ServiceURL = Environment.GetEnvironmentVariable("SQS_SERVICE_URL");
        _amazonSQSClient = new AmazonSQSClient(sqsConfig);
        _queueURL = Environment.GetEnvironmentVariable("QUEUE_URL");
    }


    public async Task<bool> AddToQueue(string loanJSON)
    {
        var sendMessageRequest = new SendMessageRequest();
        sendMessageRequest.QueueUrl = _queueURL;
        sendMessageRequest.MessageBody = loanJSON;
        sendMessageRequest.MessageGroupId = "loan";
        sendMessageRequest.MessageDeduplicationId = Guid.NewGuid().ToString();
        var result = await _amazonSQSClient.SendMessageAsync(sendMessageRequest);
        if (result.HttpStatusCode == HttpStatusCode.OK)
            return true;
        return false;

    }
}