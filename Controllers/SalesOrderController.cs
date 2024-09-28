using Microsoft.AspNetCore.Mvc;
using SalesOrder.Interfaces;
using SalesOrder.DTOs;
using System.Text.Json;

namespace SalesOrder.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public SalesOrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index(string keyword, DateTime? orderDate)
        {
            var orders = await _orderRepository.GetOrdersAsync(keyword, orderDate);
            ViewBag.Keyword = keyword;
            ViewBag.OrderDate = orderDate?.ToString("yyyy-MM-dd");
            return View(orders);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Customers = await _orderRepository.GetCustomersAsync();
            return View(order);
        }

        public async Task<IActionResult> Add()
        {
            var customers = await _orderRepository.GetCustomersAsync();
            var orderDto = new OrderDto
            {
                Customers = customers,
                OrderDate = DateTime.Now // Set default date to today
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
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
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
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            orderDto.Customers = await _orderRepository.GetCustomersAsync();
            return View(orderDto);
        }
    }
}