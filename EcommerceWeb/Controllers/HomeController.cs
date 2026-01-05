using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EcommerceWeb.Models;

namespace EcommerceWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var featuredProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, CategoryId = 1 },
            new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone", Price = 699.99m, CategoryId = 1 }
        };
        return View(featuredProducts);
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
