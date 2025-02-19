using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Constructs;
using EimaAws.DynamoDbTables;

namespace EimaAws;

public class EimaAwsStack : Stack
{
    internal EimaAwsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        Role projectAppIamRole = IamsRoles.Setup.ProjectAppRole(this);
        
        LambdaFunctions.Setup.HelloLambda(this);
        LambdaFunctions.Setup.ProjectLambdas(this, projectAppIamRole);
        
        S3Buckets.Setup.EimaTestBucket(this);
        
        DynamoDbTables.Setup.EimaTestTable(this);
        DynamoDbTables.Setup.EimaProjectTable(this, Setup.TableNames.ProjectTableName);
        DynamoDbTables.Setup.EimaDynamoDbTable(this, Setup.TableNames.CounterTableName);
        DynamoDbTables.Setup.EimaDynamoDbTable(this, Setup.TableNames.JobTableName);
    }
}