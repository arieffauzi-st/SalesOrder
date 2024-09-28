using System;

namespace SalesOrder.Entities;

public class SoOrder
{
    public long SoOrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public int ComCustomerId { get; set; }
    public string Address { get; set; } = string.Empty;

    // Navigation properties
    public ComCustomer Customer { get; set; } = null!;
    public ICollection<SoItem> Items { get; set; } = [];

}
