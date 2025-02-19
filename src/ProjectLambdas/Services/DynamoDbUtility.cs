using System.Reflection;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using ProjectLambdas.Models;

namespace ProjectLambdas.Services;

public class DynamoDbUtility
{
    public static Dictionary<string, AttributeValue> ConvertToDynamoDbItem<T>(T target, ILogger logger)
    {
        Dictionary<string, AttributeValue> item = new Dictionary<string, AttributeValue>();

        Type targetType = typeof(T);

        var publicProps = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in publicProps)
            item.Add(prop.Name, MappedAttributeValue(prop.PropertyType, prop.GetValue(target), logger));

        return item;
    }

    private static AttributeValue MappedAttributeValue(Type propPropertyType, object? value, ILogger logger)
    {
        if (value is null)
            return new AttributeValue { NULL = true };
            
        
        switch (propPropertyType.FullName)
        {
            case nameof(System.String): 
                logger.LogDebug("Map property type {PropertyTypeFullName} to S", propPropertyType.FullName);
                return new AttributeValue { S = string.Empty };
            case nameof(System.Boolean):
                logger.LogDebug("Map property type {PropertyTypeFullName} to BOOL", propPropertyType.FullName);
                return new AttributeValue { BOOL = (bool)value };
            default:
                logger.LogWarning("No attribute mapping for property type {PropertyTypeFullName}", propPropertyType.FullName);
                return new AttributeValue { NULL = true };
        }
    }
}