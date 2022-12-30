namespace IssuesApp.Models
{
    public class IssueComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime DateInserted { get; set; }
        public int IssueId { get; set; }
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual Issue Issue { get; set; }
    }
}
