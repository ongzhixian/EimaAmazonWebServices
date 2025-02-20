using Amazon.CDK;
using Amazon.CDK.AWS.IAM;
using Constructs;

namespace EimaAws;

public class EimaAwsStack : Stack
{
    internal EimaAwsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        Role projectAppIamRole = IamsRoles.Setup.ProjectAppRole(this);
        
        LambdaFunctions.Setup.HelloLambda(this);
        LambdaFunctions.Setup.ProjectLambdas(this, projectAppIamRole);
        LambdaFunctions.Setup.CounterLambdas(this, projectAppIamRole);

        S3Buckets.Setup.EimaTestBucket(this);
        
        DynamoDbTables.Setup.EimaTestTable(this);
        DynamoDbTables.Setup.EimaProjectTable(this, EimaAws.DynamoDbTables.Setup.TableNames.ProjectTableName);
        DynamoDbTables.Setup.EimaDynamoDbTable(this, EimaAws.DynamoDbTables.Setup.TableNames.CounterTableName);
        DynamoDbTables.Setup.EimaDynamoDbTable(this, EimaAws.DynamoDbTables.Setup.TableNames.JobTableName);

        ApiGateway.Setup.EimaApiGateway(this);
    }
}