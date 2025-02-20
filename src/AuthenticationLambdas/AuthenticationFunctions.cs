using Amazon.Lambda.APIGatewayEvents;
using System.Net;

using Amazon.Lambda.Core;
using BaseHttpLambda;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AuthenticationLambdas;

public class AuthenticationFunctions
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public string AuthenticateCredentialsHandler(string input, ILambdaContext context)
    {
        return input.ToUpper();
    }


    public APIGatewayProxyResponse AuthenticateCredentialsHandler(APIGatewayProxyRequest input, ILambdaContext context)
    {
        //context.Logger.LogLine($"Function handler input: {input}").;

        //var cookieAttributes = new List<string>
        // {
        //     "myCookie=myValue",
        //     "Expires=Wed, 21 Oct 2024 07:28:00 GMT",
        //     "Path=/",
        //     "HttpOnly",
        //     "SameSite=Strict",
        //     "Secure" // Only include if using HTTPS
        // };
        //response.Headers["Set-Cookie"] = string.Join("; ", cookieAttributes);

        CookieCollection cookieCollection = new CookieCollection();
        Cookie cookie = new Cookie();
        cookie.Name = "myCookie";
        cookie.Value = "myCookieValue";
        cookie.Expires = DateTime.Now.AddHours(2);
        cookie.Path = "/";
        cookie.HttpOnly = true;
        cookie.Secure = true;

        return HttpResponse.Ok(cookie: cookie);

        ////string message = input.QueryStringParameters["message"];
        ////string message = $"Check: Input is null: {input.ContainsKey("message")}";
        //string message = "Default message XXX";

        //if (input == null)
        //{
        //    message = "Input is null";
        //}
        //else
        //{
        //    message = $"Input contains {input.GetType().ToString()}";
        //}

        //var response = new APIGatewayProxyResponse
        //{
        //    StatusCode = (int)HttpStatusCode.OK,
        //    Body = $"Received message: {message}",
        //    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        //};


    }
}
