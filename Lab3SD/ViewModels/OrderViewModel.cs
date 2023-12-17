namespace Lab3SD.ViewModels;

public class OrderViewModel
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public string? Status { get; set; }

    public decimal? TotalSum { get; set; }
}