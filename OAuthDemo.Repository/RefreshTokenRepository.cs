using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OAuthDemo.Domain;

namespace OAuthDemo.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        #region Json文件保存
        private string _jsonFilePath;
        private List<RefreshToken> _refreshTokens;

        public RefreshTokenRepository()
        {
            _jsonFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "/Data/RefreshToken.json";
            if (File.Exists(_jsonFilePath))
            {
                var json = File.ReadAllText(_jsonFilePath);
                _refreshTokens = JsonConvert.DeserializeObject<List<RefreshToken>>(json);
            }
            
            if (_refreshTokens == null) _refreshTokens = new List<RefreshToken>();
        }

        public async Task<RefreshToken> GetRefreshToken(string id)
        {
            return _refreshTokens.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> AddRefreshToken(RefreshToken refreshToken)
        {
            _refreshTokens.Add(refreshToken);
            await WriteJsonToFile();
            return 1;
        }

        public async Task<int> RemoveRefreshToken(string Id)
        {
            _refreshTokens.RemoveAll(x => x.Id == Id);
            await WriteJsonToFile();
            return 1;
        }

        private async Task WriteJsonToFile()
        {
            using (var tw = TextWriter.Synchronized(new StreamWriter(_jsonFilePath, false)))
            {
                await tw.WriteAsync(JsonConvert.SerializeObject(_refreshTokens, Formatting.Indented));
            }
        }

        #endregion

        #region 数据库存储
        //private readonly IDataSource _dataSource;

        //public RefreshTokenRepository(IDataSource dataSource)
        //{
        //    _dataSource = dataSource;
        //}

        //public async Task<int> AddRefreshToken(RefreshToken refreshToken)
        //{
        //    string sql = @"INSERT INTO [dbo].[OAuth_RefreshToken]([Id],[UserName],[Subject],[ClientId],[IssuedUtc],[ExpiresUtc],[ProtectedTicket])
        //                   VALUES(@Id, @UserName, @Subject, @ClientId, @IssuedUtc, @ExpiresUtc, @ProtectedTicket)";
        //    using (var connection = _dataSource.GetConnection())
        //    {
        //        if (connection.State != ConnectionState.Open) connection.Open();
        //        return await connection.ExecuteAsync(sql, refreshToken);
        //    }
        //}

        //public async Task<RefreshToken> GetRefreshToken(string id)
        //{
        //    string sql = "SELECT [Id],[UserName],[Subject],[ClientId],[IssuedUtc],[ExpiresUtc],[ProtectedTicket] FROM [dbo].[OAuth_RefreshToken] WITH(NOLOCK) WHERE Id=@Id";
        //    using (var connection = _dataSource.GetConnection())
        //    {
        //        if (connection.State != ConnectionState.Open) connection.Open();
        //        return await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Id = id });
        //    }
        //}

        //public async Task<int> RemoveRefreshToken(string id)
        //{
        //    string sql = "DELETE FROM [dbo].[OAuth_RefreshToken] WHERE Id=@Id";
        //    using (var connection = _dataSource.GetConnection())
        //    {
        //        if (connection.State != ConnectionState.Open) connection.Open();
        //        return await connection.ExecuteAsync(sql, new { Id = id });
        //    }
        //}
        #endregion
    }
}