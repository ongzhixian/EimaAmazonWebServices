using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using ProjectLambdas.Models;

namespace ProjectLambdas.Services;

public interface IProjectService
{
    void PutProject(Project project); // Add/Update
    void DeleteProject(string projectId);
    
    void GetProject(string projectId);
    
    void GetAllProjects();
    
}

public class ProjectService : IProjectService
{
    private readonly IAmazonDynamoDB dynamoDbClient;

    // Reference: EimaAws.DynamoDbTables.Setup
    private readonly string tableName = "project"; 
    
    public ProjectService(IAmazonDynamoDB dynamoDbClient)
    {
        this.dynamoDbClient = dynamoDbClient;
    }

    public async Task WriteToDynamoDB(Project input)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            { "Id", new AttributeValue { S = input.Name } }, // Example: String primary key
            { "Version", new AttributeValue { S = input.Version } }, // Example: String attribute
            
            //{ "Value", new AttributeValue { N = input.Value.ToString() } }, // Example: Number attribute
            // Add more attributes as needed based on your DynamoDB table schema
        };

        var request = new PutItemRequest
        {
            TableName = tableName,
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

    public void PutProject(Project project)
    {
        
        //ConvertToDynamoDbItem(project);

        dynamoDbClient.PutItemAsync(this.tableName, ConvertToDynamoDbItem(project));
    }

    private Dictionary<string, AttributeValue> ConvertToDynamoDbItem(Project project)
    {
        throw new NotImplementedException();
    }

    public void DeleteProject(string projectId)
    {
        throw new NotImplementedException();
    }

    public void GetProject(string projectId)
    {
        throw new NotImplementedException();
    }

    public void GetAllProjects()
    {
        throw new NotImplementedException();
    }
}