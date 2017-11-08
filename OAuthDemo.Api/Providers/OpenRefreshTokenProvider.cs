using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using OAuthDemo.Domain;
using OAuthDemo.Service;

namespace OAuthDemo.Api.Providers
{
    public class OpenRefreshTokenProvider : AuthenticationTokenProvider
    {
        private readonly IRefreshTokenService _refreshTokenService;
        public OpenRefreshTokenProvider(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// 生成 refresh_token
        /// </summary>
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientId)) return;

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");
            if (string.IsNullOrEmpty(refreshTokenLifeTime)) return;

            RandomNumberGenerator cryptoRandomDataGenerator = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[50];
            cryptoRandomDataGenerator.GetBytes(buffer);
            var refreshTokenId = Convert.ToBase64String(buffer).TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var refreshToken = new RefreshToken
            {
                Id = refreshTokenId,
                ClientId = clientId,
                UserName = context.Ticket.Identity.Name ?? string.Empty,
                Subject = string.Empty,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime));
            //先设置过期时间再序列化,否则token过期无法刷新
            refreshToken.ProtectedTicket = context.SerializeTicket();

            var result = await _refreshTokenService.AddRefreshToken(refreshToken);

            if (result > 0)
            {
                context.SetToken(refreshTokenId);
            }
        }

        /// <summary>
        /// 由 refresh_token 解析成 access_token
        /// </summary>
        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var refreshToken = await _refreshTokenService.GetRefreshToken(context.Token);
            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await _refreshTokenService.RemoveRefreshToken(context.Token);
            }
        }
    }
}