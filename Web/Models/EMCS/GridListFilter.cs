namespace App.Web.Models.EMCS
{
    public class GridListFilterModel
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public string Term { get; set; }
        public string SearchName { get; set; }
    }
}