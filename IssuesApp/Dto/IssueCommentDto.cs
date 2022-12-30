namespace IssuesApp.Dto
{
    public class IssueCommentDto
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime DateInserted { get; set; }
        public int IssueId { get; set; }
        public int AdminId { get; set; }
    }
}
