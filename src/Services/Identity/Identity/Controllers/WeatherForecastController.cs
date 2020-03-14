using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            IdentityModelEventSource.ShowPII = true;
            var x509 = new X509Certificate2("eshopIdentity.pfx","123456");
            var rsa = x509.GetRSAPrivateKey();
            var privateKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(privateKey,SecurityAlgorithms.RsaSha256);
            var jwtHeader = new JwtHeader(signingCredentials);
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddDays(7);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var issuer = "eshopIdentity";
            var payload = new JwtPayload
            {
                {"sub", "test"},
                {"unique_name", "test"},
                {"iss", issuer},
                {"iat", now},
                {"nbf", now},
                {"exp", exp},
                {"jti", Guid.NewGuid().ToString("N")}
            };
            var jwt = new JwtSecurityToken(jwtHeader, payload);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(token);
        }

        [HttpPost]
        public ActionResult<string> test(int id)
        {
            var x509 = new X509Certificate2("eshopIdentity.pfx","123456");
            var rsa = x509.GetRSAPrivateKey();
            var privateKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(privateKey,SecurityAlgorithms.Sha256);
            var jwtHeader = new JwtHeader(signingCredentials);
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddDays(7);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            var issuer = "eshopIdentity";
            var payload = new JwtPayload
            {
                {"sub", "test"},
                {"unique_name", "test"},
                {"iss", issuer},
                {"iat", now},
                {"nbf", now},
                {"exp", exp},
                {"jti", Guid.NewGuid().ToString("N")}
            };
            var jwt = new JwtSecurityToken(jwtHeader, payload);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(token);
        }
    }
}
