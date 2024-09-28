using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        await _context.SaveChangesAsync(); 

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
            throw new Exception("Order not found.");
        }

        
        order.OrderNo = orderDto.OrderNo;
        order.OrderDate = orderDto.OrderDate;
        order.ComCustomerId = orderDto.ComCustomerId;
        order.Address = orderDto.Address;

        
        if (orderDto.Items != null)
        {
            
            foreach (var existingItem in order.Items.ToList())
            {
                if (!orderDto.Items.Any(i => i.SoItemId == existingItem.SoItemId))
                {
                    _context.SoItems.Remove(existingItem);
                }
            }

            
            foreach (var itemDto in orderDto.Items)
            {
                if (itemDto.SoItemId > 0)
                {
                    
                    var existingItem = order.Items.FirstOrDefault(i => i.SoItemId == itemDto.SoItemId);
                    if (existingItem != null)
                    {
                        existingItem.ItemName = itemDto.ItemName;
                        existingItem.Quantity = itemDto.Quantity;
                        existingItem.Price = itemDto.Price;
                    }
                }
                else
                {
                    
                    var newItem = _mapper.Map<SoItem>(itemDto);
                    newItem.SoOrderId = order.SoOrderId;
                    _context.SoItems.Add(newItem);
                }
            }
        }

        await _context.SaveChangesAsync();

        
        await _context.Entry(order).Reference(o => o.Customer).LoadAsync();
        await _context.Entry(order).Collection(o => o.Items).LoadAsync();

        return _mapper.Map<OrderDto>(order);
    }

    public IQueryable<OrderDto> GetOrdersQueryable(string keyword, DateTime? orderDate)
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

        return query.ProjectTo<OrderDto>(_mapper.ConfigurationProvider);
    }

    public async Task DeleteOrderAsync(long id)
    {
        var order = await _context.SoOrders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.SoOrderId == id);

        if (order != null)
        {
            _context.SoItems.RemoveRange(order.Items);
            _context.SoOrders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}