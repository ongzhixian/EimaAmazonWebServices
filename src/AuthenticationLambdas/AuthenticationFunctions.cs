using Amazon.Lambda.APIGatewayEvents;
using System.Net;

using Amazon.Lambda.Core;
using BaseHttpLambda;
using AuthenticationLambdas.Models;
using System.Text.Json;
using AuthenticationLambdas.ServiceProxies;
using Eima.AuthenticationServices.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AuthenticationLambdas;

public class AuthenticationFunctions
{
    private readonly JsonSerializerOptions jsonSerializerOptionsForWeb = new(JsonSerializerDefaults.Web);
    private readonly AuthenticationService authenticationService = new AuthenticationService();
    private readonly JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();

    public async Task<APIGatewayProxyResponse> AuthenticateCredentialsHandlerAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogInformation("{AuthenticateCredentialsHandlerRequest}", JsonSerializer.Serialize(request));

        var userCredentials = JsonSerializer.Deserialize<UserCredentials>(request.Body, jsonSerializerOptionsForWeb);

        if (userCredentials is null) return HttpResponse.BadRequest(request.Body);

        var credentialsAreValid = await authenticationService.ValidateUserCredentialsAsync(userCredentials);

        string? jwt = null;

        if (credentialsAreValid) jwt = jsonWebTokenService.CreateToken(userCredentials.Username);

        return HttpResponse.Ok(new AuthenticateCredentialsResponse(credentialsAreValid, jwt));
    }

    private void CookieAuth()
    {

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

        //return HttpResponse.Ok(cookie: cookie);

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
}
