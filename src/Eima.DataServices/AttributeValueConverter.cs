using Amazon.DynamoDBv2.Model;

namespace Eima.DataServices;

public class AttributeValueConverter
{
    private static readonly AttributeValue NullAttributeValue = new() { NULL = true };

    public static AttributeValue MapToAttributeValue(object? value) => value switch
    {
        // Integral Types
        null => NullAttributeValue,
        sbyte integralValue => new AttributeValue { N = integralValue.ToString() },
        byte integralValue => new AttributeValue { N = integralValue.ToString() },
        short integralValue => new AttributeValue { N = integralValue.ToString() },
        ushort integralValue => new AttributeValue { N = integralValue.ToString() },
        int integralValue => new AttributeValue { N = integralValue.ToString() },
        uint integralValue => new AttributeValue { N = integralValue.ToString() },
        long integralValue => new AttributeValue { N = integralValue.ToString() },
        ulong integralValue => new AttributeValue { N = integralValue.ToString() },

        // Floating-Point Types
        float integralValue => new AttributeValue { N = integralValue.ToString() },
        double integralValue => new AttributeValue { N = integralValue.ToString() },
        decimal integralValue => new AttributeValue { N = integralValue.ToString() },

        // Boolean Type:
        bool booleanValue => new AttributeValue { BOOL = booleanValue },

        // Character Type:
        char characterValue => new AttributeValue { S = characterValue.ToString() },

        // Reference Types
        string stringValue => new AttributeValue { S = stringValue },
        byte[] byteArray => new AttributeValue { B = new MemoryStream(byteArray) },
        List<object> list => MapToAttributeValueList(list),
        Dictionary<string, object> dictionary => MapToAttributeValueMap(dictionary),
        HashSet<string> stringSet => new AttributeValue { SS = [.. stringSet] },
        HashSet<int> numberSet => MapToNumberSet(numberSet),
        HashSet<byte[]> binarySet => MapToBinarySet(binarySet),

        // Default; 
        //_ => new AttributeValue { S = System.Text.Json.JsonSerializer.Serialize(value) }
        _ => throw new Exception("Unsupported type")
    };


    public static object? MapAttributeValueToDotNet(AttributeValue attributeValue)
    {
        if ((attributeValue == null) || (attributeValue.NULL)) return null;

        if (attributeValue.IsBOOLSet) return attributeValue.BOOL;

        //if (attributeValue.N is not null) return decimal.Parse(attributeValue.N);

        if (attributeValue.N is not null)
        {
            //return MapAttributeValueNumberToDotnet(attributeValue.N);
            return MapAttributeValueNumberStringToDotnet(attributeValue.N);
        }

        if (attributeValue.S is not null)
        {
            if (attributeValue.S.Length == 1) return char.Parse(attributeValue.S);

            return attributeValue.S;
        }

        //if (attributeValue.S != null)
        //{
        //    try
        //    {
        //        return System.Text.Json.JsonSerializer.Deserialize(attributeValue.S);
        //        //attempt to deserialize. If fails, return the string.
        //        //return JsonConvert.DeserializeObject(attributeValue.S);
        //    }
        //    catch (Exception e)
        //    {
        //        return attributeValue.S;
        //    }
        //}


        //byte[] byteArray => new AttributeValue { B = new MemoryStream(byteArray) },
        //List<object> list => MapToAttributeValueList(list),
        //Dictionary<string, object> dictionary => MapToAttributeValueMap(dictionary),
        //HashSet<string> stringSet => new AttributeValue { SS = [.. stringSet] },
        //HashSet<int> numberSet => MapToNumberSet(numberSet),
        //HashSet<byte[]> binarySet => MapToBinarySet(binarySet),

        if (attributeValue.B is not null) return attributeValue.B.ToArray();
        
        if (attributeValue.L is not null) return attributeValue.L.Select(MapAttributeValueToDotNet).ToList();
        if (attributeValue.M is not null) return attributeValue.M.ToDictionary(kvp => kvp.Key, kvp => MapAttributeValueToDotNet(kvp.Value));
        
        if (attributeValue.SS is not null) return attributeValue.SS.ToHashSet();
        
        if (attributeValue.NS != null)
        {
            return attributeValue.NS.Select(n => MapAttributeValueNumberStringToDotnet(n)).ToHashSet();
        }
        if (attributeValue.BS is not null) return attributeValue.BS.Select(av => av.ToArray()).ToHashSet();
        
        if (attributeValue.NULL) return null;

        //return null; // Default case if no matching type is found.
        throw new Exception("Unsupported");
    }

