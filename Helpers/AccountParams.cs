namespace telebibcore22.api.Helpers
{
    public class AccountParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public string MinV020 { get; set; } = "";
        public string MaxV020 { get; set; } = "zzzzzz";
        /*  public string OrderBy { get; set; } */
    }
}