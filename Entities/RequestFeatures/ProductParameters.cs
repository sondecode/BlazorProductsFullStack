namespace Entities.RequestFeatures
{
    public class ProductParameters
    {
        const int maxPagesize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 4;
        public int PageSize { 
            get { return _pageSize; }
            set { _pageSize = (value > maxPagesize) ? maxPagesize: value; }
        }

        public string? SearchTerm { get; set; }
    }
}
