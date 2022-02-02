namespace App.Data.Domain.POST
{
    public partial class GetCommentByRequest
    {
        public long RequestId { get; set; }
        public int TotalComment { get; set; }
        public int TotalRead { get; set; }
        public int TotalUnread { get; set; }
    }
}
