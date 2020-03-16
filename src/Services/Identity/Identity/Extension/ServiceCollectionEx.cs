using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Identity.Extension
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var x509 = new X509Certificate2(configuration["TokenKey:TokenPubCert"]);
            var rsa = x509.GetRSAPublicKey();
            var publicKey = new RsaSecurityKey(rsa);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    //x.ClaimsIssuer = "eshopIdentity";
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = publicKey,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidIssuer = "eshopIdentity"

                    };
                });
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}