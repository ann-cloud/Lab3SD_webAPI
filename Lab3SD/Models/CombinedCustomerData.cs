namespace Lab3SD.Models;

public class CombinedCustomerData
{
    public List<Customer> FilteredCustomers { get; set; }
    public List<CustomerOrderHistory> OrderHistory { get; set; }
}
