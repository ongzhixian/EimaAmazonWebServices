using Amazon.Lambda.APIGatewayEvents;

using System.Net;
using System.Net.Mime;
using System.Text;

namespace BaseHttpLambda;

public class HttpResponse
{
    public static APIGatewayProxyResponse Ok(string body = "", string contentType = MediaTypeNames.Text.Plain, Cookie? cookie = null) =>
        APIGatewayProxyResponseFactory(HttpStatusCode.OK, body, contentType, cookie);

    private static APIGatewayProxyResponse APIGatewayProxyResponseFactory(
        HttpStatusCode httpStatusCode = HttpStatusCode.OK
        , string body = ""
        , string contentType = MediaTypeNames.Text.Plain
        , Cookie? cookie = null)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)httpStatusCode,
            Headers = new Dictionary<string, string> { { "Content-Type", contentType } },
            Body = body
        };

        //response.Headers["Set-Cookie"] = "myCookie=myValue; Expires=Wed, 21 Oct 2024 07:28:00 GMT; Path=/; HttpOnly; SameSite=Strict; Secure";
        if (cookie is not null) response.Headers["Set-Cookie"] = GetCompleteCookieString(cookie);
        
        return response;
    }

    private static string GetCompleteCookieString(Cookie cookie)
    {
        StringBuilder sb = new StringBuilder(cookie.ToString());

        if (!string.IsNullOrEmpty(cookie.Domain)) sb.Append($"; Domain={cookie.Domain}");

        if (!string.IsNullOrEmpty(cookie.Path)) sb.Append($"; Path={cookie.Path}");

        if (cookie.Expires != DateTime.MinValue) sb.Append($"; Expires={cookie.Expires.ToUniversalTime().ToString("R")}");

        if (cookie.Secure) sb.Append("; Secure");

        if (cookie.HttpOnly) sb.Append("; HttpOnly");

        if (cookie.Version > 0) sb.Append($"; Version={cookie.Version}");

        //if (cookie.Attributes != null && cookie.Attributes.Count > 0)
        //{
        //    foreach (string key in cookie.Attributes.AllKeys)
        //    {
        //        sb.Append($"; {key}={cookie.Attributes[key]}");
        //    }
        //}

        return sb.ToString();
    }


}
