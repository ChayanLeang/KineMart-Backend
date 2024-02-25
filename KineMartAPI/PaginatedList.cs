namespace KineMartAPI
{
    public class PaginatedList<T> : List<T>
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public PaginatedList(List<T> items,int count,int pageNumber,int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber < 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        public static PaginatedList<T> Create(IEnumerable<T> source,int pageNumber,int pageSize)
        {
            int count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
