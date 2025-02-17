using Amazon.CDK;
using Constructs;
using EimaAws.DynamoDbTables;

namespace EimaAws;

public class EimaAwsStack : Stack
{
    internal EimaAwsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        IamsRoles.Setup.ProjectRoles(this);
        
        LambdaFunctions.Setup.HelloLambda(this);
        LambdaFunctions.Setup.ProjectLambdas(this);
        
        S3Buckets.Setup.EimaTestBucket(this);
        
        DynamoDbTables.Setup.EimaTestTable(this);
        DynamoDbTables.Setup.EimaProjectTable(this, Setup.TableNames.ProjectTableName);
    }
}