using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OAuthDemo.Domain;

namespace OAuthDemo.Repository
{
    public class ClientRepository : IClientRepository
    {
        #region Json文件存储
        private string _jsonFilePath;
        private readonly List<Client> _clients;

        public ClientRepository()
        {
            _jsonFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Clients.json";
            if (File.Exists(_jsonFilePath))
            {
                var json = File.ReadAllText(_jsonFilePath);
                _clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }

            //var json = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/clients.json"));
            //_clients = JsonConvert.DeserializeObject<Client[]>(json);
        }

        public async Task<Client> GetClient(string id)
        {
            return _clients.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> AddClient(Client client)
        {
            return 1;
        }

        public async Task<int> RemoveClient(string id)
        {
            return 0;
        }
        #endregion

        #region 数据库存储
        //private readonly IDataSource _dataSource;

        //public ClientRepository(IDataSource dataSource)
        //{
        //    _dataSource = dataSource;
        //}

        //public async Task<int> AddClient(Client client)
        //{
        //    string sql = @"INSERT INTO [dbo].[OAuth_Client]([Id],[Secret],[Name],[Active],[RefreshTokenLifeTime],[AllowedOrigin])
        //                   VALUES(@Id,@Secret,@Name,@Active,@RefreshTokenLifeTime,@AllowedOrigin)";
        //    using (var connection = _dataSource.GetConnection())
        //    {
        //        if (connection.State != ConnectionState.Open) connection.Open();
        //        return await connection.ExecuteAsync(sql, client);
        //    }
        //}

        //public async Task<Client> GetClient(string id)
        //{
        //    string sql = "SELECT [Id],[Secret],[Name],[Active],[RefreshTokenLifeTime],[AllowedOrigin] FROM [dbo].[OAuth_Client] WITH(NOLOCK) WHERE Id=@Id";
        //    using (var connection = _dataSource.GetConnection())
        //    {
        //        if (connection.State != ConnectionState.Open) connection.Open();
        //        return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { Id = id });
        //    }
        //}

        //public async Task<int> RemoveClient(string id)
        //{
        //    string sql = "DELETE FROM [dbo].[OAuth_Client] WHERE Id=@Id";
        //    using (var connection = _dataSource.GetConnection())
        //    {
        //        if (connection.State != ConnectionState.Open) connection.Open();
        //        return await connection.ExecuteAsync(sql, new { Id = id });
        //    }
        //}
        #endregion
    }
}