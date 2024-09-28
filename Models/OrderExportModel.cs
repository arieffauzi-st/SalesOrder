using System;

namespace SalesOrder.Models;

public class OrderExportModel
{
    public long OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}
