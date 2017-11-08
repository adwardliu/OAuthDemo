using OAuthDemo.Core.Infrastructure;
using System.Threading.Tasks;
using OAuthDemo.Domain;

namespace OAuthDemo.Service
{
    public interface IClientService : IApplicationService
    {
        Task<Client> GetClient(string id);
        Task<int> AddClient(Client client);
        Task<int> RemoveClient(string id);
    }
}
