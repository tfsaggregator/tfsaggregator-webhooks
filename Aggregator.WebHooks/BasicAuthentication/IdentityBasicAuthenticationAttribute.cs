using Aggregator.Core.Monitoring;
using Aggregator.WebHooks.Utils;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace BasicAuthentication.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        private ILogEvents2 logger;

        public IdentityBasicAuthenticationAttribute(ILogEvents2 logger)
        {
            this.logger = logger;
        }

        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {

            string token = string.Empty;
            var usersCollection = ConfigurationManager.GetSection("Users") as NameValueCollection;
            if (usersCollection != null)
            {
                token = usersCollection[userName].ToString();
            }

            if (token != password)
            {
                logger.BasicAuthenticationFailed(userName);
                // No user with userName/password exists.
                return null;
            }

            logger.BasicAuthenticationSucceeded(userName);

            // Create a ClaimsIdentity with all the claims for this user.
            ClaimsIdentity identity = new ClaimsIdentity("Basic");
            identity.AddClaim(new Claim("username", userName));
            return new ClaimsPrincipal(identity);
        }
    }
}