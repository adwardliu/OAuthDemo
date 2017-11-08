using System;
using System.Threading.Tasks;
using OAuthDemo.Domain;
using OAuthDemo.Repository;

namespace OAuthDemo.Service
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<int> AddRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                return await _refreshTokenRepository.AddRefreshToken(refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<RefreshToken> GetRefreshToken(string id)
        {
            try
            {
                return await _refreshTokenRepository.GetRefreshToken(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> RemoveRefreshToken(string id)
        {
            try
            {
                return await _refreshTokenRepository.RemoveRefreshToken(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}