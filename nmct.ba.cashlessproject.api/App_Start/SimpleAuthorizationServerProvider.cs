using Microsoft.Owin.Security.OAuth;
using nmct.ba.cashlessproject.api.Models.DA;
using nmct.ba.cashlessproject.model.Model.Costumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace nmct.ba.cashlessproject.api.App_Start
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Vereniging org = VerenigingDA.CheckCredentials(context.UserName, context.Password);
            if (org == null)
            {
                context.Rejected();
                return Task.FromResult(0);
            }

            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("id", org.Id.ToString()));
            id.AddClaim(new Claim("dbname", org.DbName));
            id.AddClaim(new Claim("dblogin", org.DbLogin));
            id.AddClaim(new Claim("dbpass", org.DbPassword));

            context.Validated(id);
            return Task.FromResult(0);
        }
    }
}