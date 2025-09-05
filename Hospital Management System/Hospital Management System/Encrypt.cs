using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class HtmlHelperExtensions
{
    public static IHtmlContent EncodedActionLink<T>(
        this IHtmlHelper<T> htmlHelper,
        string linkText,
        string actionName,
        string controllerName,
        object routeValues,
        object htmlAttributes,
        string iconclass)
    {
        return BuildEncodedLink(linkText, actionName, controllerName, routeValues, htmlAttributes, iconclass);
    }

    public static IHtmlContent EncodedActionLink(
        this IHtmlHelper htmlHelper,
        string linkText,
        string actionName,
        string controllerName,
        object routeValues,
        object htmlAttributes,
        string iconclass)
    {
        return BuildEncodedLink(linkText, actionName, controllerName, routeValues, htmlAttributes, iconclass);
    }

    private static IHtmlContent BuildEncodedLink(string linkText, string actionName, string controllerName,
        object routeValues, object htmlAttributes, string iconclass)
    {
        // Build query string from route values
        string queryString = BuildQueryString(routeValues);

        // Build HTML attributes
        string htmlAttributesString = BuildHtmlAttributes(htmlAttributes);

        // Generate unique key for this request
        string uniqueKey = GenerateUniqueKey();

        // Encrypt the query string
        string encryptedData = string.Empty;
        if (!string.IsNullOrEmpty(queryString))
        {
            encryptedData = SimpleEncrypt(queryString, uniqueKey);
        }

        // Build the complete URL
        StringBuilder url = new StringBuilder();
        url.Append("/");
        if (!string.IsNullOrEmpty(controllerName))
            url.Append(controllerName);
        if (actionName != "Index")
            url.Append("/" + actionName);
        if (!string.IsNullOrEmpty(encryptedData))
            url.Append("?data=" + encryptedData + "&key=" + uniqueKey);

        // Build the anchor tag
        StringBuilder anchor = new StringBuilder();
        anchor.Append($"<a{htmlAttributesString} href='{url}'>");
        if (!string.IsNullOrEmpty(iconclass))
            anchor.Append($"<i class='{iconclass}'></i> ");
        anchor.Append(linkText);
        anchor.Append("</a>");

        return new HtmlString(anchor.ToString());
    }

    private static string BuildQueryString(object routeValues)
    {
        if (routeValues == null) return string.Empty;

        var queryParts = new List<string>();
        var routeDict = new RouteValueDictionary(routeValues);

        foreach (var kvp in routeDict)
        {
            queryParts.Add($"{kvp.Key}={kvp.Value}");
        }

        return queryParts.Any() ? string.Join("&", queryParts) : string.Empty;
    }

    private static string BuildHtmlAttributes(object htmlAttributes)
    {
        if (htmlAttributes == null) return string.Empty;

        var attributeParts = new List<string>();
        var attributeDict = new RouteValueDictionary(htmlAttributes);

        foreach (var kvp in attributeDict)
        {
            attributeParts.Add($"{kvp.Key}='{kvp.Value}'");
        }

        return attributeParts.Any() ? " " + string.Join(" ", attributeParts) : string.Empty;
    }

    private static string GenerateUniqueKey()
    {
        // Simple unique key: current ticks + random number
        long ticks = DateTime.UtcNow.Ticks;
        int random = new Random().Next(1000, 9999);
        return $"{ticks}{random}";
    }

    private static string SimpleEncrypt(string text, string key)
    {
        // Simple XOR encryption - fast and easy
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        for (int i = 0; i < textBytes.Length; i++)
        {
            textBytes[i] = (byte)(textBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return Convert.ToBase64String(textBytes);
    }

    public static string SimpleDecrypt(string encryptedText, string key)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(encryptedBytes[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return Encoding.UTF8.GetString(encryptedBytes);
        }
        catch
        {
            return string.Empty;
        }
    }
}