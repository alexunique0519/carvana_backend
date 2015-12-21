using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;
using Microsoft.Owin;
using carpool_web_backend.Models;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.OAuth;
using carpool_web_backend.Provider;
using System.Configuration;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using System.Data.Entity;

[assembly: OwinStartup(typeof(carpool_web_backend.Startup))]
namespace carpool_web_backend
{
    public class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            HttpConfiguration httpConfig = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);

            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }
        
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {

            

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/accounts/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                //will be changed when deploy
               
                AccessTokenFormat = new CustomJWTFormat("http://carvana.azurewebsites.net/")
            };
                
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }
        
        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            //need to be changed when deploy
            var issuer = "http://carvana.azurewebsites.net/";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            //apu controller with an [authorize] attriute will be validated with jwt
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
          
        }
    }
}