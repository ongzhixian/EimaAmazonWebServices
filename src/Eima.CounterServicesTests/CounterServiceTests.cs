using System.Net;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Eima.CounterServices.Tests;

[TestClass()]

public class CounterServiceTests
{
    Dictionary<string, string?> settings = new Dictionary<string, string?>()
    {
        ["SomeKey"] = "SomeValue"
    };

    const string testCounterName = "test-counter";

    private readonly IConfiguration configuration;

    public CounterServiceTests()
    {
        configuration = new ConfigurationBuilder()
            .AddUserSecrets("47df7034-c1c1-4b87-8373-89f5e42fc9ec", true)
            .AddInMemoryCollection(settings)
            .Build();
    }

    [TestMethod()]
    [Owner("Eima-CounterService")]
    [Priority(2)]
    [TestCategory("Integration2")]
    [Description("AAA")]
    public void CounterServiceTest()
    {
        CounterService counterService = new(configuration);

        //Cookie cookie = new Cookie();
        //cookie.Name = "myCookie";
        //cookie.Value = "myCookieValue";
        //cookie.Expires = DateTime.Now.AddHours(2);
        //cookie.Path = "/";
        //cookie.HttpOnly = true;
        //cookie.Secure = true;

        //var x = GetCompleteCookieString(cookie);

        Assert.IsNotNull(counterService);
    }


    [TestMethod()]
    [Priority(1)]
    [TestCategory("Integration")]
    public async Task CreateCounterTestAsync()
    {
        CounterService counterService = new(configuration);

        var response = await counterService.CreateCounterAsync(testCounterName);

        Assert.IsTrue(response);
    }

    [TestMethod()]
    public async Task RemoveCounterTestAsync()
    {
        CounterService counterService = new(configuration);

        var response = await counterService.RemoveCounterAsync(testCounterName);

        Assert.IsTrue(response);
    }


    [TestMethod()]
    public async Task GetCounterValueTestAsync()
    {
        CounterService counterService = new(configuration);

        var response = await counterService.GetCounterValueAsync(testCounterName);

        Assert.IsInstanceOfType<uint>(response);
    }


    [TestMethod()]
    public async Task AddToCounterTestAsync()
    {
        CounterService counterService = new(configuration);

        var response = await counterService.AddToCounterAsync(testCounterName);

        Assert.IsTrue(response);
    }
}