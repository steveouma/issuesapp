using IssuesApp.Models;

namespace IssuesApp.Interfaces
{
    public interface IIssueCommentRepository
    {
        ICollection<IssueComment> GetComments();
        ICollection<IssueComment> GetIssueComments(int issueId);
        IssueComment GetComment(int id);
        bool CreateComment(IssueComment issuecomment);
        bool CommentExists(int id);
        bool UpdateComment(IssueComment comment);
        bool DeleteComment(IssueComment comment);
        bool Save();
    }
}
