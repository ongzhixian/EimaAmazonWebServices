using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging.Abstractions;
using ProjectLambdas.Models;

namespace ProjectLambdas.Services;

public class ProjectService
{
    private readonly IAmazonDynamoDB dynamoDbClient;

    // Reference: EimaAws.DynamoDbTables.Setup
    private readonly string TableName = "project"; 
    public ProjectService(IAmazonDynamoDB dynamoDbClient)
    {
        this.dynamoDbClient = dynamoDbClient;
        //ILogger logger;
    }

    public async Task WriteToDynamoDB(Project input)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            { "Id", new AttributeValue { S = input.ProjectName } }, // Example: String primary key
            { "Version", new AttributeValue { S = input.ProjectVersion } }, // Example: String attribute
            
            //{ "Value", new AttributeValue { N = input.Value.ToString() } }, // Example: Number attribute
            // Add more attributes as needed based on your DynamoDB table schema
        };

        var request = new PutItemRequest
        {
            TableName = TableName,
            Item = item
        };

        await dynamoDbClient.PutItemAsync(request);
    }

    public string TestPost(APIGatewayProxyRequest input)
    {
        var x = $"{input.HttpMethod} {input.Path} {input.Body}";
        Console.WriteLine($"INPUT IS: {System.Text.Json.JsonSerializer.Serialize(input)}");
        return x;
    }

    public async Task<OperationResult> RegisterProject(ILambdaRequest request)
    {
        OperationResult result = new OperationResult();
        var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        Project? project = null;
        
        try
        {
            Console.WriteLine("RegisterProject deserializing");
            project = JsonSerializer.Deserialize<Project>(request.Body, jsonSerializerOptions);
        }
        catch (Exception e)
        {
            Console.WriteLine("deserializing error");
            result.Success = false;
            result.Message = $"Unable to deserialize body [{request.Body}]; Message: {e.Message}";
        }
        
        Console.WriteLine("Project deserialized");
        if (project == null)
        {
            Console.WriteLine("Project deserialized but null");
            return OperationResult.Failed($"Deserialized project object is null ; request body is [{request.Body}]");
        }

        try
        {
            Console.WriteLine("Project wrtting to dynamodb");
            await WriteToDynamoDB(project);
            result.Success = true;
            result.Message = $"Project successfully registered";
        }
        catch (Exception e)
        {
            Console.WriteLine("Project write to dynamodb failed");
            result.Success = false;
            result.Message = $"Unable to WriteToDynamoDB [{request.Body}]; Message: {e.Message}";
        }
        
        return result;
    }
}