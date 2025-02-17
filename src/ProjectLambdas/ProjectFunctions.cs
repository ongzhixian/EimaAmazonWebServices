using System.Net;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ProjectLambdas.Models;
using ProjectLambdas.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ProjectLambdas;

// Functionality Blueprint (aka, we want to have ability to: 
// 1. RegisterNewProject        / 
// 1. UpdateProjectDetails      / 
// 1. GetProjectList              / 
// 1. GetProject                /
public class ProjectFunctions
{
    private readonly IAmazonDynamoDB dynamoDbClient;
    private readonly ProjectService projectService;
    public ProjectFunctions(IAmazonDynamoDB dynamoDbClient)
    {
        this.dynamoDbClient = dynamoDbClient;
        projectService = new ProjectService(this.dynamoDbClient);
    }
    
    // Default constructor (for Lambda execution)
    public ProjectFunctions() : this(new AmazonDynamoDBClient()) { }
    
    public async Task<APIGatewayProxyResponse> RegisterNewProject(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var logger = context.Logger;
        
        try
        {
            // return new APIGatewayProxyResponse
            // {
            //     StatusCode = (int)HttpStatusCode.OK,
            //     Body = System.Text.Json.JsonSerializer.Serialize(new OperationResult
            //     {
            //         Success = true,
            //         Message = "All ok"
            //     }),
            //     Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            // };    
            //

            logger.LogInformation("Registering new project");
            
            var operationResult = await projectService.RegisterProject(
                new FunctionUrlRequest(
                    request.Headers
                    , request.QueryStringParameters
                    , request.Body));

            APIGatewayProxyResponse response;
            
            Console.WriteLine("RegisterNewProject check operationresult");
            
            if (operationResult.Success)
            {
                Console.WriteLine("Operationresult success");
                
                response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = System.Text.Json.JsonSerializer.Serialize(operationResult),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };    
            }
            else
            {
                
                Console.WriteLine("Operationresult failure");
                
                // response = new APIGatewayProxyResponse
                // {
                //     StatusCode = (int)HttpStatusCode.InternalServerError,
                //     Body = $"Placeholder for RegisterNewProject [{operationResult.Message}]",
                //     Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                // };
                response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = System.Text.Json.JsonSerializer.Serialize(operationResult),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
            
            return response;
        }
        catch (Exception e)
        {
            
            Console.WriteLine("Operationresult error");
            
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = System.Text.Json.JsonSerializer.Serialize(new OperationResult
                {
                    Success = false,
                    Message = e.Message,
                }),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                // Body = e.Message,
                // Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
            
            return response;
        }
    }
    
    public APIGatewayProxyResponse GetProjectList(APIGatewayProxyRequest input, ILambdaContext context)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"Placeholder for GetProjectList",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };
    
        return response;
    }
}
