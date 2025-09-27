namespace BookingClinic.Services.Helpers.PaginationHelper
{
    public interface IPaginationHelper<T>
    {
        int GetTotalPages(IEnumerable<T> entities, int pageSize);
        IEnumerable<T> Paginate(IEnumerable<T> entities, int page, int pageSize, out List<int> pages);
    }
}
