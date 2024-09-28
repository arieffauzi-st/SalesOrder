using SalesOrder.Models; 

namespace SalesOrder.Repositories
{
    public interface IExportRepository
    {
        Task<IEnumerable<OrderExportModel>> GetFilteredOrdersForExportAsync(string keyword, DateTime? orderDate, int page, int pageSize);
    }
}
