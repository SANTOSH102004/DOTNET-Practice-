namespace EcommerceWeb.Models;

public class CartItem
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Product?.Price * Quantity ?? 0;
}
