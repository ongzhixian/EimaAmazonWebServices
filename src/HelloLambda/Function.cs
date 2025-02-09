using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloLambda;

public class Function
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    // public string FunctionHandler(string input, ILambdaContext context)
    // {
    //     return input.ToUpper();
    // }
    
    // 
    //public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        //context.Logger.LogLine($"Function handler input: {input}").;
        
        //string message = input.QueryStringParameters["message"];
        //string message = $"Check: Input is null: {input.ContainsKey("message")}";
        string message = "Default message XXX";
        
        if (input == null)
        {
            message = "Input is null";
        }
        else
        {
            message = $"Input contains {input.GetType().ToString()}";
        }
    
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"Received message: {message}",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };
    
        return response;
    }
}


