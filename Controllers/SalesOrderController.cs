using Microsoft.AspNetCore.Mvc;
using SalesOrder.Interfaces;
using SalesOrder.DTOs;
using System.Text.Json;
using SalesOrder.Models;

namespace SalesOrder.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public SalesOrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index(string keyword, DateTime? orderDate, int pageNumber = 1)
        {
            int pageSize = 5; 
            var query = _orderRepository.GetOrdersQueryable(keyword, orderDate);
            var paginatedOrders = await PaginatedList<OrderDto>.CreateAsync(query, pageNumber, pageSize);

            ViewBag.Keyword = keyword;
            ViewBag.OrderDate = orderDate?.ToString("yyyy-MM-dd");

            return View(paginatedOrders);
        }

        public async Task<IActionResult> Edit(int id, int? pageNumber)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Customers = await _orderRepository.GetCustomersAsync();

            int pageSize = 10;
            var query = _orderRepository.GetOrdersQueryable(null, null);
            var paginatedOrders = await PaginatedList<OrderDto>.CreateAsync(query, pageNumber ?? 1, pageSize);
            
            ViewData["PaginatedOrders"] = paginatedOrders;
            
            return View(order);
        }

        public async Task<IActionResult> Add()
        {
            var customers = await _orderRepository.GetCustomersAsync();
            var orderDto = new OrderDto
            {
                Customers = customers,
                OrderDate = DateTime.Now
            };
            return View(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderDto orderDto, string Items)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Items))
                {
                    orderDto.Items = JsonSerializer.Deserialize<List<ItemDto>>(Items, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                var result = await _orderRepository.CreateOrderAsync(orderDto);
                return Ok(result);
                
            }
            orderDto.Customers = await _orderRepository.GetCustomersAsync();
            return View(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDto orderDto, string Items)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Items))
                {
                    orderDto.Items = JsonSerializer.Deserialize<List<ItemDto>>(Items, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                var result = await _orderRepository.UpdateOrderAsync(orderDto);
                return Ok(result);
            }
            orderDto.Customers = await _orderRepository.GetCustomersAsync();
            return View(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _orderRepository.DeleteOrderAsync(id);
                TempData["SuccessMessage"] = "Order deleted successfully.";
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred while deleting the order: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner exception: " + ex.InnerException.Message;
                }
                TempData["ErrorMessage"] = errorMessage;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}