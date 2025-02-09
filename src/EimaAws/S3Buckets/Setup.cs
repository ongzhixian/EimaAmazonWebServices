using Amazon.CDK;
using Amazon.CDK.AWS.S3;

namespace EimaAws.S3Buckets;

public class Setup
{
    public static void EimaTestBucket(EimaAwsStack eimaAwsStack)
    {
        // var siteBucket = new Bucket(eimaAwsStack, "SiteBucket", new BucketProps
        // {
        //     BucketName = siteDomain,
        //     WebsiteIndexDocument = "index.html",
        //     WebsiteErrorDocument = "error.html",
        //     PublicReadAccess = true,
        //
        //     // The default removal policy is RETAIN, which means that cdk destroy will not attempt to delete
        //     // the new bucket, and it will remain in your account until manually deleted. By setting the policy to
        //     // DESTROY, cdk destroy will attempt to delete the bucket, but will error if the bucket is not empty.
        //     RemovalPolicy = RemovalPolicy.DESTROY // NOT recommended for production code
        // });
        
        var eimaTestBucket = new Bucket(eimaAwsStack, "EimaTestBucket", new BucketProps
        {
            // Bucket name (optional, CDK will generate one if not provided)
            BucketName = "eima-test",  // Ensure uniqueness globally
            
            // Block public access (generally recommended)
            BlockPublicAccess = BlockPublicAccess.BLOCK_ALL, // Or customize as needed
            
            //PublicReadAccess = true,
            
            // Removal policy (important for non-production environments)
            // RemovalPolicy = RemovalPolicy.DESTROY, // Be VERY careful with this in production!

            
            // // Versioning (good practice for data protection)
            // Versioning = new VersioningProps { Enabled = true },
            //
            
            //
            // // Encryption (server-side encryption is usually a good idea)
            // Encryption = BucketEncryption.SseAwsKms(), // Or SseCustomer for customer-managed keys
            
            // Lifecycle rules (for managing object lifecycle - optional)
            // LifecycleRules = new []
            // {
            //     new LifecycleRule
            //     {
            //         Id = "MyLifecycleRule",
            //         Prefix = "logs/", // Apply to objects with this prefix (optional)
            //         Expiration = Duration.Days(30), // Expire after 30 days
            //         Transitions = new []
            //         {
            //             new Transition
            //             {
            //                 StorageClass = StorageClass.GLACIER,
            //                 TransitionAfter = Duration.Days(7) // Move to Glacier after 7 days
            //             }
            //         }
            //     }
            // }
        });


        // new CfnOutput(eimaAwsStack, "Bucket", new CfnOutputProps
        // {
        //     Value = siteBucket.BucketName
        // });
        
        // Output the bucket name (useful for other resources to reference)
        new CfnOutput(eimaAwsStack, "BucketNameOutput", new CfnOutputProps
        {
            Value = eimaTestBucket.BucketName, // Or get the generated name: myBasicBucket.BucketName
            Description = "The name of the created S3 bucket"
        });
    }
}