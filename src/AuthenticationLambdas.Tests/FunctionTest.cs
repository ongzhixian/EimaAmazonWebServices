using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

namespace AuthenticationLambdas.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {

        // Invoke the lambda function and confirm the string was upper cased.
        var function = new AuthenticationFunctions();
        var context = new TestLambdaContext();
        var upperCase = function.AuthenticateCredentialsHandler("hello world", context);

        Assert.Equal("HELLO WORLD", upperCase);
    }
}
