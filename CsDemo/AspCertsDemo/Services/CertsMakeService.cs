using AspCertsDemo.Properties;
using Google.Protobuf;
using Grpc.Core;
using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace AspCertsDemo.Services;

public class CertsMakeService : AspCertsMake.AspCertsMakeBase
{
    private ILogger<CertsMakeService> logger;

    public CertsMakeService(ILogger<CertsMakeService> logger)
    {
        this.logger = logger;
    }

    public override Task<MakeCertReply> MakeCert(MakeCertRequest request, ServerCallContext context)
    {
        using var rootca = new X509Certificate2(Resources.rootca_pfx, "1234");
        //PrintChain(rootca);

        // ecdsa 算法
        //var ecdsa = ECDsa.Create();
        //var req = new CertificateRequest(rootca.Issuer, ecdsa, HashAlgorithmName.SHA256);

        //using var cert = req.Create(rootca, DateTimeOffset.Now, rootca.NotAfter, sn);

        //using var rootca = EnsureRootCert();
        //using var cert = MakeClientCert(rootca);
        using var cert = MakeClientCertRsa2(rootca);
        //using var cert = MakeClientCertChain(rootca);

        //var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(20));

        logger.LogInformation("make certs yyyyyy {}", cert.ExportCertificatePem());

        var reply = new MakeCertReply
        {
            Code = 0,
            Tip = cert.SerialNumber,
            Cert = ByteString.CopyFrom(cert.Export(X509ContentType.Pfx, "123456")),
            Pem = cert.ExportCertificatePem(),
            Cap = ByteString.CopyFrom(rootca.PublicKey.ExportSubjectPublicKeyInfo()),
        };
        return Task.FromResult(reply);
    }

