using System.Collections.Generic;

using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AwsApigatewayv2Integrations;

namespace EimaAws.ApiGateway;

internal class Setup
{
    public static void EimaApiGateway(EimaAwsStack eimaAwsStack, List<AddRoutesOptions> addRoutesOptionsList)
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
            },
        });

        //var httpIntegration = new HttpIntegration(eimaAwsStack, "", new HttpIntegrationProps()
        //{
        //    Method = HttpMethod.ANY,
        //    IntegrationUri = "http://localhost"
        //});


        //var httpIntegration = new HttpIntegration("MyHttpIntegration", new HttpIntegrationProps
        //{
        //    // The URL of the HTTP endpoint you want to proxy to
        //    Url = "https://example.com", // Or your target URL
        //    // HTTP method to use when calling the target URL (important!)
        //    Method = HttpMethod.POST,  // Adjust to your needs (GET, POST, etc.)

        //    // Optional: Payload format version (if needed)
        //    PayloadFormatVersion = PayloadFormatVersion.VERSION_1_0, // Or Version_2_0 if required

        //    // Optional: Timeout for the integration
        //    Timeout = Duration.Seconds(30)
        //});

        //var mockEndpoint = "https://httpbin.org/get"; // Example mock



        //var mockIntegration = new HttpProxyIntegration("MockIntegration", new HttpProxyIntegrationProps
        //{
        //    Url = mockEndpoint
        //});

        HttpRouteIntegration httpRouteIntegration = new HttpUrlIntegration("google", "https://www.google.com/");

        httpApi.AddRoutes(new AddRoutesOptions
        {
            Path = "/authentication",
            Methods = [HttpMethod.POST],
            Integration = httpRouteIntegration
        });

        httpApi.AddRoutes(new AddRoutesOptions
        {
            Path = "/authentication",
            Methods = [HttpMethod.GET],
            Integration = httpRouteIntegration
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
