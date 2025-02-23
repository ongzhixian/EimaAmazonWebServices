using Eima.DataServices.Models;

namespace Eima.DataServices;

public interface IDataService
{
    Task<DataOperationResult> PutAsync<T>(string containerName, T targetObject);

    Task<DataOperationResult<T>> GetAsync<T>(string containerName, string id) where T : class, new();

    //object? Get(
    //    string containerName // containerName = tableName | bucketName | containerName
    //    , string id);        // primary key id of item

        //void PutAsync(string containerName, string id, object? item);


}

public abstract class DataService : IDataService
{
    public abstract Task<DataOperationResult> PutAsync<T>(string containerName, T targetObject);

    public abstract Task<DataOperationResult<T>> GetAsync<T>(string containerName, string id) where T : class, new();
}
