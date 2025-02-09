using System.Net;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace Eima.ConfigurationServices;

public interface IConfigurationService
{
    Task<Dictionary<string, AttributeValue>> GetConfiguration(string key);

    Task<bool> PutConfiguration(string key, string value, 
        DateTime? effectiveDateTime = null,
        DateTime? expiryDateTime = null);
    
    Task<bool> DeleteConfiguration(string key);
}


public class ConfigurationService : IConfigurationService
{
    private readonly IAmazonDynamoDB dynamoDb;
    
    private const string configurationTableName = "configuration";
    
    public ConfigurationService(IConfiguration configuration)
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
    
    public async Task<Dictionary<string, AttributeValue>> GetConfiguration(string key)
    {
        var request = new GetItemRequest
        {
            TableName = configurationTableName,
            Key = new Dictionary<string,AttributeValue>() { { "Id", new AttributeValue { N = "202" } } },
        };
        
        var getItemResponse = await dynamoDb.GetItemAsync(request);

        return getItemResponse.Item;
    }

    public async Task<bool> PutConfiguration(string key, string value, DateTime? effectiveDateTime = null, DateTime? expiryDateTime = null)
    {
        var request = new PutItemRequest
        {
            TableName = configurationTableName,
            
            Item = new Dictionary<string, AttributeValue>()
            {
                { "Id", new AttributeValue { S = key }}
                , { "EffectiveDateTime", new AttributeValue { S = (effectiveDateTime.HasValue ? effectiveDateTime.Value.ToString("o") : DateTime.UtcNow.ToString("o")) }}
                , { "ExpiryDateTime", 
                    expiryDateTime.HasValue 
                    ? new AttributeValue { S =  expiryDateTime.Value.ToString("o") }
                    : new AttributeValue { NULL = true} 
                }
                , { "DataType", new AttributeValue { S = "string" }} // Hard code to string (because we only do string values) 
                , { "DataValue", new AttributeValue { S = value }}
                
                // (expiryDateTime.HasValue ? new AttributeValue { S =  effectiveDateTime.Value.ToString("o") } : new AttributeValue()) 
                // new AttributeValue { S = (effectiveDateTime.HasValue ? effectiveDateTime.Value.ToString("o") : DateTime.UtcNow.ToString("o")) }}
                // { "DataType", new AttributeValue { S = "Book 201 Title" }},
                // { "ISBN", new AttributeValue { S = "11-11-11-11" }},
                // { "Price", new AttributeValue { S = "20.00" }},
                // {
                //     "Authors",
                //     new AttributeValue
                //         { SS = new List<string>{"Author1", "Author2"}   }
                // }
            }
        };

        try
        {
            var putItemResponse = await dynamoDb.PutItemAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        

        return true;
        //return putItemResponse.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteConfiguration(string key)
    {
        var request = new DeleteItemRequest
        {
            TableName = configurationTableName,
            Key = new Dictionary<string,AttributeValue>() { { "Id", new AttributeValue { N = key } } },
        };

        var deleteItemResponse = await dynamoDb.DeleteItemAsync(request);
        
        return deleteItemResponse.HttpStatusCode == HttpStatusCode.OK;
    }
}
