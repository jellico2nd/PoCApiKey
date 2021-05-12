using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TestAPI.Resources;

namespace TestAPI.Authorization
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,  ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
            base(options, logger, encoder, clock)
        {
            
        }

        

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string apiKeyValue = Request.Headers[ConstantNames.API_TOKEN_PREFIX];

            if (string.IsNullOrEmpty(apiKeyValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            //using (DBContext db = new DBContext())
            //{
            //    var salesChannel = db.SalesChannel.FirstOrDefault(l => l.Apikey == authorization);  // query db
            //}

            //if (salesChannel is null)
            if (!apiKeyValue.Equals("Yes", StringComparison.OrdinalIgnoreCase))
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
