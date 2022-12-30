using IssuesApp.Data;
using IssuesApp.Interfaces;
using IssuesApp.Models;

namespace IssuesApp.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly DataContext _context;
        public IssueRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateIssue(Issue issue)
        {
            _context.Add(issue);
            return Save();
        }

        public bool DeleteIssue(Issue issue)
        {
            _context.Remove(issue);
            return Save();
        }

        public ICollection<Issue> GetClientIssues(int clientId)
        {
            return _context.Issues.Where(x => x.ClientUserId == clientId).ToList();
        }

        public Issue GetIssue(int issueId)
        {
            return _context.Issues.Where(x => x.IssueId == issueId).FirstOrDefault();
        }

        public ICollection<Issue> GetIssues()
        {
            return _context.Issues.OrderBy(x => x.IssueId).ToList();
        }

        public bool IssueExists(int issueId)
        {
            return _context.Issues.Any(x => x.IssueId == issueId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateIssue(Issue issue)
        {
            _context.Update(issue);
            return Save();
        }
    }
}
