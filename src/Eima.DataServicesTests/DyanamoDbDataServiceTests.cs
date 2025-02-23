using Amazon.DynamoDBv2.Model;

using Eima.DataServicesTests;

namespace Eima.DataServices.Tests;

[TestClass]
public class DyanamoDbDataServiceTests
{
    // CONSTRUCTOR TESTS

    [TestMethod()]
    public void DyanamoDbDataServiceTest()
    {
        var dataService = new DyanamoDbDataService(TestConfiguration.GetDynamoDb());

        Assert.IsNotNull(dataService);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void WhenNullAmazonDynamoDbClient_ThrowsArgumentNullException()
    {
        var dataService = new DyanamoDbDataService(null);

        Assert.IsNotNull(dataService);
    }

    // PUTASYNC TESTS

    [TestMethod()]
    public async Task GetPutAsync()
    {
        DyanamoDbDataService dataService = new DyanamoDbDataService(TestConfiguration.GetDynamoDb());

        var counter = new Counter("Home", "HDB", 1);

        var opResponse = await dataService.PutAsync<Counter>("counter", counter);

        Assert.IsFalse(opResponse.Failed);
    }

    // GETASYNC TESTS

    [TestMethod()]
    public async Task GetTestAsync()
    {

        DyanamoDbDataService dataService = new DyanamoDbDataService(TestConfiguration.GetDynamoDb());

        var opResponse = await dataService.GetAsync<Counter>("counter", "Home");

        Assert.IsFalse(opResponse.Failed);
    }

    [TestMethod()]
    public async Task WhenFetchNonExisting_ResponseFailedIsTrue()
    {
        DyanamoDbDataService dataService = new DyanamoDbDataService(TestConfiguration.GetDynamoDb());

        var opResponse = await dataService.GetAsync<Counter>("counter", "Home");

        Assert.IsTrue(opResponse.Failed);
    }


    //public void GetEquiv<T>(T userCredentials)
    //{
    //    Type t = typeof(T);

    //    var publicProps = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

    //    Dictionary<string, AttributeValue> result = new();

    //    //foreach (var prop in publicProps)
    //    //{
    //    //    result.Add(
    //    //        prop.Name, 
    //    //        AttributeValueConverter.ConvertToAttributeValue(prop.PropertyType, out var conversionIssue)
    //    //    );
    //    //}

    //}

    private AttributeValue ConvertToAttributeValue(Type propertyType)
    {
        throw new NotImplementedException();
        if (propertyType.IsValueType) ConvertValueTypeToAttributeValue(propertyType);

        //if (propertyType.IsEnum) { }
        //if (propertyType.IsValueType) { }
        //if (propertyType.IsGenericType) { }
    }

    private void ConvertValueTypeToAttributeValue(Type propertyType)
    {
        throw new NotImplementedException();
    }


}

public record UserCredentials(string Username, string Password);
public record Counter
{
    public Counter()
    {
    }

    public Counter(string Id, string Type, int Value)
    {
    }

    public string Id { get; set; }
    public string Type { get; set; }
    public int Value { get; set; }
}