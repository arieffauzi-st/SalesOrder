using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesOrder.DTOs;
using SalesOrder.Entities;
using SalesOrder.Interfaces;

namespace SalesOrder.Data;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderDto>> GetOrdersAsync(string keyword, DateTime? orderDate)
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

        var orders = await query.ToListAsync();
        return _mapper.Map<List<OrderDto>>(orders);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        var order = await _context.SoOrders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.SoOrderId == id);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
    {
        var order = _mapper.Map<SoOrder>(orderDto);
        
        if (orderDto.ComCustomerId != 0)
        {
            order.Customer = await _context.ComCustomers.FindAsync(orderDto.ComCustomerId);
        }

        _context.SoOrders.Add(order);
        await _context.SaveChangesAsync(); // This will generate SoOrderId

        if (orderDto.Items != null && orderDto.Items.Any())
        {
            foreach (var itemDto in orderDto.Items)
            {
                var item = _mapper.Map<SoItem>(itemDto);
                item.SoOrderId = order.SoOrderId;
                _context.SoItems.Add(item);
            }
            await _context.SaveChangesAsync();
        }

        // Reload the order with its items and customer
        await _context.Entry(order).Reference(o => o.Customer).LoadAsync();
        await _context.Entry(order).Collection(o => o.Items).LoadAsync();

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<List<CustomerDto>> GetCustomersAsync()
    {
        var customers = await _context.ComCustomers.ToListAsync();
        return _mapper.Map<List<CustomerDto>>(customers);
    }

    public async Task<OrderDto> UpdateOrderAsync(OrderDto orderDto)
    {
        var order = await _context.SoOrders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.SoOrderId == orderDto.SoOrderId);

        if (order == null)
        {
            return null;
        }

        _context.Entry(order).CurrentValues.SetValues(orderDto);

        if (orderDto.ComCustomerId != 0)
        {
            order.Customer = await _context.ComCustomers.FindAsync(orderDto.ComCustomerId);
        }

        // Remove existing items
        foreach (var existingItem in order.Items.ToList())
        {
            if (!orderDto.Items.Any(i => i.SoItemId == existingItem.SoItemId))
            {
                _context.SoItems.Remove(existingItem);
            }
        }

        // Update or add items
        foreach (var itemDto in orderDto.Items)
        {
            var existingItem = order.Items.FirstOrDefault(i => i.SoItemId == itemDto.SoItemId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).CurrentValues.SetValues(itemDto);
            }
            else
            {
                var newItem = _mapper.Map<SoItem>(itemDto);
                newItem.SoOrderId = order.SoOrderId;
                order.Items.Add(newItem);
            }
        }

        await _context.SaveChangesAsync();

        // Reload the order with its items and customer
        await _context.Entry(order).Reference(o => o.Customer).LoadAsync();
        await _context.Entry(order).Collection(o => o.Items).LoadAsync();

        return _mapper.Map<OrderDto>(order);
    }
}