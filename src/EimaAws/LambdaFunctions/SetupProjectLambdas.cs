using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SES.Actions;

namespace EimaAws.LambdaFunctions;

public partial class Setup
{
    //const string HelloLambdaFunctionName = "HelloLambdaFunction";
    //const string RegisterNewProjectFunctionName = "register-project";
    //const string GetProjectListFunctionName = "GetProjectList";

    
    public static void ProjectLambdas(EimaAwsStack eimaAwsStack, IRole projectAppIamRole)
    {
        SetupRegisterNewProjectLambda(eimaAwsStack, projectAppIamRole);
        SetupGetProjectListLambda(eimaAwsStack, projectAppIamRole);
    }
    
    private static void SetupGetProjectListLambda(EimaAwsStack eimaAwsStack, IRole projectAppIamRole)
    {
        string functionId = $"{GetProjectListFunctionName}Function";
        string functionUrlId = $"{functionId}Url";

        var lambdaFunction = new Function(eimaAwsStack, functionId, new FunctionProps
        {
            FunctionName = GetProjectListFunctionName,
            
            Runtime = Runtime.DOTNET_8,
            MemorySize = 256,
            Handler = "ProjectLambdas::ProjectLambdas.ProjectFunctions::GetProjectList",
            Code = Code.FromAsset("./src/ProjectLambdas/publish"),
            Environment = new System.Collections.Generic.Dictionary<string, string> {
                { "MY_VARIABLE", "some value" }
            },
            Description = "Get project list",
            Role = projectAppIamRole
            
        });
        
        var functionUrl = new FunctionUrl(eimaAwsStack, functionUrlId, new FunctionUrlProps
        {
            Function = lambdaFunction,
            AuthType = FunctionUrlAuthType.NONE,
            Cors = new FunctionUrlCorsOptions()
            {
                AllowedOrigins = ["*"],
                AllowedHeaders = ["*"],
                AllowedMethods = [ HttpMethod.ALL ],
            }
        });
        
        new CfnOutput(eimaAwsStack, $"{functionId}Arn", new CfnOutputProps {
            Value = lambdaFunction.FunctionArn
        });
        
        new CfnOutput(eimaAwsStack, $"{functionUrlId}Output", new CfnOutputProps
        {
            Value = functionUrl.Url,
            Description = "URL of the Lambda function"
        });
    }

    private static void SetupRegisterNewProjectLambda(EimaAwsStack eimaAwsStack, IRole projectAppIamRole)
    {
        string projectName = "eima";
        string moduleName = "project";
        string functionId = $"{RegisterNewProjectFunctionName}Function";
        string functionUrlId = $"{functionId}Url";

        var lambdaFunction = new Function(eimaAwsStack, functionId, new FunctionProps
        {
            FunctionName = $"{projectName}-{moduleName}-{RegisterNewProjectFunctionName}",
            Runtime = Runtime.DOTNET_8,
            MemorySize = 256,
            Handler = "ProjectLambdas::ProjectLambdas.ProjectFunctions::RegisterNewProject",
            Code = Code.FromAsset("./src/ProjectLambdas/publish"),
            Environment = new System.Collections.Generic.Dictionary<string, string> {
                { "MY_VARIABLE", "some value" }
            },
            Description = "Register new project",
            Role = projectAppIamRole
        });
        
        var functionUrl = new FunctionUrl(eimaAwsStack, functionUrlId, new FunctionUrlProps
        {
            Function = lambdaFunction,
            AuthType = FunctionUrlAuthType.NONE,
            Cors = new FunctionUrlCorsOptions()
            {
                AllowedOrigins = ["*"],
                AllowedHeaders = ["*"],
                AllowedMethods = [ HttpMethod.ALL ],
            }
        });
        
        new CfnOutput(eimaAwsStack, $"{functionId}Arn", new CfnOutputProps {
            Value = lambdaFunction.FunctionArn
        });
        
        new CfnOutput(eimaAwsStack, $"{functionUrlId}Output", new CfnOutputProps
        {
            Value = functionUrl.Url,
            Description = "URL of the Lambda function"
        });
    }
}