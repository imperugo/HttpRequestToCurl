using System.Text;

using Imperugo.HttpRequestToCurl;

using Microsoft.AspNetCore.Http.Extensions;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// The extensions method needed to convert an HTTP request to a cURL
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequestStorage"/>.
    /// </summary>
    /// <param name="storage">The Http Request storage.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <returns>The cURL.</returns>
    public static string ToCurl(this HttpRequestStorage storage, bool insecure = false,  bool includeDelimiters = false)
    {
        var sb = new StringBuilder();

        if (includeDelimiters)
            sb.AppendLine("--------------------- cURL REQUEST BEGIN ---------------------");

        sb.Append("curl --location ");

        if (insecure)
            sb.Append("--insecure ");

        sb.AppendLine($"--request {storage.Method} '{storage.Url}'");

        foreach (var header in storage.Headers)
            sb.AppendLine($"--header '{header.Key}: {string.Join(',', header.Value)}' ");

        if (storage.ContentType?.Length > 0)
            sb.AppendLine($"--header 'Content-Type: {storage.ContentType}' ");

        if (storage.Payload?.Length > 0)
            sb.AppendLine($"--data-raw '{storage.Payload}'");

        if (includeDelimiters)
            sb.AppendLine("--------------------- cURL REQUEST END ---------------------");

        return sb.ToString();
    }

#if NET7_0
    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="request">The Http Request.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The cURL.</returns>
    public static async Task<string> ToCurlAsync(this HttpRequest request, bool insecure = true, bool includeDelimiters = false, CancellationToken cancellationToken = default)
#else
    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="request">The Http Request.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <returns>The cURL.</returns>
    public static async Task<string> ToCurlAsync(this HttpRequest request, bool insecure = true, bool includeDelimiters = false)
#endif
    {
        var headers = new HeaderStorage[request.Headers.Count];
        var count = 0;

        foreach (var (key, value) in request.Headers)
        {
            headers[count] = new HeaderStorage(key, value.ToString());
            count++;
        }

        using var reader = new StreamReader(request.Body, encoding: Encoding.UTF8);

        request.EnableBuffering();

        var result = new HttpRequestStorage(
                request.Method,
                request.ContentType,
                request.GetDisplayUrl(),
                headers,
#if NET7_0
                await reader.ReadToEndAsync(cancellationToken))
#else
                await reader.ReadToEndAsync())
#endif
            .ToCurl(insecure: insecure, includeDelimiters: includeDelimiters);

        request.Body.Position = 0;

        return result;
    }
}
