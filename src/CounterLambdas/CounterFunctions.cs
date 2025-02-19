using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CounterLambdas;

public class CounterFunctions
{
    
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public string CreateCounterHandler(string input, ILambdaContext context)
    {
        return input.ToUpper();
    }

    public string FunctionHandler1(string input, ILambdaContext context)
    {
        return input.ToUpper();
    }

    public string FunctionHandler2(string input, ILambdaContext context)
    {
        return input.ToUpper();
    }


}