    private static object MapAttributeValueNumberStringToDotnet(string numberString)
    {
        if (numberString.Contains('.'))
        {
            if (decimal.TryParse(numberString, out decimal decimalValue)) return decimalValue;
            if (float.TryParse(numberString, out float floatValue)) return floatValue;
            if (double.TryParse(numberString, out double doubleValue)) return doubleValue;

            throw new Exception("Number out of range");
            //return attributeValue.N; //if all else fails, return the string.
        }
        else
        {
            //sbyte integralValue => new AttributeValue { N = integralValue.ToString() },
            //byte integralValue => new AttributeValue { N = integralValue.ToString() },

            if (sbyte.TryParse(numberString, out sbyte sbyteValue)) return sbyteValue;
            if (byte.TryParse(numberString, out byte byteValue)) return byteValue;
            if (short.TryParse(numberString, out short shortValue)) return shortValue;
            if (ushort.TryParse(numberString, out ushort ushortValue)) return ushortValue;
            if (int.TryParse(numberString, out int intValue)) return intValue;
            if (uint.TryParse(numberString, out uint uintValue)) return uintValue;
            if (long.TryParse(numberString, out long longValue)) return longValue;
            if (ulong.TryParse(numberString, out ulong ulongValue)) return ulongValue;

            throw new Exception("Number out of range");
            //return attributeValue.N; //if all else fails, return the string.
        }
    }

    //private static object MapAttributeValueNumberToDotnet(AttributeValue attributeValue)
    //{
    //    if (attributeValue.N.Contains('.'))
    //    {
    //        if (decimal.TryParse(attributeValue.N, out decimal decimalValue)) return decimalValue;
    //        if (float.TryParse(attributeValue.N, out float floatValue)) return floatValue;
    //        if (double.TryParse(attributeValue.N, out double doubleValue)) return doubleValue;

    //        throw new Exception("Number out of range");
    //        //return attributeValue.N; //if all else fails, return the string.
    //    }
    //    else
    //    {
    //        //sbyte integralValue => new AttributeValue { N = integralValue.ToString() },
    //        //byte integralValue => new AttributeValue { N = integralValue.ToString() },

    //        if (sbyte.TryParse(attributeValue.N, out sbyte sbyteValue)) return sbyteValue;
    //        if (byte.TryParse(attributeValue.N, out byte byteValue)) return byteValue;
    //        if (short.TryParse(attributeValue.N, out short shortValue)) return shortValue;
    //        if (ushort.TryParse(attributeValue.N, out ushort ushortValue)) return ushortValue;
    //        if (int.TryParse(attributeValue.N, out int intValue)) return intValue;
    //        if (uint.TryParse(attributeValue.N, out uint uintValue)) return uintValue;
    //        if (long.TryParse(attributeValue.N, out long longValue)) return longValue;
    //        if (ulong.TryParse(attributeValue.N, out ulong ulongValue)) return ulongValue;

    //        throw new Exception("Number out of range");
    //        //return attributeValue.N; //if all else fails, return the string.
    //    }
    //}


    //public static AttributeValue ConvertToAttributeValue(Type propertyType, object? propertyValue, out ConversionIssue? conversionIssue)
    //{
    //    conversionIssue = null;

    //    if (propertyValue is null) return NullAttributeValue;

    //    switch (propertyValue)
    //    {
    //        case string str:
    //            return new AttributeValue { S = str };
    //        case byte b:
    //            return new AttributeValue { N = b.ToString() };
    //        case sbyte sb:
    //            return new AttributeValue { N = sb.ToString() };
    //        case short s:
    //            return new AttributeValue { N = s.ToString() };
    //        case ushort us:
    //            return new AttributeValue { N = us.ToString() };
    //        case int i:
    //            return new AttributeValue { N = i.ToString() };
    //        case uint ui:
    //            return new AttributeValue { N = ui.ToString() };
    //        case long l:
    //            return new AttributeValue { N = l.ToString() };
    //        case ulong ul:
    //            return new AttributeValue { N = ul.ToString() };
    //        case float f:
    //            return new AttributeValue { N = f.ToString() };
    //        case double d:
    //            return new AttributeValue { N = d.ToString() };
    //        case decimal dec:
    //            return new AttributeValue { N = dec.ToString() };
    //        case bool bl:
    //            return new AttributeValue { BOOL = bl };
    //        case byte[] byteArray:
    //            return new AttributeValue { B = new AttributeValue { B = new MemoryStream(byteArray) }.B }; //Handles memory stream conversion
    //        case List<object> list:
    //            var avList = new List<AttributeValue>();
    //            foreach (var item in list)
    //            {
    //                avList.Add(ConvertToAttributeValue(item));
    //            }
    //            return new AttributeValue { L = avList };
    //        case Dictionary<string, object> dictionary:
    //            var avMap = new Dictionary<string, AttributeValue>();
    //            foreach (var kvp in dictionary)
    //            {
    //                avMap[kvp.Key] = ConvertToAttributeValue(kvp.Value);
    //            }
    //            return new AttributeValue { M = avMap };
    //        case HashSet<string> stringSet:
    //            return new AttributeValue { SS = new List<string>(stringSet) };
    //        case HashSet<int> numberSet:
    //            var numberSetStrings = new List<string>();
    //            foreach (var num in numberSet)
    //            {
    //                numberSetStrings.Add(num.ToString());
    //            }
    //            return new AttributeValue { NS = numberSetStrings };
    //        case HashSet<byte[]> binarySet:
    //            var binarySetMemoryStreams = new List<Amazon.DynamoDBv2.Model.AttributeValue>();
    //            foreach (var binary in binarySet)
    //            {
    //                binarySetMemoryStreams.Add(new Amazon.DynamoDBv2.Model.AttributeValue { B = new System.IO.MemoryStream(binary) });
    //            }

