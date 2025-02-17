using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.TestUtilities;

namespace ProjectLambdas.Tests;

public class ProjectFunctionsTest
{
    [Fact]
    public void RegisterNewProject()
    {
        var function = new ProjectFunctions();
        var context = new TestLambdaContext();
        APIGatewayProxyRequest request = new APIGatewayProxyRequest();
        var upperCase = function.RegisterNewProject(request, context);

        //Assert.Equal("Placeholder for RegisterNewProject", upperCase.Body);
    }
    
    [Fact]
    public void GetProjectList()
    {
        var function = new ProjectFunctions();
        var context = new TestLambdaContext();
        APIGatewayProxyRequest request = new APIGatewayProxyRequest();
        var upperCase = function.GetProjectList(request, context);

        Assert.Equal("Placeholder for GetProjectList", upperCase.Body);
    }
}
