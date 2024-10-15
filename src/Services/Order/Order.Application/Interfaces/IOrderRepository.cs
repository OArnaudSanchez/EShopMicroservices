﻿using BuildingBlocks.Pagination;

namespace Order.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Domain.Entities.Order>> GetOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken);

        Task<Domain.Entities.Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken);//TODO: Receive func

        Task<IEnumerable<Domain.Entities.Order>> GetOrdersByNameAsync(string orderName, CancellationToken cancellationToken); //TODO: Receive func

        Task<IEnumerable<Domain.Entities.Order>> GetOrdersByCustomerAsync(Guid customerId, CancellationToken cancellationToken); //TODO: Receive func

        Task AddOrderAsync(Domain.Entities.Order order, CancellationToken cancellationToken); //TODO: Use UnitOfWork

        Task UpdateOrderAsync(Domain.Entities.Order order, CancellationToken cancellationToken); //TODO: Use UnitOfWork

        Task DeleteOrderAsync(Domain.Entities.Order order, CancellationToken cancellationToken); //TODO: Use UnitOfWork
    }
}