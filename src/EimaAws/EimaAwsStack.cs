using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace EimaAws;

public class EimaAwsStack : Stack
{
    
    internal EimaAwsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        EimaAws.LambdaFunctions.Setup.HelloLambda(this);
        //
        // var helloWorldLambdaFunction = new Function(this, "HelloWorldFunction", new FunctionProps
        // {
        //     FunctionName = "HelloWorldFunctionTest",
        //     
        //     Runtime = Runtime.DOTNET_8,
        //     MemorySize = 1024,
        //     //LogRetention = RetentionDays.ONE_DAY,
        //     //
        //     Handler = "HelloWorldLambda::HelloWorldLambda.Function::FunctionHandler",
        //     Code = Code.FromAsset("./src/HelloWorldLambda"),
        //     Environment = new System.Collections.Generic.Dictionary<string, string> {
        //         { "MY_VARIABLE", "some value" }
        //     },
        //     Description = "A sample testing lambda function in EimaAwsStack"
        //     
        // });
        //
        // var functionUrl = new FunctionUrl(this, "MyFunctionUrl", new FunctionUrlProps
        // {
        //     Function = helloWorldLambdaFunction,
        //     AuthType = FunctionUrlAuthType.NONE,
        //     Cors = new FunctionUrlCorsOptions()
        //     {
        //         AllowedOrigins = ["*"],
        //         AllowedHeaders = ["*"],
        //         AllowedMethods = [ HttpMethod.ALL ],
        //     }
        //     //Authorization = FunctionUrlAuthType.NONE, // Or IAM if you need authentication
        //     // Cors = new FunctionUrlCorsOptions // Optional: Configure CORS
        //     // {
        //     //     AllowedOrigins = new { "*" }, // Replace with your allowed origins
        //     //     AllowedMethods = new { HttpMethod.ALL }, // Or specify methods
        //     //     AllowedHeaders = new { "*" } // Or specify headers
        //     // }
        // });
        //
        //
        // new CfnOutput(this, "LambdaFunctionArn", new CfnOutputProps {
        //     Value = helloWorldLambdaFunction.FunctionArn
        // });
        //
        // new CfnOutput(this, "FunctionUrlOutput", new CfnOutputProps
        // {
        //     Value = functionUrl.Url,
        //     Description = "URL of the Lambda function"
        // });
    }
}