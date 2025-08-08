using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HypermediaSystemsDemo.Models;

namespace HypermediaSystemsDemo.Controllers;

public class HomeController(
    ILogger<HomeController> logger
    ) : Controller
{
  private readonly ILogger<HomeController> _logger = logger;

  public IActionResult Index()
  {
    return Redirect("/Contacts");
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
