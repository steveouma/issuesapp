namespace IssuesApp.Models
{
    public class Admin
    {
        public Admin() 
        {
            this.IssueComments = new HashSet<IssueComment>();
        }
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<IssueComment> IssueComments { get; set; }
    }
}
