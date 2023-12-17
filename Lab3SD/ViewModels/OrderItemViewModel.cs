namespace Lab3SD.ViewModels;

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }
}