using AspCertsDemo.Properties;
using Google.Protobuf;
using Grpc.Core;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AspCertsDemo.Services;

public class CertsMakeService : AspCertsMake.AspCertsMakeBase
{
    public CertsMakeService()
    {

    }

    public override Task<MakeCertReply> MakeCert(MakeCertRequest request, ServerCallContext context)
    {
        var rootca = new X509Certificate2(Resources.rootca_pfx, "1234");

        var ecdsa = ECDsa.Create();
        var req = new CertificateRequest(rootca.Issuer, ecdsa, HashAlgorithmName.SHA256);
        var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(20));

        var b = cert.Export(X509ContentType.Pfx, "123456");

        var reply = new MakeCertReply
        {
            Code = 0,
            Tip = rootca.Issuer,
            Cert = ByteString.CopyFrom(b),
        };
        return Task.FromResult(reply);
    }
}
