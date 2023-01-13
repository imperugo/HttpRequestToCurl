// Copyright (c) Ugo Lattanzi.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System.Text;

using Imperugo.HttpRequestToCurl;
using Imperugo.HttpRequestToCurl.Extensions;

using Microsoft.AspNetCore.Http.Extensions;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// The extensions method needed to convert an HTTP request to a cURL
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequestStorage" />.
    /// </summary>
    /// <param name="storage">The Http Request storage.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <returns>
    /// The cURL.
    /// </returns>
    public static string ToCurl(this HttpRequestStorage storage, bool insecure = false, bool includeDelimiters = false) => ToCurl(storage, ToCurlOptions.Bash, insecure, includeDelimiters);

    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequestStorage" />.
    /// </summary>
    /// <param name="storage">The Http Request storage.</param>
    /// <param name="options">The command generation options.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <returns>
    /// The cURL.
    /// </returns>
    public static string ToCurl(this HttpRequestStorage storage, ToCurlOptions options, bool insecure = false, bool includeDelimiters = false)
    {
        var commandLines = new List<string>();

        if (insecure)
            commandLines.Add($"{options.CommandSequence} --location --insecure --request {storage.Method} {options.QuoteSequence}{storage.Url}{options.QuoteSequence}");
        else
            commandLines.Add($"{options.CommandSequence} --location --request {storage.Method} {options.QuoteSequence}{storage.Url}{options.QuoteSequence}");

        foreach (var header in storage.Headers)
            commandLines.Add($"--header {options.QuoteSequence}{header.Key}: {string.Join(',', header.Value.Replace(options.QuoteSequence, options.InnerQuoteSequence))}{options.QuoteSequence}");

        if (storage.ContentType?.Length > 0)
            commandLines.Add($"--header {options.QuoteSequence}Content-Type: {storage.ContentType.Replace(options.QuoteSequence, options.InnerQuoteSequence)}{options.QuoteSequence}");

        if (storage.Payload?.Length > 0)
            commandLines.Add($"--data-raw {options.QuoteSequence}{storage.Payload.Replace(options.QuoteSequence, options.InnerQuoteSequence)}{options.QuoteSequence}");

        var command = string.Join(options.LineBreakSequence + Environment.NewLine, commandLines.ToArray());

        if (includeDelimiters)
            command = string.Join(Environment.NewLine, "--------------------- cURL REQUEST BEGIN ---------------------", command, "--------------------- cURL REQUEST END ---------------------");

        return command;
    }

#if NET7_0
    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequest" />.
    /// </summary>
    /// <param name="request">The Http Request.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// The cURL.
    /// </returns>
    public static Task<string> ToCurlAsync(this HttpRequest request, bool insecure = false, bool includeDelimiters = false, CancellationToken cancellationToken = default) => ToCurlAsync(request, ToCurlOptions.Bash, insecure, includeDelimiters, cancellationToken);

    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="request">The Http Request.</param>
    /// <param name="options">The command generation options.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The cURL.</returns>
    public static async Task<string> ToCurlAsync(this HttpRequest request, ToCurlOptions options, bool insecure = false, bool includeDelimiters = false, CancellationToken cancellationToken = default)
#else
    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequest" />.
    /// </summary>
    /// <param name="request">The Http Request.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <returns>
    /// The cURL.
    /// </returns>
    public static Task<string> ToCurlAsync(this HttpRequest request, bool insecure = false, bool includeDelimiters = false) => ToCurlAsync(request, ToCurlOptions.Bash, insecure, includeDelimiters);

    /// <summary>
    /// Generate the cURL from ad instance of <see cref="HttpRequest" />.
    /// </summary>
    /// <param name="request">The Http Request.</param>
    /// <param name="options">The command generation options.</param>
    /// <param name="insecure">True if you want to user insecure flag, otherwise False.</param>
    /// <param name="includeDelimiters">The Http Request storage.</param>
    /// <returns>
    /// The cURL.
    /// </returns>
    public static async Task<string> ToCurlAsync(this HttpRequest request, ToCurlOptions options, bool insecure = false, bool includeDelimiters = false)
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
            .ToCurl(options: options, insecure: insecure, includeDelimiters: includeDelimiters);

        request.Body.Position = 0;

        return result;
    }
}
