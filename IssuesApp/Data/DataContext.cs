using IssuesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IssuesApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }

        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssueComment> IssuesComments { get; set; }
    }
}
