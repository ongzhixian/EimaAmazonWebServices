using System.Collections.Generic;

using Amazon.CDK;
using Amazon.CDK.AWS.Apigatewayv2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AwsApigatewayv2Integrations;

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
        //var authenticateCredentialsHandler = LambdaFunctions.Setup.AuthenticateCredentialsHandler(this, projectAppIamRole);

        S3Buckets.Setup.EimaTestBucket(this);
        
        DynamoDbTables.Setup.EimaTestTable(this);
        DynamoDbTables.Setup.EimaProjectTable(this, EimaAws.DynamoDbTables.Setup.TableNames.ProjectTableName);
        DynamoDbTables.Setup.EimaDynamoDbTable(this, EimaAws.DynamoDbTables.Setup.TableNames.CounterTableName);
        DynamoDbTables.Setup.EimaDynamoDbTable(this, EimaAws.DynamoDbTables.Setup.TableNames.JobTableName);

        List<AddRoutesOptions> addRoutesOptionsList =
        [
            new AddRoutesOptions
            {
                Path = "/authentication",
                Methods = [HttpMethod.GET],
                Integration = new HttpLambdaIntegration("authenticateCredentialsHandler", LambdaFunctions.Setup.AuthenticateCredentialsHandler(this, projectAppIamRole))
            },
        ];

        //var placeholderLambda = new Function(this, "PlaceholderLambda", new FunctionProps
        //{
        //    Runtime = Runtime.NODEJS_16_X,
        //    Handler = "index.handler", // Even an empty handler is required
        //    Code = Code.FromInline("") // Or Code.FromAsset if you have a dummy handler
        //});

        ApiGateway.Setup.EimaApiGateway(this, addRoutesOptionsList);
    }
}