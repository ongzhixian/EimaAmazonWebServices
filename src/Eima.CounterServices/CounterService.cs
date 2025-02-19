using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace Eima.CounterServices;

public interface ICounterService
{
    Task<bool> CreateCounterAsync(string counterName);
    
    Task<bool> AddToCounterAsync(string counterName, sbyte valueToAdd);

    Task<bool> CounterExistsAsync(string counterName);

    Task<bool> RemoveCounterAsync(string counterName);

    Task<uint> GetCounterValueAsync(string counterName);
}

public class CounterService : ICounterService
{
    private readonly IAmazonDynamoDB dynamoDb;

    private const string tableName = "counter";

    public CounterService(IConfiguration configuration)
    {
        BasicAWSCredentials awsCredentials = new BasicAWSCredentials(
            configuration["default_aws_access_key_id"]
            , configuration["default_aws_secret_access_key"]);

        AmazonDynamoDBConfig awsConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.USEast1
        };

        AmazonDynamoDBClient awsDynamoDbClient = new AmazonDynamoDBClient(awsCredentials, awsConfig);

        dynamoDb = awsDynamoDbClient;
    }

    public CounterService(IAmazonDynamoDB amazonDynamoDB) => dynamoDb = amazonDynamoDB;

    public async Task<bool> CounterExistsAsync(string counterName)
    {
        QueryRequest queryRequest = new QueryRequest
        {
            TableName = tableName,
            KeyConditionExpression = "Id = :id",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":id", new AttributeValue { S = counterName} }
            }
        };

        var queryResponse = await dynamoDb.QueryAsync(queryRequest);

        return queryResponse.HttpStatusCode == System.Net.HttpStatusCode.OK && queryResponse.Count > 0;
    }

    public async Task<uint> GetCounterValueAsync(string counterName)
    {
        QueryRequest queryRequest = new QueryRequest
        {
            TableName = tableName,
            KeyConditionExpression = "Id = :id",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":id", new AttributeValue { S = counterName} }
            },
        };

        var queryResponse = await dynamoDb.QueryAsync(queryRequest);

        if (queryResponse.HttpStatusCode == System.Net.HttpStatusCode.OK && queryResponse.Count > 0)
        {
            List<Dictionary<string, AttributeValue>> recordList = queryResponse.Items;
            Dictionary<string, AttributeValue>? record = recordList.FirstOrDefault(dictRec => dictRec["Id"].S.Equals(counterName, StringComparison.InvariantCultureIgnoreCase));

            if (record is null) return 0;

            if (record.TryGetValue("Value", out var value)) return uint.Parse(value.N);
        }

        return 0;
    }

    public async Task<bool> CreateCounterAsync(string counterName)
    {
        if (await CounterExistsAsync(counterName)) return true;

        var putItemResponse = await dynamoDb.PutItemAsync(tableName, new Dictionary<string, AttributeValue>()
        {
            { "Id", new AttributeValue { S = counterName } },
            { "Value", new AttributeValue { N = "0" } }
        });

        return putItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> RemoveCounterAsync(string counterName)
    {
        if (!await CounterExistsAsync(counterName)) return true;

        var deleteItemResponse = await dynamoDb.DeleteItemAsync(tableName, new Dictionary<string, AttributeValue>()
        {
            { "Id", new AttributeValue { S = counterName } },
        });

        return deleteItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> AddToCounterAsync(string counterName, sbyte valueToAdd = 1)
    {
        UpdateItemRequest updateItemRequest = new UpdateItemRequest
        {
            TableName = tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                { "Id", new AttributeValue { S = counterName } }
            },
            UpdateExpression = "ADD #value :value",
            ExpressionAttributeNames = new Dictionary<string, string>
            {
                { "#value", "Value" }
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":value", new AttributeValue { N = valueToAdd.ToString() } }
            }
        };

        var updateItemResponse = await dynamoDb.UpdateItemAsync(updateItemRequest);
        
        return updateItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }


}