using IssuesApp.Data;
using IssuesApp.Dto;
using IssuesApp.Interfaces;
using IssuesApp.Models;

namespace IssuesApp.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        public AdminRepository(DataContext context) 
        {
            _context = context;
        }

        public bool AdminExists(int AdminId)
        {
            return _context.Admins.Any(a => a.AdminId == AdminId);
        }

        public Admin CheckAdminEmail(AdminDto adminCreate)
        {
            return GetAdmins().Where(a => a.Email == adminCreate.Email).FirstOrDefault();
        }

        public bool CreateAdmin(Admin admin)
        {
            _context.Add(admin);
            return Save();
        }

        public bool DeleteAdmin(Admin admin)
        {
            _context.Remove(admin);
            return Save();
        }

        public Admin GetAdmin(int AdminId)
        {
            return _context.Admins.Find(AdminId);
        }

        public ICollection<Admin> GetAdmins()
        {
            return _context.Admins.OrderBy(a => a.AdminId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateAdmin(Admin admin)
        {
            _context.Update(admin);
            return Save();
        }
    }
}
