using Amazon.Lambda.APIGatewayEvents;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

namespace HelloLambda.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new Function();
        var context = new TestLambdaContext();
        APIGatewayProxyRequest request = new APIGatewayProxyRequest();
        var upperCase = function.FunctionHandler(request, context);

        Assert.Equal("Received message: Input contains Amazon.Lambda.APIGatewayEvents.APIGatewayProxyRequest", upperCase.Body);
    }
}
