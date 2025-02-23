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

    public static Function AuthenticateCredentialsHandler(EimaAwsStack eimaAwsStack, IRole projectAppIamRole)
    {
        //SetupRegisterNewProjectLambda(eimaAwsStack, projectAppIamRole);
        //SetupGetProjectListLambda(eimaAwsStack, projectAppIamRole);
        var projectName = "AuthenticationLambdas";

        return SetupBasicLambda(eimaAwsStack, projectAppIamRole
            , projectName: "eima"
            , moduleName: "authentication"
            , functionName: "authenticate-credentials"
            , handlerPath: $"{projectName}::${projectName}.AuthenticationFunctions::AuthenticateCredentialsHandler"
            , codePath: $"./src/{projectName}/publish"
            , description: "Authenticate user credentials"
            , environmentVariables: new Dictionary<string, string> {
                { "MY_VARIABLE", "some value" }
            }
            , useFunctionUrl: false
            );
    }



}