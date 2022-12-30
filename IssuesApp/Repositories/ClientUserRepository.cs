using IssuesApp.Data;
using IssuesApp.Interfaces;
using IssuesApp.Models;

namespace IssuesApp.Repositories
{
    public class ClientUserRepository : IClientUserRepository
    {
        private readonly DataContext _context;
        public ClientUserRepository(DataContext context) 
        {
            _context = context;
        }

        public bool ClientUserExists(int ClientUserId)
        {
            return _context.ClientUsers.Any(cu => cu.ClientUserId == ClientUserId);
        }

        public bool CreateClientUser(ClientUser clientuser)
        {
            _context.Add(clientuser);
            return Save();
        }

        public bool DeleteClientUser(ClientUser clientuser)
        {
            _context.Remove(clientuser);
            return Save();
        }

        public ClientUser GetClientUser(int clientUserId)
        {
            return _context.ClientUsers.Where(cu => cu.ClientUserId == clientUserId).FirstOrDefault();
        }

        public ICollection<ClientUser> GetClientUsers()
        {
            return _context.ClientUsers.OrderBy(cu => cu.ClientUserId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateClientUser(ClientUser clientuser)
        {
            _context.Update(clientuser);
            return Save();
        }
    }
}
