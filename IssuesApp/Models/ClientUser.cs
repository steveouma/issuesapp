namespace IssuesApp.Models
{
    public class ClientUser
    {
        public ClientUser() 
        {
            this.Issues= new HashSet<Issue>();
        }
        public int ClientUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }
}
