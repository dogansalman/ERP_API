using System.Linq;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using abkar_api.Contexts;
using abkar_api.Models;
using Microsoft.Owin;
using System;
using Microsoft.Owin.Security;
using System.Collections.Generic;

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

            AuthenticationProperties properties = CreateProperties(p.name + " " + p.lastname, role);

        
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("name", p.name + " " + p.lastname));
            identity.AddClaim(new Claim("id", p.id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            

            AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);


            context.Validated(ticket);
        }
      
        public static AuthenticationProperties CreateProperties(string fullname, string Roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "fullname", fullname },
                { "roles", Roles},
                { "date", DateTime.Now.ToString()}
            };
                return new AuthenticationProperties(data);
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