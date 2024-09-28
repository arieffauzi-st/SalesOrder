using System.ComponentModel.DataAnnotations;

namespace SalesOrder.DTOs;

public class OrderDto
{
    public long SoOrderId { get; set; }

    [Required(ErrorMessage = "Order Number is required")]
    public string OrderNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Order Date is required")]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "Customer is required")]
    public int ComCustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    // If you need to include a list of customers for dropdown
    public List<CustomerDto>? Customers { get; set; }
    public List<ItemDto> Items { get; set; } = new List<ItemDto>();
}
