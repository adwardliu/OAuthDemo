using System.Data;

namespace OAuthDemo.Core.Infrastructure
{
    public interface IDataSource
    {
        IDbConnection GetConnection();
    }
}