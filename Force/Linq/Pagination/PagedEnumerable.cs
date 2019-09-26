using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Force.Linq.Pagination
{
    public class PagedEnumerable: IEnumerable
    {
        public IEnumerable Items { get; protected set; }

        public long Total { get; protected set;}

        public PagedEnumerable(IEnumerable items, long total)
        {
            Total = total;
            Items = items;
        }

        public IEnumerator GetEnumerator() => Items.GetEnumerator();
    }
    public class PagedEnumerable<T>: PagedEnumerable, IEnumerable<T>
    {
        public new IEnumerable<T> Items { get; }

        public PagedEnumerable(IOrderedQueryable<T> queryable, IPaging paging)
            : base(queryable.Paginate(paging).ToList(), queryable.Count())
        {
        }

        public PagedEnumerable(IEnumerable<T> items, long total)
            // ReSharper disable once PossibleMultipleEnumeration
            : base(items, total)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            Items = items;
        }

        public new IEnumerator<T> GetEnumerator()
            => Items.GetEnumerator();
    }
}