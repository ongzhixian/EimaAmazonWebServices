using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SES.Actions;

namespace EimaAws.LambdaFunctions;

public partial class Setup
{
    const string HelloLambdaFunctionName = "HelloLambdaFunction";
    const string RegisterNewProjectFunctionName = "register-project";
    const string GetProjectListFunctionName = "GetProjectList";
    
    public readonly List<string> LambdaList = [
        HelloLambdaFunctionName,
        RegisterNewProjectFunctionName,
        GetProjectListFunctionName
    ];
    
    
    public static void HelloLambda(EimaAwsStack eimaAwsStack)
    {
        string functionIdName = "HelloLambdaFunction";

        // This is for container deployment
        // var buildOption = new BundlingOptions()
        // {
        //     Image = Runtime.DOTNET_8.BundlingImage,
        //     User = "root",
        //     OutputType = BundlingOutput.ARCHIVED,
        //     // Command = new string[]{
        //     //     "/bin/sh",
        //     //     "-c",
        //     //     " dotnet tool install -g Amazon.Lambda.Tools"+
        //     //     " && dotnet build"+
        //     //     " && dotnet lambda package --output-package /asset-output/function.zip"
        //     // }
        // };
        
        var helloWorldLambdaFunction = new Function(eimaAwsStack, functionIdName, new FunctionProps
        {
            FunctionName = functionIdName,
            
            Runtime = Runtime.DOTNET_8,
            MemorySize = 256,
            //LogRetention = RetentionDays.ONE_DAY,
            //
            Handler = "HelloLambda::HelloLambda.Function::FunctionHandler",
            Code = Code.FromAsset("./src/HelloLambda/publish"),
            Environment = new System.Collections.Generic.Dictionary<string, string> {
                { "MY_VARIABLE", "some value" }
            },
            Description = "A sample testing lambda function in EimaAwsStack"
            
        });
        
        var functionUrl = new FunctionUrl(eimaAwsStack, $"{functionIdName}Url", new FunctionUrlProps
        {
            Function = helloWorldLambdaFunction,
            AuthType = FunctionUrlAuthType.NONE,
            Cors = new FunctionUrlCorsOptions()
            {
                AllowedOrigins = ["*"],
                AllowedHeaders = ["*"],
                AllowedMethods = [ HttpMethod.ALL ],
            }
            //Authorization = FunctionUrlAuthType.NONE, // Or IAM if you need authentication
            // Cors = new FunctionUrlCorsOptions // Optional: Configure CORS
            // {
            //     AllowedOrigins = new { "*" }, // Replace with your allowed origins
            //     AllowedMethods = new { HttpMethod.ALL }, // Or specify methods
            //     AllowedHeaders = new { "*" } // Or specify headers
            // }
        });
        
        
        new CfnOutput(eimaAwsStack, "LambdaFunctionArn", new CfnOutputProps {
            Value = helloWorldLambdaFunction.FunctionArn
        });
        
        new CfnOutput(eimaAwsStack, "FunctionUrlOutput", new CfnOutputProps
        {
            Value = functionUrl.Url,
            Description = "URL of the Lambda function"
        });
    }

    private static void SetupBasicLambda(EimaAwsStack eimaAwsStack, IRole lambdaIamRole,
    string projectName, string moduleName, string functionName
    , string handlerPath
    , string codePath
    , string description
    , Dictionary<string, string> environmentVariables = null
    , bool useFunctionUrl = false
    )
    {
        string functionId = $"{functionName}Function";

        var lambdaFunction = new Function(eimaAwsStack, functionId, new FunctionProps
        {
            Role = lambdaIamRole,
            FunctionName = $"{projectName}-{moduleName}-{functionName}",
            Runtime = Runtime.DOTNET_8,
            MemorySize = 256,
            Handler = handlerPath,
            Code = Code.FromAsset(codePath),
            Description = description,
            Environment = environmentVariables
        });

        new CfnOutput(eimaAwsStack, $"{functionId}Arn", new CfnOutputProps
        {
            Value = lambdaFunction.FunctionArn
        });

        if (useFunctionUrl)
        {
            string functionUrlId = $"{functionId}Url";
            var functionUrl = new FunctionUrl(eimaAwsStack, functionUrlId, new FunctionUrlProps
            {
                Function = lambdaFunction,
                AuthType = FunctionUrlAuthType.NONE,
                Cors = new FunctionUrlCorsOptions()
                {
                    AllowedOrigins = ["*"],
                    AllowedHeaders = ["*"],
                    AllowedMethods = [HttpMethod.ALL],
                }
            });

            new CfnOutput(eimaAwsStack, $"{functionUrlId}Output", new CfnOutputProps
            {
                Value = functionUrl.Url,
                Description = "URL of the Lambda function"
            });
        }
    }
}