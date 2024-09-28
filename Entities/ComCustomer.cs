using System;

namespace SalesOrder.Entities;

public class ComCustomer
{
    public int ComCustomerId { get; set; }
    public string? CustomerName { get; set; }

    // Navigation property
    public List<SoOrder> Orders { get; set; } = [];

}
