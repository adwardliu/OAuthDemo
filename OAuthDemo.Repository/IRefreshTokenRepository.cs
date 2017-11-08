using System.Threading.Tasks;
using OAuthDemo.Core.Infrastructure;
using OAuthDemo.Domain;

namespace OAuthDemo.Repository
{
    public interface IRefreshTokenRepository : IRepository
    {
        Task<RefreshToken> GetRefreshToken(string id);
        Task<int> AddRefreshToken(RefreshToken refreshToken);
        Task<int> RemoveRefreshToken(string id);
    }
}
