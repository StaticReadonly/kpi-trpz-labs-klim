using BookingClinic.Application.Interfaces.Helpers;

namespace BookingClinic.Application.Helpers
{
    public class PaginationHelper<T> : IPaginationHelper<T>
    {
        public int GetTotalPages(IEnumerable<T> entities, int pageSize)
        {
            var cnt = entities.Count();

            var pages = cnt / pageSize;

            if (cnt % pageSize != 0 || pages == 0)
            {
                pages++;
            }

            return pages;
        }

        public IEnumerable<T> Paginate(IEnumerable<T> entities, int page, int pageSize, out List<int> pages)
        {
            if (page < 1)
            {
                page = 1;
            }

            int totalPages = GetTotalPages(entities, pageSize);
            int lower = page - 3;

            if (lower < 1)
            {
                lower = 1;
            }

            int upper = page + 3;

            if (upper > totalPages)
            {
                upper = totalPages;
            }

            pages = Enumerable.Range(lower, upper - lower + 1).ToList();

            return entities.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
