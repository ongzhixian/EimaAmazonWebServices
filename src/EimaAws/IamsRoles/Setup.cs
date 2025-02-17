using Amazon.CDK;
using Amazon.CDK.AWS.IAM;

namespace EimaAws.IamsRoles;

public class Setup
{
    public static void ProjectRoles(EimaAwsStack eimaAwsStack)
    {
        // var appRole = new Role(eimaAwsStack, "EimaProjectReadWriteRole", new RoleProps
        // {
        //     // AssumedBy = new AccountPrincipal(Account),
        //     // RoleName = $"{appName}-Role", // Example naming convention
        //     // ManagedPolicies = new [] { ManagedPolicy.FromAwsManagedPolicyName("AmazonS3FullAccess")}, // Grant role permissions to S3
        //     // Condition = Fn.Not(bucketExistsCondition) // Create role ONLY if bucket was created
        // });
    }
}