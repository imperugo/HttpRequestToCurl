// Copyright (c) Ugo Lattanzi.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

namespace Imperugo.HttpRequestToCurl.Extensions;

/// <summary>
/// Options for generating the curl command
/// </summary>
public class ToCurlOptions
{
    /// <summary>
    /// Gets the command sequence, ie. <c>curl.exe</c> for CMD.
    /// </summary>
    /// <value>
    /// The command sequence.
    /// </value>
    public string CommandSequence { get; init; } = "curl.exe";

    /// <summary>
    /// Gets the line break sequence, ie. <c> ^</c> for CMD.
    /// </summary>
    /// <value>
    /// The line break sequence.
    /// </value>
    public string LineBreakSequence { get; init; } = " ^";

    /// <summary>
    /// Gets the quote sequence, ie. <c>"</c> for CMD.
    /// </summary>
    /// <value>
    /// The quote sequence.
    /// </value>
    public string QuoteSequence { get; init; } = "\"";

    /// <summary>
    /// Gets the inner quote sequence, ie. <c>""</c> for CMD.
    /// </summary>
    /// <value>
    /// The inner quote sequence.
    /// </value>
    public string InnerQuoteSequence { get; init; } = "\"\"";

    /// <summary>
    /// Get options for CMD.
    /// </summary>
    public static readonly ToCurlOptions CmdExe = new();

    /// <summary>
    /// Get options for PowerShell.
    /// </summary>
    public static readonly ToCurlOptions PowerShell = new() { CommandSequence = "curl.exe", LineBreakSequence = "`", QuoteSequence = "\"", InnerQuoteSequence = "\"\"" };

    /// <summary>
    /// Get options for Bash.
    /// </summary>
    public static readonly ToCurlOptions Bash = new() { CommandSequence = "curl", LineBreakSequence = "\\", QuoteSequence = "\'", InnerQuoteSequence = "\\\'" };
}
