using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;

namespace Eima.DataServicesTests;

class TestConfiguration
{
    static IConfiguration configuration;

    static TestConfiguration()
    {
        configuration = new ConfigurationBuilder()
            .AddUserSecrets("47df7034-c1c1-4b87-8373-89f5e42fc9ec")
            .Build();
    }


    public static IAmazonDynamoDB GetDynamoDb()
    {
        BasicAWSCredentials awsCredentials = new BasicAWSCredentials(
            configuration["default_aws_access_key_id"]
            , configuration["default_aws_secret_access_key"]);

        AmazonDynamoDBConfig awsConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.USEast1
        };

        return new AmazonDynamoDBClient(awsCredentials, awsConfig);
        
    }
}
