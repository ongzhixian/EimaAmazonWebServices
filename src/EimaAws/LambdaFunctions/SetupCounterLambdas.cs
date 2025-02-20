using System.Collections.Generic;

using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SES.Actions;

namespace EimaAws.LambdaFunctions;

public partial class Setup
{
    //const string HelloLambdaFunctionName2 = "HelloLambdaFunction";
    //const string RegisterNewProjectFunctionName2 = "register-project";
    //const string GetProjectListFunctionName2 = "GetProjectList";

    public static void CounterLambdas(EimaAwsStack eimaAwsStack, IRole projectAppIamRole)
    {
        //SetupRegisterNewProjectLambda(eimaAwsStack, projectAppIamRole);
        //SetupGetProjectListLambda(eimaAwsStack, projectAppIamRole);
        SetupBasicLambda(eimaAwsStack, projectAppIamRole
            , projectName: "eima"
            , moduleName: "counter"
            , functionName: "create-counter"
            , handlerPath: "CounterLambdas::CounterLambdas.CounterFunctions::CreateCounterHandler"
            , codePath: "./src/CounterLambdas/publish"
            , description: "Create a new counter"
            , environmentVariables: new Dictionary<string, string> {
                { "MY_VARIABLE", "some value" }
            }
            , useFunctionUrl: false
            );
    }


    //private static void SetupSomeCounterLambda(EimaAwsStack eimaAwsStack, IRole lambdaIamRole,
    //    string projectName, string moduleName, string functionName
    //    , string handlerPath
    //    , string codePath
    //    , string description
    //    , Dictionary<string, string> environmentVariables = null
    //    , bool useFunctionUrl = false
    //    )
    //{
    //    string functionId = $"{functionName}Function";

    //    var lambdaFunction = new Function(eimaAwsStack, functionId, new FunctionProps
    //    {
    //        Role = lambdaIamRole,
    //        FunctionName = $"{projectName}-{moduleName}-{functionName}",
    //        Runtime = Runtime.DOTNET_8,
    //        MemorySize = 256,
    //        Handler = handlerPath,
    //        Code = Code.FromAsset(codePath),
    //        Description = description,
    //        Environment = environmentVariables
    //    });

    //    new CfnOutput(eimaAwsStack, $"{functionId}Arn", new CfnOutputProps
    //    {
    //        Value = lambdaFunction.FunctionArn
    //    });

    //    if (useFunctionUrl)
    //    {
    //        string functionUrlId = $"{functionId}Url";
    //        var functionUrl = new FunctionUrl(eimaAwsStack, functionUrlId, new FunctionUrlProps
    //        {
    //            Function = lambdaFunction,
    //            AuthType = FunctionUrlAuthType.NONE,
    //            Cors = new FunctionUrlCorsOptions()
    //            {
    //                AllowedOrigins = ["*"],
    //                AllowedHeaders = ["*"],
    //                AllowedMethods = [HttpMethod.ALL],
    //            }
    //        });

    //        new CfnOutput(eimaAwsStack, $"{functionUrlId}Output", new CfnOutputProps
    //        {
    //            Value = functionUrl.Url,
    //            Description = "URL of the Lambda function"
    //        });
    //    }





    //}
}