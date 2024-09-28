using Microsoft.EntityFrameworkCore;
using SalesOrder.Entities;
using System;
using System.Linq;

namespace SalesOrder.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ComCustomers.Any())
            {
                return; // Database sudah diisi
            }

            // Seed Customers
            var customers = new ComCustomer[]
            {
                new ComCustomer { CustomerName = "AAA" },
                new ComCustomer { CustomerName = "BBB" },
                new ComCustomer { CustomerName = "CCC" },
                new ComCustomer { CustomerName = "DDD" },
                new ComCustomer { CustomerName = "EEE" }
            };

            context.ComCustomers.AddRange(customers);
            context.SaveChanges();

            // Seed Orders
            var orders = new SoOrder[]
            {
                new SoOrder { OrderNo = "SO_001", OrderDate = new DateTime(2024, 1, 1), ComCustomerId = customers[0].ComCustomerId, Address = "Address 1" },
                new SoOrder { OrderNo = "SO_002", OrderDate = new DateTime(2024, 1, 15), ComCustomerId = customers[1].ComCustomerId, Address = "Address 2" },
                new SoOrder { OrderNo = "SO_003", OrderDate = new DateTime(2024, 2, 1), ComCustomerId = customers[2].ComCustomerId, Address = "Address 3" },
                new SoOrder { OrderNo = "SO_004", OrderDate = new DateTime(2024, 2, 15), ComCustomerId = customers[3].ComCustomerId, Address = "Address 4" },
                new SoOrder { OrderNo = "SO_005", OrderDate = new DateTime(2024, 3, 1), ComCustomerId = customers[4].ComCustomerId, Address = "Address 5" },
                new SoOrder { OrderNo = "SO_006", OrderDate = new DateTime(2024, 3, 15), ComCustomerId = customers[0].ComCustomerId, Address = "Address 6" },
                new SoOrder { OrderNo = "SO_007", OrderDate = new DateTime(2024, 4, 1), ComCustomerId = customers[1].ComCustomerId, Address = "Address 7" },
                new SoOrder { OrderNo = "SO_008", OrderDate = new DateTime(2024, 4, 15), ComCustomerId = customers[2].ComCustomerId, Address = "Address 8" },
                new SoOrder { OrderNo = "SO_009", OrderDate = new DateTime(2024, 5, 1), ComCustomerId = customers[3].ComCustomerId, Address = "Address 9" },
                new SoOrder { OrderNo = "SO_010", OrderDate = new DateTime(2024, 5, 15), ComCustomerId = customers[4].ComCustomerId, Address = "Address 10" }
            };

            context.SoOrders.AddRange(orders);
            context.SaveChanges();

            // Seed Items
            var items = new SoItem[]
            {
                new SoItem { SoOrderId = orders[0].SoOrderId, ItemName = "KULKAS", Quantity = 2, Price = 10000 },
                new SoItem { SoOrderId = orders[0].SoOrderId, ItemName = "AC", Quantity = 1, Price = 15000 },
                new SoItem { SoOrderId = orders[1].SoOrderId, ItemName = "MESIN CUCI", Quantity = 3, Price = 5000 },
                new SoItem { SoOrderId = orders[1].SoOrderId, ItemName = "MESIN JAHIT", Quantity = 2, Price = 7500 },
                new SoItem { SoOrderId = orders[2].SoOrderId, ItemName = "AC", Quantity = 1, Price = 20000 },
                new SoItem { SoOrderId = orders[2].SoOrderId, ItemName = "KULKAS", Quantity = 4, Price = 10000 },
                new SoItem { SoOrderId = orders[3].SoOrderId, ItemName = "AC", Quantity = 2, Price = 15000 },
                new SoItem { SoOrderId = orders[3].SoOrderId, ItemName = "KULKAS", Quantity = 1, Price = 5000 },
                new SoItem { SoOrderId = orders[4].SoOrderId, ItemName = "MESIN JAHIT", Quantity = 3, Price = 7500 },
                new SoItem { SoOrderId = orders[4].SoOrderId, ItemName = "AC", Quantity = 2, Price = 20000 },
                new SoItem { SoOrderId = orders[5].SoOrderId, ItemName = "AC", Quantity = 1, Price = 10000 },
                new SoItem { SoOrderId = orders[5].SoOrderId, ItemName = "KULKAS", Quantity = 3, Price = 15000 },
                new SoItem { SoOrderId = orders[6].SoOrderId, ItemName = "AC", Quantity = 2, Price = 5000 },
                new SoItem { SoOrderId = orders[6].SoOrderId, ItemName = "KULKAS", Quantity = 1, Price = 7500 },
                new SoItem { SoOrderId = orders[7].SoOrderId, ItemName = "AC", Quantity = 4, Price = 20000 },
                new SoItem { SoOrderId = orders[7].SoOrderId, ItemName = "KULKAS", Quantity = 2, Price = 10000 },
                new SoItem { SoOrderId = orders[8].SoOrderId, ItemName = "AC", Quantity = 1, Price = 15000 },
                new SoItem { SoOrderId = orders[8].SoOrderId, ItemName = "MESIN JAHIT", Quantity = 3, Price = 5000 },
                new SoItem { SoOrderId = orders[9].SoOrderId, ItemName = "AC", Quantity = 2, Price = 7500 },
                new SoItem { SoOrderId = orders[9].SoOrderId, ItemName = "KULKAS", Quantity = 1, Price = 20000 }
            };

            context.SoItems.AddRange(items);
            context.SaveChanges();
        }
    }
}