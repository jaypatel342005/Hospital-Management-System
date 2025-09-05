using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class EncryptedActionParameterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Get encrypted data and key from URL
        var request = context.HttpContext.Request;
        string encryptedData = request.Query["data"].FirstOrDefault();
        string key = request.Query["key"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(encryptedData) && !string.IsNullOrEmpty(key))
        {
            try
            {
                // Decrypt the data
                string decryptedData = HtmlHelperExtensions.SimpleDecrypt(encryptedData, key);
                
                if (!string.IsNullOrEmpty(decryptedData))
                {
                    // Parse parameters from decrypted string
                    var parameters = ParseParameters(decryptedData);
                    
                    // Add to action arguments
                    foreach (var param in parameters)
                    {
                        AddToActionArguments(context, param.Key, param.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Decryption failed: {ex.Message}");
            }
        }
        
        base.OnActionExecuting(context);
    }

    private Dictionary<string, object> ParseParameters(string queryString)
    {
        var parameters = new Dictionary<string, object>();
        
        if (string.IsNullOrEmpty(queryString)) return parameters;
        
        // Split by & to get individual parameters
        string[] pairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split('=', 2);
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim();
                string value = keyValue[1].Trim();
                
                // Convert to appropriate type
                object typedValue = ConvertValue(value);
                parameters[key] = typedValue;
            }
        }
        
        return parameters;
    }

    private object ConvertValue(string value)
    {
        // Try to convert to common types
        if (int.TryParse(value, out int intValue))
            return intValue;
            
        if (bool.TryParse(value, out bool boolValue))
            return boolValue;
            
        if (decimal.TryParse(value, out decimal decimalValue))
            return decimalValue;
            
        if (DateTime.TryParse(value, out DateTime dateValue))
            return dateValue;
            
        // Default to string
        return value;
    }

    private void AddToActionArguments(ActionExecutingContext context, string key, object value)
    {
        // Find matching parameter (case-insensitive)
        var matchingKey = context.ActionArguments.Keys
            .FirstOrDefault(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
            
        if (matchingKey != null)
        {
            context.ActionArguments[matchingKey] = value;
        }
        else
        {
            context.ActionArguments.Add(key, value);
        }
    }
}