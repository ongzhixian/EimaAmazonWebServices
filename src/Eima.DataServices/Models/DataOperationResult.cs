

namespace Eima.DataServices.Models;

// Default of Failed->false and message->null implies a success message
//public record DataOperationResult(bool Failed = false, string? Message = null)
//{
//    static readonly DataOperationResult OkDataOperationResult = new();

//    public static DataOperationResult Ok(T returnObject) => OkDataOperationResult;

//    internal static DataOperationResult<T> Ok<T>(T returnObject) where T : class, new()
//    {
//        return new DataOperationResult<T>()
//        {
//            Failed = false,
//            Message = string.Empty,
//            Data = returnObject
//        };
//    }
//}

//public record DataOperationResult<T>(bool Failed = false, string? Message = null) : DataOperationResult(Failed, Message) where T : class, new()
//{
//    public static DataOperationResult<T> Ok(T returnObject, bool Failed = false, string? Message = null)
//    {
//        return new DataOperationResult<T>()
//        {
//            Failed = Failed,
//            Message = Message,
//            Data = returnObject
//        };
//    }

//    public T? Data { get; set; }
//}




public record DataOperationResult(bool Failed = false, string? Message = null)
{
    static readonly DataOperationResult OkDataOperationResult = new();

    public static DataOperationResult Ok() => OkDataOperationResult;

    public static DataOperationResult<T> Ok<T>(T returnObject) where T : class
    {
        return new DataOperationResult<T>(false, null, returnObject);
    }

    public static DataOperationResult<T> Ok<T>(T returnObject, bool failed, string? message) where T : class
    {
        return new DataOperationResult<T>(failed, message, returnObject);
    }

    public static DataOperationResult<T> Fail<T>(string? message) where T : class
    {
        return new DataOperationResult<T>(true, message, null);
    }

    public static DataOperationResult Fail(string? message)
    {
        return new DataOperationResult(true, message);
    }
}

public record DataOperationResult<T>(bool Failed = false, string? Message = null, T? Data = default) : DataOperationResult(Failed, Message) where T : class
{
}