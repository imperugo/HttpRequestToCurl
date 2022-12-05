# Imperugo.HttpRequestToCurl

[![Nuget](https://img.shields.io/nuget/v/Imperugo.HttpRequestToCurl?style=flat-square)](https://www.nuget.org/packages/Imperugo.HttpRequestToCurl/)
[![Nuget](https://img.shields.io/nuget/vpre/Imperugo.HttpRequestToCurl?style=flat-square)](https://www.nuget.org/packages/Imperugo.HttpRequestToCurl/)
[![GitHub](https://img.shields.io/github/license/imperugo/HttpRequestToCurl?style=flat-square)](https://github.com/imperugo/HttpRequestToCurl/blob/main/LICENSE)

The idea of this library is to have a curl from an HttpRequest instance.
The library produce a string that could be logged everywhere (console, db and so on).

## Quickstart

### Installation

Add the NuGet Package to your project:

```bash
dotnet add package Imperugo.HttpRequestToCurl
```

### Usage

The useage is absolutely easy. Just add the reference and, anywhere in your code write this:

```c#
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Imperugo.HttpRequestToCurl.Sample.Models;

namespace Imperugo.HttpRequestToCurl.Sample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var curl = await Request.ToCurlAsync();

        _logger.LogDebug(curl);
        return View();
    }
}

```

That code produces this output

```bash
dbug: Imperugo.HttpRequestToCurl.Sample.Controllers.HomeController[0]
      curl --location --request GET 'http://localhost:5069/'
      --header 'Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8' 
      --header 'Connection: keep-alive' 
      --header 'Host: localhost:5069' 
      --header 'User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36' 
      --header 'Accept-Encoding: gzip, deflate, br' 
      --header 'Accept-Language: en-GB,en' 
      --header 'Cache-Control: max-age=0' 
      --header 'Upgrade-Insecure-Requests: 1' 
      --header 'sec-ch-ua: "Brave";v="107", "Chromium";v="107", "Not=A?Brand";v="24"' 
      --header 'sec-ch-ua-mobile: ?0' 
      --header 'sec-ch-ua-platform: "macOS"' 
      --header 'Sec-GPC: 1' 
      --header 'Sec-Fetch-Site: none' 
      --header 'Sec-Fetch-Mode: navigate' 
      --header 'Sec-Fetch-User: ?1' 
      --header 'Sec-Fetch-Dest: document'
```

> If you want to use the `--insecure` you could do it with the specific parameter.
> ```csharp
> var curl = await Request.ToCurlAsync(insecure:true);
> ```

## Sample

Take a look [here](https://github.com/imperugo/HttpRequestToCurl/blob/main/sample/HttpRequestToCurl.Sample****)

## License

Imperugo.HttpRequestToCurl [MIT](https://github.com/imperugo/HttpRequestToCurl/blob/main/LICENSE) licensed.

### Contributing

Thanks to all the people who already contributed!

<a href="https://github.com/imperugo/HttpRequestToCurl/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=imperugo/HttpRequestToCurl" />
</a>
