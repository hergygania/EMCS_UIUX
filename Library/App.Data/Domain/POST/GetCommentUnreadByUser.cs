namespace App.Data.Domain.POST
{
    public partial class GetCommentUnreadByUser
    {
        public long RequestId { get; set; }
        public long CommentId { get; set; }
    }
}
