namespace telebibcore22.api.Helpers
{
    public class CustomerParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string MinA107 { get; set; } = "";
        public string MaxA107 { get; set; } = "zzzzzz";
        /*  public string OrderBy { get; set; } */
    }
}