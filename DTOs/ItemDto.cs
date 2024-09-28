using System;

namespace SalesOrder.DTOs;

public class ItemDto
{
    public long SoItemId { get; set; }
    public long SoOrderId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double Price { get; set; }

}
