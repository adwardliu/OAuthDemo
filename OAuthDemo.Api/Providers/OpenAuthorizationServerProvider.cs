using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using OAuthDemo.Domain;
using OAuthDemo.Service;

namespace OAuthDemo.Api.Providers
{
    public class OpenAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IClientService _clientService;
        public OpenAuthorizationServerProvider(IClientService clientService)
        {
            _clientService = clientService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = new Client { AllowedOrigin = "*", RefreshTokenLifeTime = 30 };

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            ////验证客户端,客户端信息已提前写入数据库中
            //if (context.ClientId == null)
            //{
            //    context.Validated();
            //    context.SetError("invalid_clientId", "ClientId should be sent.");
            //    return;
            //}

            //client = await _clientService.GetClient(context.ClientId);
            //if (client == null)
            //{
            //    context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
            //    return;
            //}

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            //用户身份验证

            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, new Guid("7D648C4A-0AA2-4C76-99FB-6A21E97ABD92").ToString()));

            var props = new AuthenticationProperties(new Dictionary<string, string> { { "as:client_id", context.ClientId ?? string.Empty } });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);

            await base.GrantResourceOwnerCredentials(context);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            ////刷新验证码进行客户端验证
            //var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            //var currentClient = context.ClientId;

            //if (originalClient != currentClient)
            //{
            //    context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
            //    return Task.FromResult<object>(null);
            //}

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "refreshToken"));

            var employeeIdClaim = newIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}