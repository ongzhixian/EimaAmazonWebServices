using Amazon.DynamoDBv2;
using NSubstitute;
using ProjectLambdas.Models;
using ProjectLambdas.Services;
using Xunit;

namespace ProjectLambdas.Tests.Services;

public class ProjectServiceTest
{

    [Fact]
    public void METHOD()
    {
        
    }

    [Fact]
    public void WhenPutProject_ThenReturnTrue()
    {
        IAmazonDynamoDB dynamoDbClient = Substitute.For<IAmazonDynamoDB>();
        ProjectService projectService = new ProjectService(dynamoDbClient);
        
        Project newProject = new Project();
        newProject.Name = "test-project";
        newProject.Version = "1.0.0";
        
        projectService.PutProject(newProject);
    }
}