    public static X509Certificate2 MakeRootCert()
    {
        using RSA rsa = RSA.Create(4096);
        var req = new CertificateRequest(
            "CN=MyCARootSelfSigned",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        req.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(true, false, 0, true)
        );
        req.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(req.PublicKey, false)
        );
        return req.CreateSelfSigned(
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddYears(20)
        );
    }

    public static X509Certificate2 EnsureRootCert(string path="aaa.pfx")
    {
        if (File.Exists(path))
        {
            return new X509Certificate2(path, "1234");
        }
        else
        {
            var rootca = MakeRootCert();
            ExportCertPfxTo(rootca, path);
            return rootca;
        }
    }

    public static void ExportCertPfxTo(X509Certificate2 cert, string path, string password="1234")
    {
        var bytes = cert.Export(X509ContentType.Pfx, password);
        File.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// TODO 带上 CA 证书
    /// 
    /// 
    /// </summary>
    /// <param name="rootca"></param>
    /// <returns></returns>
    public static X509Certificate2 MakeClientCertChain(X509Certificate2 rootca)
    {
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest(
            rootca.Issuer,
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        req.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, true)
        );
        req.CertificateExtensions.Add(
            new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true)
        );
        var issuerSubjectKey = rootca.Extensions["Subject Key Identifier"]!.RawData;
        var segment = new ArraySegment<byte>(issuerSubjectKey, 2, issuerSubjectKey.Length - 2);
        var authorityKeyIdentifer = new byte[segment.Count + 4];
        // these bytes define the "KeyID" part of the AuthorityKeyIdentifer
        authorityKeyIdentifer[0] = 0x30;
        authorityKeyIdentifer[1] = 0x16;
        authorityKeyIdentifer[2] = 0x80;
        authorityKeyIdentifer[3] = 0x14;
        segment.CopyTo(authorityKeyIdentifer, 4);
        req.CertificateExtensions.Add(new X509Extension("2.5.29.35", authorityKeyIdentifer, false));
        // DPS samples create certs with the device name as a SAN name 
        // in addition to the subject name
        var sanBuilder = new SubjectAlternativeNameBuilder();
        sanBuilder.AddDnsName(rootca.SubjectName.Name);
        var sanExtension = sanBuilder.Build();
        req.CertificateExtensions.Add(sanExtension);

        // Enhanced key usages
        req.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection {
                    new Oid("1.3.6.1.5.5.7.3.2"), // TLS Client auth
                    new Oid("1.3.6.1.5.5.7.3.1")  // TLS Server auth
                },
                false));
        // add this subject key identifier
        req.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(req.PublicKey, false));

        var signer = X509SignatureGenerator.CreateForRSA(rootca.GetRSAPrivateKey()!, RSASignaturePadding.Pkcs1);
        var sn = new byte[16];
        Random.Shared.NextBytes(sn);
        //return req.Create(
        //    rootca,
        //    DateTimeOffset.Now,
        //    rootca.NotAfter,
        //    sn
        //).CopyWithPrivateKey(rsa);
        return req.Create(
            rootca.IssuerName,
            signer,
            DateTimeOffset.Now,
            rootca.NotAfter,
            sn
        ).CopyWithPrivateKey(rsa);
    }

    /// <summary>
    /// 生成客户端证书，客户端可用，CA 验证会失败。
    /// options.RevocationMode = X509RevocationMode.NoCheck;
    /// 不检查可以让代码自动生成的证书通过，打开手动命令签的可以，代码自动生成的不行（会进入 OnAuthenticationFailed）。
    /// TODO 尝试修改生成证书代码，使得打开检查也能和手动命令签的一样通过。
    /// tip: 应该是代码生成的不是 chain certs ，没有带上 CA 的 Pubkey 导致不能通过 CA 的验证。
    /// </summary>
    /// <param name="rootca"></param>
    /// <returns></returns>
    public static X509Certificate2 MakeClientCertRsa2(X509Certificate2 rootca)
    {
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest(
            rootca.Issuer,
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        req.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false)
        );
        req.CertificateExtensions.Add(
            new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, false)
        );
        // 会导致客户端使用证书时， “处理证书时，出现了一个未知错误。”
        //req.CertificateExtensions.Add(
        //    new X509EnhancedKeyUsageExtension(
        //        new OidCollection
        //        {
        //            new Oid("1.3.6.1.5.5.7.3.8"),//timeStamping  PKIX key purpose timeStamping
        //        },
        //        true
        //    )
        //);
        req.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(req.PublicKey, false)
        );
        var signer = X509SignatureGenerator.CreateForRSA(rootca.GetRSAPrivateKey()!, RSASignaturePadding.Pkcs1);
        var sn = new byte[16];
        Random.Shared.NextBytes(sn);
        return req.Create(rootca.IssuerName, signer, DateTimeOffset.Now, rootca.NotAfter, sn).CopyWithPrivateKey(rsa);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public static X509Certificate2 MakeClientCert(X509Certificate2 root)
    {
        using RSA rsa = RSA.Create(2048);
        var req = new CertificateRequest(
            root.Issuer,
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
        req.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false)
        );
        req.CertificateExtensions.Add(
            new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, false)
        );
        req.CertificateExtensions.Add(
            new X509EnhancedKeyUsageExtension(
                new OidCollection
                {
                    new Oid("1.3.6.1.5.5.7.3.8"),//timeStamping  PKIX key purpose timeStamping
                },
                true
            )
        );
        req.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(req.PublicKey, false)
        );
        var signer = X509SignatureGenerator.CreateForRSA(root.GetRSAPrivateKey()!, RSASignaturePadding.Pkcs1);
        var sn = new byte[16];
        Random.Shared.NextBytes(sn);

        // 命令生成的 CARoot 缺少 X509BasicConstraintsExtension 是通过不了这个，要用下面的。
        return req.Create(
            root,
            DateTimeOffset.Now,
            DateTimeOffset.Now.AddYears(1),
            sn
        ).CopyWithPrivateKey(rsa);
        //return req.Create(
        //    root.IssuerName,
        //    signer,
        //    DateTimeOffset.Now,
        //    DateTimeOffset.Now.AddYears(1),
        //    sn
        //).CopyWithPrivateKey(rsa);
    }


    public void PrintChain(X509Certificate2 certificate)
    {
        var ch = new X509Chain();
        ch.ChainPolicy.RevocationMode = X509RevocationMode.Online;
        ch.Build(certificate);
        logger.LogInformation("Chain Information");
        logger.LogInformation("Chain revocation flag: {0}", ch.ChainPolicy.RevocationFlag);
        logger.LogInformation("Chain revocation mode: {0}", ch.ChainPolicy.RevocationMode);
        logger.LogInformation("Chain verification flag: {0}", ch.ChainPolicy.VerificationFlags);
        logger.LogInformation("Chain verification time: {0}", ch.ChainPolicy.VerificationTime);
        logger.LogInformation("Chain status length: {0}", ch.ChainStatus.Length);
        logger.LogInformation("Chain application policy count: {0}", ch.ChainPolicy.ApplicationPolicy.Count);
        logger.LogInformation("Chain certificate policy count: {0} {1}", ch.ChainPolicy.CertificatePolicy.Count, Environment.NewLine);

        //Output chain element information.
        logger.LogInformation("Chain Element Information");
        logger.LogInformation("Number of chain elements: {0}", ch.ChainElements.Count);
        logger.LogInformation("Chain elements synchronized? {0} {1}", ch.ChainElements.IsSynchronized, Environment.NewLine);

        foreach (X509ChainElement element in ch.ChainElements)
        {
            logger.LogInformation("Element issuer name: {0}", element.Certificate.Issuer);
            logger.LogInformation("Element certificate valid until: {0}", element.Certificate.NotAfter);
            logger.LogInformation("Element certificate is valid: {0}", element.Certificate.Verify());
            logger.LogInformation("Element error status length: {0}", element.ChainElementStatus.Length);
            logger.LogInformation("Element information: {0}", element.Information);
            logger.LogInformation("Number of element extensions: {0}{1}", element.Certificate.Extensions.Count, Environment.NewLine);

            if (ch.ChainStatus.Length > 1)
            {
                for (int index = 0; index < element.ChainElementStatus.Length; index++)
                {
                    logger.LogInformation("{}", element.ChainElementStatus[index].Status);
                    logger.LogInformation("{}", element.ChainElementStatus[index].StatusInformation);
                }
            }
        }
    }
}
