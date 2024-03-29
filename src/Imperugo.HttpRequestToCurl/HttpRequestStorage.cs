// Copyright (c) Ugo Lattanzi.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Imperugo.HttpRequestToCurl;

/// <summary>
/// The Http Request
/// </summary>
public record HttpRequestStorage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderStorage"/> class.
    /// </summary>
    /// <param name="method">The http request verb.</param>
    /// <param name="contentType">The http request content type.</param>
    /// <param name="url">The http request url.</param>
    /// <param name="headers">The http request headers.</param>
    /// <param name="protocol">The http protocol (i.e. http/2.0)</param>
    /// <param name="payload">The http request body.</param>
    public HttpRequestStorage(string method, string? contentType, string url, HeaderStorage[] headers, string protocol, string? payload)
    {
        Method = method;
        ContentType = contentType;
        Url = url;
        Headers = headers;
        Payload = payload;
        Protocol = protocol;
    }

    /// <summary>
    /// The HttpRequest Verb.
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// The Http Protocol.
    /// </summary>
    public string Protocol { get; set; }

    /// <summary>
    /// The Content Type of the HttpRequest
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// The HttpRequest url.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The HttpRequest Headers
    /// </summary>
    public HeaderStorage[] Headers { get; set; }

    /// <summary>
    /// The HttpRequest Payload
    /// </summary>
    public string? Payload { get; set; }
}

/// <summary>
/// The Http Header
/// </summary>
public record HeaderStorage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderStorage"/> class.
    /// </summary>
    /// <param name="key">The http request header key.</param>
    /// <param name="value">The http request header value.</param>
    public HeaderStorage(string key, string value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// The Http Header Key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// The Http Header Value.
    /// </summary>
    public string Value { get; set; }
}
