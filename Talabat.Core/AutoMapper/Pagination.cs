namespace Talabat.Core.AutoMapper
{
    public class Pagination<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Count { get; set; }
		public IReadOnlyList<T> Data { get; set; }

        public Pagination(int PageIndex, int PageSize, long Count, IReadOnlyList<T> Data)
        {
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
            this.Count = Count;
            this.Data = Data;
        }
    }
}
