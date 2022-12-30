using IssuesApp.Data;
using IssuesApp.Interfaces;
using IssuesApp.Models;

namespace IssuesApp.Repositories
{
    public class IssueCommentRepository : IIssueCommentRepository
    {
        private readonly DataContext _context;
        public IssueCommentRepository(DataContext context)
        {
            _context = context;
        }
        public bool CommentExists(int id)
        {
            return _context.IssuesComments.Any(c => c.Id == id);
        }

        public bool CreateComment(IssueComment issuecomment)
        {
            _context.Add(issuecomment);
            return Save();
        }

        public bool DeleteComment(IssueComment comment)
        {
            _context.Remove(comment);
            return Save();
        }

        public IssueComment GetComment(int commentId)
        {
            return _context.IssuesComments.Where(icom => icom.Id == commentId).FirstOrDefault();
        }

        public ICollection<IssueComment> GetComments()
        {
            return _context.IssuesComments.OrderBy(x => x.Id).ToList();
        }

        public ICollection<IssueComment> GetIssueComments(int issueId)
        {
            return _context.IssuesComments.Where(comm => comm.IssueId == issueId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true: false;
        }

        public bool UpdateComment(IssueComment comment)
        {
            _context.Update(comment);
            return Save();
        }
    }
}
