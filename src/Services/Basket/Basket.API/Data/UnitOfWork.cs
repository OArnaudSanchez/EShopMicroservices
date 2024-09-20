namespace Basket.API.Data
{
    public sealed class UnitOfWork(IDocumentSession session) : IUnitOfWork, IDisposable
    {
        public void Dispose()
        {
            session.Dispose();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}