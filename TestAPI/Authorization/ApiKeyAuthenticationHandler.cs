using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TestAPI.Resources;

namespace TestAPI.Authorization
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private ApiKeyRepository database;
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,  ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ApiKeyRepository keyRepository) :
            base(options, logger, encoder, clock)
        {
            keyRepository.Database.EnsureCreated();
            database = keyRepository;
        }

        

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string apiKeyValue = Request.Headers[ConstantNames.API_TOKEN_PREFIX];

            if (string.IsNullOrEmpty(apiKeyValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            
            var salesChannel = database.ApiKeys.FirstOrDefault(l => l.ApiKeyValue == Guid.Parse(apiKeyValue).GetHashCode());

            if (salesChannel is null)
            {
                return Task.FromResult(AuthenticateResult.Fail($"token {ConstantNames.API_TOKEN_PREFIX} not match"));
            }
            else
            {
                var id = new ClaimsIdentity(
                    new Claim[] { new Claim("Key", apiKeyValue) },  // not safe , just as an example , should custom claims on your own
                    Scheme.Name
                );
                ClaimsPrincipal principal = new ClaimsPrincipal(id);
                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }
    }
}
