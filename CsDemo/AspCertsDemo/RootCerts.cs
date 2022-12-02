using System.Security.Cryptography.X509Certificates;
using AspCertsDemo.Properties;

namespace AspCertsDemo;

public static class RootCerts
{
    public static void Register()
    {
        var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        store.Open(OpenFlags.MaxAllowed);
        var rootCerts = new X509Certificate2(Resources.rootca_pfx, "1234");
        var findResult = store.Certificates.Find(X509FindType.FindByThumbprint, rootCerts.Thumbprint, false);

        try
        {
            if (findResult is null)
            {
                store.Add(rootCerts);
            }
        }
        finally
        {
            store.Close();
        }
    }
}
