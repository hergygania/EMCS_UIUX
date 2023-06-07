namespace App.Data.Domain.POST
{
    public partial class GetTotalCommentUnreadByUser
    {
        public long RequestId { get; set; }
        public int TotalComment { get; set; }
        public int TotalUnread { get; set; }
    }
}
