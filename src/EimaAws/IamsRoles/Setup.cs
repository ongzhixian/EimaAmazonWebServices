using Amazon.CDK;
using Amazon.CDK.AWS.IAM;

namespace EimaAws.IamsRoles;

public class Setup
{
    public static Role ProjectAppRole(EimaAwsStack eimaAwsStack)
    {
        // var appRole = new Role(eimaAwsStack, "EimaProjectReadWriteRole", new RoleProps
        // {
        //     // AssumedBy = new AccountPrincipal(Account),
        //     // RoleName = $"{appName}-Role", // Example naming convention
        //     // ManagedPolicies = new [] { ManagedPolicy.FromAwsManagedPolicyName("AmazonS3FullAccess")}, // Grant role permissions to S3
        //     // Condition = Fn.Not(bucketExistsCondition) // Create role ONLY if bucket was created
        // });
        
        const string ProjectAppRoleId = "ProjectAppRole";
        
        var projectAppRole = new Role(eimaAwsStack, ProjectAppRoleId, new RoleProps
        {
            RoleName = ProjectAppRoleId,
            AssumedBy = new ServicePrincipal("lambda.amazonaws.com") // Important: Trust relationship for Lambda
        });
        
        projectAppRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole"));
        projectAppRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AmazonDynamoDBFullAccess"));
        projectAppRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AmazonS3FullAccess"));
        projectAppRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AmazonSQSFullAccess"));
        projectAppRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AmazonSNSFullAccess"));
        projectAppRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("AmazonEventBridgeFullAccess"));
        
        // projectAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        // {
        //     Actions = new[] { "dynamodb:PutItem", "dynamodb:GetItem" }, // Add other DynamoDB actions as needed
        //     Resources = new[]
        //     {
        //         "arn:aws:dynamodb:us-east-1:009167579319:table/project"
        //         , " arn:aws:dynamodb:us-east-1:009167579319:table/configuration"
        //         , "arn:aws:dynamodb:us-east-1:009167579319:table/appUser"
        //         
        //     } // Replace with your DynamoDB table ARN
        // }));
        
        // projectAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        // {
        //     Actions = new[] { "s3:GetObject" },
        //     Resources = new[] { "arn:aws:s3:::lab-bucket1/*" } // Replace with your S3 bucket ARN
        // }));

        // projectAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        // {
        //     Actions = new[] { "logs:CreateLogGroup", "logs:CreateLogStream", "logs:PutLogEvents" },
        //     Resources = new[] { "arn:aws:logs:*:*:*" } // Or restrict to a specific log group if needed
        // }));

        // Optional: Output the role ARN (useful for cross-stack references)
        new CfnOutput(eimaAwsStack, "projectAppRoleArn", new CfnOutputProps
        {
            Value = projectAppRole.RoleArn
        });

        return projectAppRole;


        // var projectAppRole = new Role(eimaAwsStack, ProjectAppRoleId, new RoleProps
        // {
        //     AssumedBy = new ServicePrincipal("ec2.amazonaws.com"),
        //     ManagedPolicies = new[]
        //     {
        //         ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMManagedInstanceCore"),
        //         ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSCodeDeployRole")
        //     }
        // });



    }
}