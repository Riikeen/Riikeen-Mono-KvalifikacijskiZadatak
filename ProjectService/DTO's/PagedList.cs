using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectService.DTO_s
{
    public class PagedList<T> 
    {
        private PagedList (List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get;  }
        public int Page { get;  }
        public int PageSize { get; }
        public int TotalCount { get; }


        public static async Task<PagedList<T>> CreateAsync (IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = await query.CountAsync();
            var items = await query.Skip(page-1)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedList<T>(items,page,page,totalCount);
        }
    }
}
