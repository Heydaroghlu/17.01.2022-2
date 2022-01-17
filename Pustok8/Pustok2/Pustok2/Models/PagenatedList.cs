using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
    public class PagenatedList<T>:List<T>
    {
        public PagenatedList(List<T> items,int count,int pageindex,int pagesize)
        {
            this.AddRange(items);
            PageIndex = pagesize;
            TotalPage = (int)Math.Ceiling(count / (double)pagesize);
        }
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public bool Hasprev
        {
            get
            {
                return PageIndex > 1;
            }
        }
        public bool HasNext
        {
            get
            {
                return TotalPage > PageIndex;
            }
        }
        public static PagenatedList<T> Create(IQueryable<T> query,int pageIndex,int pageSize)
        {
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagenatedList<T>(items, query.Count(), pageIndex, pageSize);
        }
    }
}
