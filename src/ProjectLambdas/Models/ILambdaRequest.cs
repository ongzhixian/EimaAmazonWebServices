namespace ProjectLambdas.Models;

public interface ILambdaRequest
{
    IDictionary<string, string> Headers { get; set; }
    
    IDictionary<string, string> QueryStringParameters { get; set; }
    
    string Body { get; set; }
}