using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using SalesOrder.Data;
using SalesOrder.Models;


namespace SalesOrder.Controllers
{
    public class ExportController : Controller
    {
        private readonly AppDbContext _context;

        public ExportController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ExportToExcel(string keyword, DateTime? orderDate, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.SoOrders
                    .Include(o => o.Customer)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(o => o.OrderNo.Contains(keyword) || o.Customer.CustomerName.Contains(keyword));
                }

                if (orderDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);
                }

                var totalItems = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                page = Math.Min(Math.Max(1, page), totalPages);

                var orders = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(o => new OrderExportModel
                    {
                        OrderId = o.SoOrderId,
                        OrderDate = o.OrderDate,
                        CustomerName = o.Customer.CustomerName ?? string.Empty
                    })
                    .ToListAsync();

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Sales Orders");

                    
                    worksheet.Cell(1, 1).Value = "No";
                    worksheet.Cell(1, 2).Value = "Sales Order";
                    worksheet.Cell(1, 3).Value = "Order Date";
                    worksheet.Cell(1, 4).Value = "Customer";

                    
                    int row = 2;
                    foreach (var order in orders)
                    {
                        worksheet.Cell(row, 1).Value = row - 1;
                        worksheet.Cell(row, 2).Value = order.OrderId;
                        worksheet.Cell(row, 3).Value = order.OrderDate.ToString("yyyy-MM-dd");
                        worksheet.Cell(row, 4).Value = order.CustomerName;
                        row++;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"SalesOrders_Export_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while exporting to Excel.");
            }
        }
    }
}
