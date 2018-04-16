using System.Linq;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using System.Security.Claims;
using abkar_api.Contexts;
using abkar_api.Models;
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

            var body = await context.Request.ReadFormAsync();
            var type = body["type"] != null ? body["type"] : null;
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);


            // Supplier Auth
            if (type != null && type == "supplier")
            {
                Suppliers customer = db.suppliers.Where(c => c.email == context.UserName && c.password == context.Password).FirstOrDefault();
                if (customer == null)
                {
                    context.SetError("invalid_grant", "Email or password is wrong !");
                    return;
                }
                identity.AddClaim(new Claim("supplier", customer.company));
                identity.AddClaim(new Claim("id", customer.id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, "supplier"));
                AuthenticationProperties _properties = CreateCustomerProperties(customer.company);
                AuthenticationTicket _ticket = new AuthenticationTicket(identity, _properties);
      
        

                context.Validated(_ticket);
                context.Validated(identity);
                return;
            }

            // Customer Auth
            if (type != null && type == "customer")
            {
                Customers customer = db.customers.Where(c => c.email == context.UserName && c.password == context.Password).FirstOrDefault();
                if (customer == null) {
                    context.SetError("invalid_grant", "Email or password is wrong !");
                    return;
                }
                identity.AddClaim(new Claim("company", customer.company));
                identity.AddClaim(new Claim("id", customer.id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Role, "customer"));
                AuthenticationProperties _properties = CreateCustomerProperties(customer.company);
                AuthenticationTicket _ticket = new AuthenticationTicket(identity, _properties);
                context.Validated(_ticket);
                context.Validated(identity);
                return;
            }

            // Personnel Auth
            Personnel p = db.personnels.Where(pp => pp.username == context.UserName && pp.password == context.Password).FirstOrDefault();
            if (p == null)
            {
                context.SetError("invalid_grant", "Kullanıcı adı veya şifre yanlış.");
                return;
            }
            string role = db.departments.Find(p.department_id).role;
            AuthenticationProperties properties = CreatePersonnelProperties(p.name + " " + p.lastname, role);

            identity.AddClaim(new Claim("name", p.name + " " + p.lastname));
            identity.AddClaim(new Claim("id", p.id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);
        }
      
        public static AuthenticationProperties CreatePersonnelProperties(string fullname, string Roles)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "fullname", fullname },
                { "roles", Roles},
                { "date", DateTime.Now.ToString()}
            };
           return new AuthenticationProperties(data);
        }
        public static AuthenticationProperties CreateCustomerProperties(string company)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "company", company },
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