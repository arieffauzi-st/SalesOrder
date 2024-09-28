using System;

namespace SalesOrder.Entities;

public class SoItem
{
    public long SoItemId { get; set; } //long
    public long SoOrderId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Price { get; set; }

    // Navigation property
    public SoOrder Order { get; set; } = null!;

}
