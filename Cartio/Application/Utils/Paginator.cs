using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cartio.Application.Utils
{
    public class Paginator<T>
    {
        private Paginator(
            List<T> items,
            int page,
            int itemsPerPage,
            int totalCount)
        {
            Items = items;
            Page = page;
            ItemsPerPage = itemsPerPage;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }
        public int Page { get; }
        public int ItemsPerPage { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * ItemsPerPage < TotalCount;
        public bool HasPreviousPage => ItemsPerPage > 1;

        public static async Task<Paginator<T>> CreateAsync(
            IQueryable<T> query,
            int page,
            int itemsPerPage)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

            return new Paginator<T>(items, page, itemsPerPage, totalCount);
        }

    }
}
