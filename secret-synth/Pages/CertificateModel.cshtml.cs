using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography.X509Certificates;

namespace secret_synth.Pages
{
    public class CertificateModelModel : PageModel
    {
        // PKI folder contains the following files:
        // Private:
        // domain.pfx - pfx certificate protected by a password
        // domain-nopass.pfx - pfx certificate without password protection
/*[0]*/// domain.key - private key for the certificate, encrypted with a password "Nope" - yes, this one should be a valid finding too (don't store passwords in comments)
        // domain-nopass.key - private key for the certificate, plain-text
        // rootCA.key - private key for the root CA
        // Public (can be stored, no sensitive info)
        // domain.crt - corresponding private key for certificate
        // domain.csr - code signign request (CSR) for the certificate - no private info in CSR
        // domain.der - DER encoded certificate
        // domain.pem - PEM encoded certificate


        private static string certDir = Directory.GetCurrentDirectory() + "\\PKI\\";
        string certDirPrivate = certDir + "private\\";
        string certDirPublic = certDir + "public\\";

/*[1]*/string certPass = "Nope";

        public void OnGet()
        {
            // just to have a reference
            var nopasscertPath = certDirPrivate + "domain-nopass.pfx";
            var withpasscertPath = certDirPrivate + "domain.pfx";

            /* Possible issues
             * 1. pfx certificate without password protection
             * 2. Hard-coded password for pfx certificate 
             * 
             */
            // load pfx certificate without a password from nopasscertPath
/*[2]*/     var nopasscert = new X509Certificate2(nopasscertPath, "", X509KeyStorageFlags.Exportable);
            // load pfx certificate with a password from nopasscertPath
/*[3]*/     var withpasscert = new X509Certificate2(withpasscertPath, certPass, X509KeyStorageFlags.Exportable);
        }
    }
}
