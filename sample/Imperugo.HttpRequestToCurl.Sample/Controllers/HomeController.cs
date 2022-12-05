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

    [HttpPost]
    public async Task<IActionResult> PostAsync()
    {
        var curl = await Request.ToCurlAsync();

        _logger.LogDebug(curl);
        return Ok();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
