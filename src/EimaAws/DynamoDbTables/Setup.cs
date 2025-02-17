using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.S3;

namespace EimaAws.DynamoDbTables;

public class Setup
{
    public class TableNames
    {
        public const string ConfigurationTableName = "configuration";
        public const string AppUserTableName = "appUser";
        public const string ProjectTableName = "project";
    }
    
    public static void EimaTestTable(EimaAwsStack eimaAwsStack)
    {
        // Method 1: Simple Table with Hash Key (Recommended for most cases)
        var configurationTable = new Table(eimaAwsStack, "configuration", new TableProps
        {
            TableName = TableNames.ConfigurationTableName, // Optional: Provide a name, or CDK will generate one
            PartitionKey = new Attribute { Name = "Id", Type = AttributeType.STRING }, // Hash key (required)
            
            //BillingMode = BillingMode.PAY_PER_REQUEST, // On-demand billing (recommended for variable workloads)
            // Use PROVISIONED to use ALWAYS-FREE tier
            BillingMode = BillingMode.PROVISIONED, // Optional: Use PROVISIONED for predictable costs
            ReadCapacity = 1, // Required if BillingMode is PROVISIONED
            WriteCapacity = 1, // Required if BillingMode is PROVISIONED
            RemovalPolicy = RemovalPolicy.DESTROY // CAUTION: Be very careful with this in production!
        });
        
        var appUserTable = new Table(eimaAwsStack, "appUser", new TableProps
        {
            TableName = TableNames.AppUserTableName, // Optional: Provide a name, or CDK will generate one
            PartitionKey = new Attribute { Name = "Id", Type = AttributeType.STRING }, // Hash key (required)
            
            //BillingMode = BillingMode.PAY_PER_REQUEST, // On-demand billing (recommended for variable workloads)
            // Use PROVISIONED to use ALWAYS-FREE tier
            BillingMode = BillingMode.PROVISIONED, // Optional: Use PROVISIONED for predictable costs
            ReadCapacity = 1, // Required if BillingMode is PROVISIONED
            WriteCapacity = 1, // Required if BillingMode is PROVISIONED
            RemovalPolicy = RemovalPolicy.DESTROY // CAUTION: Be very careful with this in production!
        });

        // // Local Secondary Index (LSI)
        // myTable.AddLocalSecondaryIndex(new LocalSecondaryIndexProps
        // {
        //     IndexName = "OrderByStatus",
        //     SortKey = new Attribute { Name = "orderStatus", Type = AttributeType.STRING }
        // });
        //
        // // Global Secondary Index (GSI)
        // myTable.AddGlobalSecondaryIndex(new GlobalSecondaryIndexProps
        // {
        //     IndexName = "CustomerOrdersByDate",
        //     PartitionKey = new Attribute { Name = "customerId", Type = AttributeType.STRING },
        //     SortKey = new Attribute { Name = "orderDate", Type = AttributeType.STRING },
        //     // Projection:  ALL (default), KEYS_ONLY, INCLUDE
        //     // NonKeyAttributes:  For INCLUDE
        //     // ReadCapacity: For PROVISIONED mode
        //     // WriteCapacity: For PROVISIONED mode
        // });
        
        // Output the table name (useful for other resources to reference)
        new CfnOutput(eimaAwsStack, "TableNameOutput", new CfnOutputProps
        {
            Value = configurationTable.TableName, // Or mySimpleTable.TableName
            Description = "The name of the created DynamoDB table"
        });
    }
    
    public static void EimaProjectTable(EimaAwsStack eimaAwsStack, string tableName)
    {
        string tableId = $"{tableName}DynamoDbTable";
        
        var dynamoDbTable = new Table(eimaAwsStack, tableId, new TableProps
        {
            TableName = tableName, // Optional: Provide a name, or CDK will generate one
            PartitionKey = new Attribute { Name = "Id", Type = AttributeType.STRING }, // Hash key (required)
            
            //BillingMode = BillingMode.PAY_PER_REQUEST, // On-demand billing (recommended for variable workloads)
            // Use PROVISIONED to use ALWAYS-FREE tier
            BillingMode = BillingMode.PROVISIONED, // Optional: Use PROVISIONED for predictable costs
            ReadCapacity = 1, // Required if BillingMode is PROVISIONED
            WriteCapacity = 1, // Required if BillingMode is PROVISIONED
            RemovalPolicy = RemovalPolicy.RETAIN // CAUTION: Be very careful with this in production!
        });

        // // Local Secondary Index (LSI)
        // myTable.AddLocalSecondaryIndex(new LocalSecondaryIndexProps
        // {
        //     IndexName = "OrderByStatus",
        //     SortKey = new Attribute { Name = "orderStatus", Type = AttributeType.STRING }
        // });
        //
        // // Global Secondary Index (GSI)
        // myTable.AddGlobalSecondaryIndex(new GlobalSecondaryIndexProps
        // {
        //     IndexName = "CustomerOrdersByDate",
        //     PartitionKey = new Attribute { Name = "customerId", Type = AttributeType.STRING },
        //     SortKey = new Attribute { Name = "orderDate", Type = AttributeType.STRING },
        //     // Projection:  ALL (default), KEYS_ONLY, INCLUDE
        //     // NonKeyAttributes:  For INCLUDE
        //     // ReadCapacity: For PROVISIONED mode
        //     // WriteCapacity: For PROVISIONED mode
        // });
        
        // Output the table name (useful for other resources to reference)
        new CfnOutput(eimaAwsStack, $"{tableName}TableNameOutput", new CfnOutputProps
        {
            Value = tableName, // Or mySimpleTable.TableName
            Description = "The name of the created DynamoDB table"
        });
    }
}