using System.Threading.Tasks;
using OAuthDemo.Core.Infrastructure;
using OAuthDemo.Domain;

namespace OAuthDemo.Service
{
    public interface IRefreshTokenService : IApplicationService
    {
        Task<RefreshToken> GetRefreshToken(string id);
        Task<int> AddRefreshToken(RefreshToken refreshToken);
        Task<int> RemoveRefreshToken(string id);
    }
}