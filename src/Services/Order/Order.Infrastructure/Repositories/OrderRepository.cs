using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.ValueObjects;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext dbContext) : IOrderRepository
    {
        public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            return await dbContext.Orders
                .Include(x => x.OrderItems)
                .OrderBy(x => x.OrderName.Value)
                .Skip(pagination.PageSize * pagination.PageIndex)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Domain.Entities.Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await dbContext.Orders.FindAsync([orderId, cancellationToken], cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersByCustomerAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await dbContext.Orders
               .Include(x => x.OrderItems)
               .AsNoTracking()
               .Where(x => x.CustomerId == CustomerId.Of(customerId))
               .OrderBy(x => x.OrderName.Value)
               .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Domain.Entities.Order>> GetOrdersByNameAsync(string orderName, CancellationToken cancellationToken)
        {
            return await dbContext.Orders
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .Where(x => x.OrderName.Value.Contains(orderName))
                .OrderBy(x => x.OrderName.Value)
                .ToListAsync(cancellationToken);
        }

        public async Task AddOrderAsync(Domain.Entities.Order order, CancellationToken cancellationToken)
        {
            await dbContext.Orders.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken); //TODO: Use UnitOfWork
        }

        public async Task UpdateOrderAsync(Domain.Entities.Order order, CancellationToken cancellationToken)
        {
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken); //TODO: Use UnitOfWork
        }

        public async Task DeleteOrderAsync(Domain.Entities.Order order, CancellationToken cancellationToken)
        {
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken); //TODO: Use UnitOfWork
        }
    }
}