using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Apigatewayv2;

namespace EimaAws.ApiGateway;

internal class Setup
{
    public static void EimaApiGateway(EimaAwsStack eimaAwsStack)
    {
        var apiName = "EimaHttpApi";

        var httpApi = new HttpApi(eimaAwsStack, apiName, new HttpApiProps
        {
            ApiName = apiName,
            Description = "Eima HTTP API",

            CorsPreflight = new CorsPreflightOptions // Configure CORS here
            {
                AllowOrigins = new[] { "*" }, // Or specify your allowed origins (e.g., "https://example.com")
                AllowHeaders = new[] { "*" }, // Or specify allowed headers (e.g., "Content-Type", "Authorization")
                //AllowMethods = new[] { HttpMethod.GET, HttpMethod.POST, HttpMethod.OPTIONS, HttpMethod.DELETE, HttpMethod.PUT }, // Allowed HTTP methods
                AllowMethods = new[] { CorsHttpMethod.ANY }, 
                MaxAge = Duration.Days(1) // Optional: Maximum age for preflight responses
            }

        });

        httpApi.AddRoutes(new AddRoutesOptions
        {
            Path = "/authentication",
            Methods = [HttpMethod.POST],
        });

        // Optional: Add more routes as needed
        //httpApi.AddRoute(new HttpRouteProps
        //{
        //    Path = "/authentication",
        //    Methods = new[] { HttpMethod.POST },
        //    Integration = lambdaIntegration // You can reuse the integration or create a new one
        //});
        

        // Optional: Output the API endpoint URL
        new CfnOutput(eimaAwsStack, $"{apiName}HttpApiEndpoint", new CfnOutputProps
        {
            Value = httpApi.Url,
            Description = "URL of the HTTP API endpoint"
        });
    }
}
