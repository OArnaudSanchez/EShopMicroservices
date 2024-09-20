namespace Basket.API.Data
{
    public interface IUnitOfWork
    {
        //TODO: Finish this implementation
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}