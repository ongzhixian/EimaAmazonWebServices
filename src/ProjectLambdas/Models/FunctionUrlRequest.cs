namespace ProjectLambdas.Models;

// I guess AWS Lambda expects you to manage Lambdas through its API Gateway service
// So when you call the lambda function via function url, it drops other information
// (which is kind of reasonable since each function url maps exactly to one lambda function; no sharing)
// No sharing, No one-url to multiple endpoints scenario

public class FunctionUrlRequest : ILambdaRequest 
{
    public IDictionary<string, string> Headers { get; set; }
    
    public IDictionary<string, string> QueryStringParameters { get; set; }
    
    public string Body { get; set; }
    
    public FunctionUrlRequest(IDictionary<string, string> requestHeaders, IDictionary<string, string> requestQueryStringParameters, string requestBody)
    {
        Headers = requestHeaders;
        QueryStringParameters = requestQueryStringParameters;
        Body = requestBody;
    }
}