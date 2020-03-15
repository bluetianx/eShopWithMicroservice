using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Xunit;

namespace IdentityTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.KeySize = 2048;
            var req = new CertificateRequest("cn=EshopIdentity",rsa,HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.MaxValue);
            File.WriteAllBytes("eshopIdentity.pfx",cert.Export(X509ContentType.Pfx,"123456"));
            File.WriteAllBytes("eshopIdentityPub.cer",cert.Export(X509ContentType.Cert));
            var x509 = new X509Certificate2("eshopIdentityPub.cer");
            var testStr = "hello";
            var testByte = Encoding.UTF8.GetBytes(testStr);
            var rsaPrivate = cert.GetRSAPrivateKey();
            RSACryptoServiceProvider rsaCryptoServiceProvider= new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.ImportParameters(rsaPrivate.ExportParameters(true));
            var signData = rsaCryptoServiceProvider.SignData(testByte,new SHA256CryptoServiceProvider());
            cert.Export(X509ContentType.Cert);
            RSACryptoServiceProvider rsaCryptoServiceProvider2= new RSACryptoServiceProvider();
            var rsaPublickey = cert.GetRSAPublicKey();
            rsaCryptoServiceProvider2.ImportParameters(rsaPublickey.ExportParameters(false));
            var result = rsaCryptoServiceProvider2.VerifyData(testByte,
                new SHA256CryptoServiceProvider(),
                signData);
        }

        [Fact]
        public void Test2()
        {
            List<byte> bytes= new List<byte>();
            int i = 1;
            var tt = Encoding.ASCII.GetBytes("aab");
            var ii=IPAddress.HostToNetworkOrder(i);
            if (BitConverter.IsLittleEndian)//假如当前平台是按小端法序列化时
            {
                var barra = BitConverter.GetBytes(i);
                Array.Reverse(barra);
                bytes.AddRange(barra);
            }
            else
            {
                var barra = BitConverter.GetBytes(i);
                bytes.AddRange(barra);
            }

        }
        
        
    }
    [Serializable]
    public  struct TestStruct
    {
        public int length;
        public long SeqId;
        public int CmdId;
        public int CmdType;
        
        public byte[] Content;
    }
}