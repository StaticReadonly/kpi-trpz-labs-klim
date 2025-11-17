namespace BookingClinic.Application.Interfaces.UnitOfWork
{
    public interface IDbTransaction
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
