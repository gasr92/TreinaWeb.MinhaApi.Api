using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using TreinaWeb.MinhaApi.Api.Providers;

[assembly: OwinStartup(typeof(TreinaWeb.MinhaApi.Api.Startup))]

namespace TreinaWeb.MinhaApi.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            HttpConfiguration config = new HttpConfiguration();

            // chamar este método após a criação do HttpConfiguration
            ConfigureOauth(app);

            WebApiConfig.Register(config);

            app.UseCors(CorsOptions.AllowAll); // permite o Cors a partir de todos os lugares
            app.UseWebApi(config);            
        }

        private void ConfigureOauth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions oAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true, // informa que não é necessário a utilização de HTTPS
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(120), // o token fica válido por 30 segundos
                Provider = new SimpleAuthServerProvider(),
            };

            app.UseOAuthAuthorizationServer(oAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
