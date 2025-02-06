using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;

namespace EimaAws;

public class EimaAwsStack : Stack
{
    internal EimaAwsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        var helloWorldLambdaFunction = new Function(this, "HelloWorldFunction", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            MemorySize = 1024,
            LogRetention = RetentionDays.ONE_DAY,
            //
            Handler = "HelloWorldLambda::HelloWorldLambda.Function::FunctionHandler",
            Code = Code.FromAsset("./src/HelloWorldLambda"),
            Environment = new System.Collections.Generic.Dictionary<string, string> {
                { "MY_VARIABLE", "some value" }
            }
        });
        
        new CfnOutput(this, "LambdaFunctionArn", new CfnOutputProps {
            Value = helloWorldLambdaFunction.FunctionArn
        });
    }
}