    //            List<Amazon.DynamoDBv2.Model.AttributeValue> finalBinarySet = new List<AttributeValue>();

    //            foreach (var attributeValue in binarySetMemoryStreams)
    //            {
    //                finalBinarySet.Add(new AttributeValue { B = attributeValue.B });
    //            }

    //            return new AttributeValue { BS = finalBinarySet };
    //        default:
    //            // Handle complex types or types you don't explicitly map.
    //            // You might serialize to JSON and store as a String.
    //            return new AttributeValue { S = Newtonsoft.Json.JsonConvert.SerializeObject(value) };
    //    }

    //    ConversionIssues conversionIssues = new ConversionIssues();

    //    if (propertyType.IsValueType) ConvertValueTypeToAttributeValue(propertyType, conversionIssues);

    //    if (propertyType.IsEnum) { }

    //    if (propertyType.IsValueType) { }

    //    if (propertyType.IsGenericType) { }

    //    //conversionIssue = new ConversionIssue("Unmapped", propertyType);
    //    //conversionIssues.Add("Unmapped", propertyType);

    //    if (propertyType == typeof(string)) return new AttributeValue { S = (string)propertyValue };

    //    if (propertyType == typeof(object)) return new AttributeValue { S = (string)propertyValue };


    //    return NullAttributeValue;
    //}



    private static AttributeValue MapToBinarySet(HashSet<byte[]> binarySet)
    {
        List<MemoryStream> binarySetMemoryStreams = new List<MemoryStream>();

        foreach (var binary in binarySet) binarySetMemoryStreams.Add(new MemoryStream(binary));

        return new AttributeValue { BS = binarySetMemoryStreams };
    }

    private static AttributeValue MapToNumberSet(HashSet<int> numberSet)
    {
        var numberSetStrings = new List<string>();

        foreach (var num in numberSet) numberSetStrings.Add(num.ToString());

        return new AttributeValue { NS = numberSetStrings };
    }

    private static AttributeValue MapToAttributeValueMap(Dictionary<string, object> dictionary)
    {
        var attributeValueMap = new Dictionary<string, AttributeValue>();

        foreach (var keyValuePair in attributeValueMap) attributeValueMap[keyValuePair.Key] = MapToAttributeValue(keyValuePair.Value);

        return new AttributeValue { M = attributeValueMap };
    }

    private static AttributeValue MapToAttributeValueList(List<object> list)
    {
        var attributeValueList = new List<AttributeValue>();

        foreach (var item in list) attributeValueList.Add(MapToAttributeValue(item));
        
        return new AttributeValue { L = attributeValueList };
    }

    //private static void ConvertValueTypeToAttributeValue(Type propertyType, ConversionIssues conversionIssues)
    //{
    //    //switch (propertyType.Name)
    //}

    //public static bool TryConvert(object? value, out AttributeValue attributeValue)
    //{
    //    if (MapToAttributeValue(value))
    //}

}

public class ConversionIssues
{
    public bool HasIssues => Issues.Count > 0;

    public IList<ConversionIssue> Issues = new List<ConversionIssue>();

    public void Add(string issueDescription, Type propertyType) =>
        Issues.Add(new ConversionIssue(issueDescription, propertyType));


}

public record ConversionIssue(string IssueDescription, Type PropertyType);
