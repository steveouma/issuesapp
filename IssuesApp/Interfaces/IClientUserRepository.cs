using IssuesApp.Models;

namespace IssuesApp.Interfaces
{
    public interface IClientUserRepository
    {
        ICollection<ClientUser> GetClientUsers();
        ClientUser GetClientUser(int clientUserId);
        bool CreateClientUser(ClientUser clientuser);
        bool UpdateClientUser(ClientUser clientuser);
        bool DeleteClientUser(ClientUser clientuser);
        bool Save();
        bool ClientUserExists(int clientUserId);
    }
}
