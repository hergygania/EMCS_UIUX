namespace App.Service.POST
{
    public partial class PaginationParam
    {
        public int Skip { get; set; }

        public int Page { get; set; }

        public int Pagesize { get; set; }

        public string Sortby { get; set; }

        public string Sortorder { get; set; }
    }
}
