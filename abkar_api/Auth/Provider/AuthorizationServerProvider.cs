using System.Linq;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using abkar_api.Contexts;
using abkar_api.Models;
using Microsoft.Owin;
using System;

namespace abkar_api.Auth.Provider
{
    public class AuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        DatabaseContext db = new DatabaseContext();
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
           context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            Personnel p = db.personnels.Where(pp => pp.username == context.UserName && pp.password == context.Password).FirstOrDefault();
            string role = db.departments.Find(p.department_id).role;

            if (p == null) context.SetError("invalid_grant", "Kullanıcı adı veya şifre yanlış.");

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
              identity.AddClaim(new Claim("name", p.name + " " + p.lastname));
                identity.AddClaim(new Claim("id", p.id.ToString()));
               identity.AddClaim(new Claim(ClaimTypes.Role, role));
            //identity.AddClaim(new Claim("user","asdasd1"));
            context.Validated(identity);
        }

    
        



    }
}