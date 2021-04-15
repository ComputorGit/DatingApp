using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items,int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            TotalPages = (int) Math.Ceiling(count /(double)pageSize);
            PageSize = pageSize;
            CurrentPage = pageNumber;
            AddRange(items);
        }

        public PagedList(int totalCount, int totalPages, int pageSize, int currentPage) 
        {
            this.TotalCount = totalCount;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;
            this.CurrentPage = currentPage;
   
        }
         public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }




        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber,int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber,pageSize);
        }
    }
}