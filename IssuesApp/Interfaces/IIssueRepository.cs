using IssuesApp.Models;

namespace IssuesApp.Interfaces
{
    public interface IIssueRepository
    {
        ICollection<Issue> GetIssues();
        ICollection<Issue> GetClientIssues(int clientId);
        Issue GetIssue(int issueId);
        bool IssueExists(int issueId);
        bool CreateIssue(Issue issue);
        bool UpdateIssue(Issue issue);
        bool DeleteIssue(Issue issue);
        bool Save();
    }
}
