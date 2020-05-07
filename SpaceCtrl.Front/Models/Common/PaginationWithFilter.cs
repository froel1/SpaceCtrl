namespace SpaceCtrl.Front.Models.Common
{
    public class PaginationWithFilter<TFilter> : Pagination
        where TFilter : class
    {
        public TFilter? Filter { get; set; }
    }

    public class Pagination
    {
        public uint Index { get; set; }
        public uint Size { get; set; }
    }
}