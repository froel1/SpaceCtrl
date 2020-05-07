using System.Collections.Generic;

namespace SpaceCtrl.Front.Models.Common
{
    public class PagedList<TData>
    {
        public int Count { get; set; }

        public List<TData> Data { get; set; }

        public PagedList(int count, List<TData> data)
        {
            Count = count;
            Data = data;
        }
    }
}