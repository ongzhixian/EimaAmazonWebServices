using Amazon.DynamoDBv2.Model;

namespace Eima.DataServices.Tests;

public record TestRec(string Username, string Password);

[TestClass()]
public class AttributeValueConverterTests
{
    [TestMethod()]
    public void ConvertToAttributeValueTest()
    {
        TestRec sampleTestRec = new TestRec("SomeUsername", "SomePassword");

        Type t = typeof(TestRec);

        var publicProps = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        Dictionary<string, AttributeValue> propertyAttributeValueMap = new();

        //foreach (var prop in publicProps)
        //{
        //    result.Add(
        //        prop.Name,
        //        AttributeValueConverter.ConvertToAttributeValue(prop.PropertyType)
        //    );
        //}

        var prop = publicProps.Skip(0).First();

        //var converted = AttributeValueConverter.ConvertToAttributeValue(
        //    prop.PropertyType
        //    , prop.GetValue(sampleTestRec)
        //    , out ConversionIssue? conversionIssue);

        List<ConversionIssue> issues = new List<ConversionIssue>();
        var converted = AttributeValueConverter.MapToAttributeValue(null);

        bool hasIssue = true;
        //int.TryParse("", System.Globalization.NumberStyles.Currency, out int)

        //if (AttributeValueConverter.TryConvert(null, out AttributeValue attributeValue))
        //{ 
        //    if (conversionIssue != null) propertyAttributeValueMap.Add("PropName", attributeValue);
        //    return attributeValue;
        //}



        System.Diagnostics.Debugger.Break();

    }
}