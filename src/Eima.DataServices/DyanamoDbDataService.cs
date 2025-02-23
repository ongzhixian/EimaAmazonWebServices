using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using Eima.DataServices.Models;

namespace Eima.DataServices;

public class DyanamoDbDataService : DataService
{
    private readonly IAmazonDynamoDB dynamoDb;

    public DyanamoDbDataService(IAmazonDynamoDB amazonDynamoDB)
    {
        dynamoDb = amazonDynamoDB ?? throw new ArgumentNullException();
    }

    public override async Task<DataOperationResult> PutAsync<T>(string containerName, T targetObject)
    {
        Type targetObjectType = typeof(T);

        var publicProps = targetObjectType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        var propertyAttributeValueMap = new Dictionary<string, AttributeValue>();

        foreach (var prop in publicProps)
            propertyAttributeValueMap.Add(prop.Name, AttributeValueConverter.MapToAttributeValue(prop.GetValue(targetObject)));

        var putItemResponse = await dynamoDb.PutItemAsync(containerName, propertyAttributeValueMap);

        return DataOperationResult.Ok();

        //return putItemResponse.HttpStatusCode == System.Net.HttpStatusCode.OK;

        // Terse version
        //dynamoDb.PutItemAsync(containerName, 
        //    typeof(T)
        //    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
        //    .ToDictionary(prop => prop.Name, prop => AttributeValueConverter.MapToAttributeValue(prop.GetValue(targetObject))));
    }

    public override async Task<DataOperationResult<T>> GetAsync<T>(string containerName, string id) where T : class
    {
        var getItemResponse = await dynamoDb.GetItemAsync(containerName, new Dictionary<string, AttributeValue>
        {
            {"Id", new AttributeValue {S = id} }
        });

        if (getItemResponse.HttpStatusCode != System.Net.HttpStatusCode.OK || getItemResponse.Item == null || getItemResponse.Item.Count == 0) return null;

        Type targetObjectType = typeof(T);

        var publicProps = targetObjectType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        var returnObject = new T();

        foreach (var prop in publicProps)
            if (getItemResponse.Item.TryGetValue(prop.Name, out AttributeValue attributeValue))
            {
                try
                {
                    prop.SetValue(returnObject, AttributeValueConverter.MapAttributeValueToDotNet(attributeValue));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting property {prop.Name}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Warning: Property {prop.Name} not found in DynamoDB item.");
            }

        return DataOperationResult.Ok<T>(returnObject);
    }

    //public override async Task PutAsync(string containerName, object? item)
    //{
    //    throw new NotImplementedException();

    //    if (item is null) return;

    //    await Task.CompletedTask;


    //    //var request = new PutItemRequest
    //    //{
    //    //    TableName = containerName,

    //    //    Item = new Dictionary<string, AttributeValue>()
    //    //    {
    //    //        { "Id", new AttributeValue { S = key }}
    //    //        , { "EffectiveDateTime", new AttributeValue { S = (effectiveDateTime.HasValue ? effectiveDateTime.Value.ToString("o") : DateTime.UtcNow.ToString("o")) }}
    //    //        , { "ExpiryDateTime",
    //    //            expiryDateTime.HasValue
    //    //            ? new AttributeValue { S =  expiryDateTime.Value.ToString("o") }
    //    //            : new AttributeValue { NULL = true}
    //    //        }
    //    //        , { "DataType", new AttributeValue { S = "string" }} // Hard code to string (because we only do string values) 
    //    //        , { "DataValue", new AttributeValue { S = value }}

    //    //        // (expiryDateTime.HasValue ? new AttributeValue { S =  effectiveDateTime.Value.ToString("o") } : new AttributeValue()) 
    //    //        // new AttributeValue { S = (effectiveDateTime.HasValue ? effectiveDateTime.Value.ToString("o") : DateTime.UtcNow.ToString("o")) }}
    //    //        // { "DataType", new AttributeValue { S = "Book 201 Title" }},
    //    //        // { "ISBN", new AttributeValue { S = "11-11-11-11" }},
    //    //        // { "Price", new AttributeValue { S = "20.00" }},
    //    //        // {
    //    //        //     "Authors",
    //    //        //     new AttributeValue
    //    //        //         { SS = new List<string>{"Author1", "Author2"}   }
    //    //        // }
    //    //    }
    //    //};

    //    //try
    //    //{
    //    //    var putItemResponse = await dynamoDb.PutItemAsync(request);
    //    //}
    //    //catch (Exception e)
    //    //{
    //    //    Console.WriteLine(e);
    //    //    throw;
    //    //}


    //    //return true;
    //}








    //public void PutAsync(DataServices.Tests.UserCredentials userCredentials)
    //{
    //    throw new NotImplementedException();
    //}

    //public override object? Put(string containerName, string id, object? object)
    //{
    //    throw new NotImplementedException();
    //}

}