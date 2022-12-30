using IssuesApp.Dto;
using IssuesApp.Models;

namespace IssuesApp.Interfaces
{
    public interface IAdminRepository
    {
        ICollection<Admin> GetAdmins();
        Admin GetAdmin(int AdminId);
        Admin CheckAdminEmail(AdminDto adminCreate);
        bool CreateAdmin(Admin admin);
        bool UpdateAdmin(Admin admin);
        bool DeleteAdmin(Admin admin);
        bool Save();
        bool AdminExists(int AdminId);
    }
}
