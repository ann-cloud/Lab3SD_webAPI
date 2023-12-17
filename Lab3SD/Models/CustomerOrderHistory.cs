namespace Lab3SD.Models;

public class CustomerOrderHistory
{
    public int OrderId { get; set; }
    public string ProductsOrdered { get; set; }
    public decimal TotalSum { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
}