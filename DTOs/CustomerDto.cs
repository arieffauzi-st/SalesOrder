using System;

namespace SalesOrder.DTOs;

public class CustomerDto
{
    public int ComCustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}
