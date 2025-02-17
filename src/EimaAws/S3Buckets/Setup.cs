using System;
using Amazon.CDK;
using Amazon.CDK.AWS.S3;

namespace EimaAws.S3Buckets;

public class Setup
{
    public static void EimaTestBucket(EimaAwsStack eimaAwsStack)
    {
        if (!BucketExists(eimaAwsStack, "EimaTestBucket", "eima-test")) CreateEimaTestBucket(eimaAwsStack);
        
        // try
        // {
        //     Bucket existingBucket = Bucket.FromBucketName(this, "ExistingBucket", "eima-test");
        //
        //     // If we reach here, the bucket EXISTS.  You can now use existingBucket.
        //     Console.WriteLine($"Bucket '{bucketName}' already exists. Using existing bucket.");
        //
        //     // Example: Granting public read access (use with caution in production!)
        //     // existingBucket.GrantPublicRead();
        //
        // }
        // catch (System.Exception) // More specific exception handling is recommended
        // {
        //     // 2. If FromBucketName throws an exception, the bucket likely DOES NOT exist.
        //     Console.WriteLine($"Bucket '{bucketName}' does not exist. Creating a new bucket.");
        //
        //     // Create the bucket only if it doesn't already exist.
        //     var newBucket = new Bucket(this, "NewBucket", new BucketProps
        //     {
        //         BucketName = bucketName, // Important: Use the same name!
        //         // ... other bucket properties ...
        //         // RemovalPolicy = RemovalPolicy.DESTROY // Be very careful with this in production!
        //     });
        //
        //     // Optional: Output the bucket name for confirmation.
        //     new CfnOutput(this, "BucketNameOutput", new CfnOutputProps
        //     {
        //         Value = newBucket.BucketName
        //     });
        //
        // }
    }

    private static bool BucketExists(EimaAwsStack eimaAwsStack, string bucketId, string bucketName)
    {
        try
        {
            var existingBucket = Bucket.FromBucketName(eimaAwsStack, bucketId, bucketName);
            return true;
        }
        catch (System.Exception ex) // More specific exception handling is recommended
        {
            Console.WriteLine($"Bucket '{bucketName}' does not exist. Creating a new bucket." + ex.Message);
            return false;
        }
    }


    private static void CreateEimaTestBucket(EimaAwsStack eimaAwsStack)
    {
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
