using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Controllers
{
    
    /// <summary>
    /// 用户token管理
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private ILogger<TokenController> Logger;

        public TokenController(ILogger<TokenController> logger)
        {
            Logger = logger;
        }
        /// <summary>
        /// 获取一个token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> CreateToken([FromBody] UserModel userInfo)
        {
            //IdentityModelEventSource.ShowPII = true;
            Logger.LogDebug("收到token创建请求，请求信息为：{userInfo}",userInfo);
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
                {JwtRegisteredClaimNames.Sub, "test"},
                {JwtRegisteredClaimNames.UniqueName, "test"},
                {JwtRegisteredClaimNames.Iss, issuer},
                {JwtRegisteredClaimNames.Iat, now},
                {JwtRegisteredClaimNames.Nbf, now},
                {JwtRegisteredClaimNames.Exp, exp},
                {"roles",new string[]{"Admin","user"}},
                {JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")}
            };
            var jwt = new JwtSecurityToken(jwtHeader, payload);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            Logger.LogDebug("颁发一个token：{token}",token);
            return Ok(token);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> Get()
        {
            var x509 = new X509Certificate2("eshopIdentityPub.cer");
            var certStr = Convert.ToBase64String(x509.Export(X509ContentType.Cert));
            string userName = User.Identity.Name;
            return Ok(userName);
        }
    }
}