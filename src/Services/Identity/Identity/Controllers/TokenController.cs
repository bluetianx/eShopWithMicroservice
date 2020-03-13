using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        
        
        [HttpPost]
        public ActionResult<string> CreateToken([FromBody] string userId)
        {
            //IdentityModelEventSource.ShowPII = true;
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
    }
}