namespace DistanceCalculatorAPI.Extensions;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Extension class for <see cref="HttpContext"/>
/// </summary>
public static class IpAddressExtensions
{
    /// <summary>
    /// Identify Ip address
    /// </summary>
    /// <param name="httpContext"></param>
    public static string? GetIpAddress(this HttpContext httpContext)
    {
        if (!string.IsNullOrEmpty(httpContext.Request.Headers["X-Forwarded-For"]))
        {
            return httpContext.Request.Headers["X-Forwarded-For"];
        }
        
        return httpContext.Connection.RemoteIpAddress?.ToString()!;
    }
}