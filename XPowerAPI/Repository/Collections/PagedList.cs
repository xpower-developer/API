using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XPowerAPI.Repository.Collections
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IPagedList{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the data to page</typeparam>
    public class PagedList<T> : IPagedList<T>
    {
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <value>The page number.</value>
        public int PageNumber { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; private set; }
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; private set; }
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages { get; private set; }
        /// <summary>
        /// Gets or sets the index from.
        /// </summary>
        /// <value>The index from.</value>
        public int IndexFrom { get; private set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        public IList<T> Items { get; private set; }

        /// <summary>
        /// Gets whether the page has a previous page.
        /// </summary>
        /// <value>The has previous page.</value>
        public bool HasPreviousPage => PageNumber - IndexFrom > 0;

        /// <summary>
        /// Gets the has next page.
        /// </summary>
        /// <value>The has next page.</value>
        public bool HasNextPage => PageNumber - IndexFrom + 1 < TotalPages;

        /// <summary>
        /// Creates a new instance of a PagedList containing the source elements
        /// </summary>
        /// <param name="source">the source elements to be contained within the list</param>
        /// <param name="count">the total amount of items within the collection</param>
        /// <param name="pageSize">the size of a single page</param>
        /// <param name="pageNumber">the index of the page</param>
        /// <param name="indexFrom">the index to start from</param>
        public PagedList(IEnumerable<T> source, int count, int pageSize, int pageNumber, int indexFrom) {
            if (indexFrom > pageNumber)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageNumber}, indexFrom must be less than or equal pageIndex");
            }

            PageNumber = pageNumber;
            PageSize = pageSize;
            IndexFrom = indexFrom;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            Items = source.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}" /> class.
        /// </summary>
        public PagedList() => Items = Array.Empty<T>();
    }
}
