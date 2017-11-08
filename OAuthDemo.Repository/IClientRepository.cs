using System.Threading.Tasks;
using OAuthDemo.Core.Infrastructure;
using OAuthDemo.Domain;

namespace OAuthDemo.Repository
{
    public interface IClientRepository : IRepository
    {
        Task<Client> GetClient(string id);
        Task<int> AddClient(Client client);
        Task<int> RemoveClient(string id);
    }
}