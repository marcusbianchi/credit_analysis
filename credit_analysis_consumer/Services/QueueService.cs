using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using credit_analysis_consumer.Interfaces;
using Microsoft.Extensions.Logging;

namespace credit_analysis_consumer.Services
{
    public class QueueService : IQueueService
    {
        private readonly AmazonSQSClient _amazonSQSClient;
        private readonly string _queueURL;
        private readonly ILogger _logger;

        public QueueService(ILogger<QueueService> logger)
        {
            var sqsConfig = new AmazonSQSConfig();
            sqsConfig.ServiceURL = Environment.GetEnvironmentVariable("SQS_SERVICE_URL");
            _amazonSQSClient = new AmazonSQSClient(sqsConfig);
            _queueURL = Environment.GetEnvironmentVariable("QUEUE_URL");
            _logger = logger;
        }
        public async Task<IList<Loan>> GetFromQueue()
        {
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();

            receiveMessageRequest.QueueUrl = _queueURL;
            receiveMessageRequest.MaxNumberOfMessages = 10;
            var result = await _amazonSQSClient.ReceiveMessageAsync(receiveMessageRequest);
            var loanList = new List<Loan>();
            foreach (var item in result.Messages)
            {
                try
                {
                    Amazon.SQS.Model.DeleteMessageRequest deleteReq = new Amazon.SQS.Model.DeleteMessageRequest();
                    deleteReq.QueueUrl = _queueURL;
                    deleteReq.ReceiptHandle = item.ReceiptHandle;
                    var deleteResp = await _amazonSQSClient.DeleteMessageAsync(deleteReq);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error: {Message}", ex.ToString());
                }
                var loan = JsonSerializer.Deserialize<Loan>(item.Body);
                loanList.Add(loan);
            }
            return loanList;
        }
    }
}