using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TreinaWeb.MinhaApi.Api.Providers
{
    public class SimpleAuthServerProvider : OAuthAuthorizationServerProvider
    {
        // valida se o cliente tem permissão de fato para acessar a API
        public async override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public async override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // indica que qualquer um pode fazer a utilização da api
            context.OwinContext.Response.Headers.Add("Access-Control_allow_Origin", new string[] { "*" });

            if (context.UserName != "treinaweb" || context.Password != "treinaweb")
            {
                context.SetError("invalid_user_or_password", "Usuário ou senha incorretos");                
            }
            else
            {
                ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
                context.Validated(identity);
            }
        }
    }
}