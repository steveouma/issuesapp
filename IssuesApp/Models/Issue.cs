namespace IssuesApp.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int ResolutionState { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public int ClientUserId { get; set; }
        public virtual ClientUser ClientUser { get; set; }
    }
}
