namespace ProjectLambdas.Models;

public class OperationResult
{
    public bool Success { get; set; }
    
    public string Message { get; set; }

    public static OperationResult Failed(string failureReason)
    {
        return new OperationResult
        {
            Success = false,
            Message = failureReason
        };
    }
}