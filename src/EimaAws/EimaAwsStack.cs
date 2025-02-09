using Amazon.CDK;
using Constructs;

namespace EimaAws;

public class EimaAwsStack : Stack
{
    internal EimaAwsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        LambdaFunctions.Setup.HelloLambda(this);
        
        S3Buckets.Setup.EimaTestBucket(this);
        
        DynamoDbTables.Setup.EimaTestTable(this);
    }
}