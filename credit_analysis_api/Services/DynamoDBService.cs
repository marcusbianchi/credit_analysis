using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using credit_analysis_api;

/// <summary>
/// DBService implement for DynamoDB
/// </summary>
public class DynamoDBService : IDBService
{
    private readonly Table _table;

    public DynamoDBService()
    {
        var sqsConfig = new AmazonDynamoDBConfig();
        sqsConfig.ServiceURL = Environment.GetEnvironmentVariable("DYNAMO_SERVICE_URL");
        var amazonDynamoDBClient = new AmazonDynamoDBClient(sqsConfig);
        var tableName = Environment.GetEnvironmentVariable("DYNAMO_TABLE");
        _table = Table.LoadTable(amazonDynamoDBClient, tableName);
    }
    public async Task Insert(LoanRequest loan)
    {
        var loanJSON = JsonSerializer.Serialize(loan);

        var item = Document.FromJson(loanJSON);

        await _table.PutItemAsync(item);
    }

    public async Task<LoanRequest> Read(string uuid)
    {
        var item = await _table.GetItemAsync(uuid);
        var jsonText = item.ToJson();
        Console.Write(jsonText);
        var loanRequest = JsonSerializer.Deserialize<LoanRequest>(jsonText);
        return loanRequest;

    }

    public async Task<LoanRequest> Update(string uuid, LoanRequest loan)
    {
        var request = await Read(uuid);
        if (request == null)
            return null;
        var loanJSON = JsonSerializer.Serialize(loan);

        var item = Document.FromJson(loanJSON);

        var result = await _table.UpdateItemAsync(item);

        return loan;
    }
}