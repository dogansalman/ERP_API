using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Web.Http;
using abkar_api.Auth.Provider;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(abkar_api.Auth.Startup))]
namespace abkar_api.Auth
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            ConfigureOAuth(appBuilder);
            WebApiConfig.Register(httpConfiguration);
           // appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.UseWebApi(httpConfiguration);
        }
        private void ConfigureOAuth(IAppBuilder appBuilder)
        {
            OAuthAuthorizationServerOptions oAuthAuthorizationServerOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/auth"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(20),
                AllowInsecureHttp = true,
                Provider = new AuthorizationServerProvider()
            };
            appBuilder.UseOAuthAuthorizationServer(oAuthAuthorizationServerOptions);

            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}