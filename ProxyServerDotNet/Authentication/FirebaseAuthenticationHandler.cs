using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProxyServerDotNet.Authentication
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            FirebaseApp firebaseApp) 
            : base(options, logger, encoder, clock)
        {
            _firebaseApp = firebaseApp;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Context.Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.NoResult();
                }

                string bearerToken = Context.Request.Headers["Authorization"];

                if (bearerToken == null || !bearerToken.StartsWith("Bearer "))
                {
                    return AuthenticateResult.Fail("Invalid Scheme");
                }

                string token = bearerToken.Substring("Bearer ".Length);
                FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);

                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(FirebaseAuthenticationHandler))
                }), JwtBearerDefaults.AuthenticationScheme));
            }
            catch (Exception exp)
            {

                return AuthenticateResult.Fail(exp);
            }

        }

        private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {
                new Claim("id", claims["user_id"].ToString()),
                new Claim("email", claims["email"].ToString())
            };
        }
    }
}
