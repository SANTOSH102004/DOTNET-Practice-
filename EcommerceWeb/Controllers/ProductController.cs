using Microsoft.AspNetCore.Mvc;
using EcommerceWeb.Models;

namespace EcommerceWeb.Controllers;

public class ProductController : Controller
{
    private static List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, CategoryId = 1 },
        new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone", Price = 699.99m, CategoryId = 1 },
        new Product { Id = 3, Name = "Book", Description = "Bestselling novel", Price = 19.99m, CategoryId = 2 }
    };

    private static List<Category> _categories = new List<Category>
    {
        new Category { Id = 1, Name = "Electronics" },
        new Category { Id = 2, Name = "Books" }
    };

    public IActionResult Index()
    {
        var viewModel = new ProductIndexViewModel
        {
            Products = _products,
            Categories = _categories
        };
        return View(viewModel);
    }

    public IActionResult Details(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
}

public class ProductIndexViewModel
{
    public List<Product> Products { get; set; }
    public List<Category> Categories { get; set; }
}
