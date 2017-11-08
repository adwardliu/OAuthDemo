using System;
using System.Threading.Tasks;
using OAuthDemo.Domain;
using OAuthDemo.Repository;

namespace OAuthDemo.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<int> AddClient(Client client)
        {
            try
            {
                return await _clientRepository.AddClient(client);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Client> GetClient(string id)
        {
            try
            {
                return await _clientRepository.GetClient(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> RemoveClient(string id)
        {
            try
            {
                return await _clientRepository.RemoveClient(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}