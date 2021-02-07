using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NLog;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    /// <summary>
    /// Provides methods to verify file certificate and signature
    /// </summary>
    public class FileCertVerifier : IDisposable
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly X509Certificate2? _rootCertificate;
        private readonly X509Certificate2 _fileSignCertificate;

        /// <summary>
        /// Initializes a new instance of the <c>FileCertVerifier</c> class using a sign certificate file name.
        /// </summary>
        /// <param name="signCertFilename">An sign certificate filename (*.cer)</param>
        /// <exception cref="CryptographicException">
        /// An error with the certificate occurs.
        /// For example: 
        ///     The certificate file does not exist. 
        ///     The certificate is invalid. 
        ///     The certificate's password is incorrect.
        /// </exception>
        public FileCertVerifier(string signCertFilename)
        {
            _fileSignCertificate = new X509Certificate2(signCertFilename);
        }

        /// <summary>
        /// Initializes a new instance of the <c>FileCertVerifier</c> class using information from a byte array
        /// </summary>
        /// <param name="signCertRawData">A byte array containing data from an X.509 sign certificate</param>
        /// <exception cref="CryptographicException">
        /// An error with the certificate occurs.
        /// For example: 
        ///     The certificate is invalid. 
        ///     The certificate's password is incorrect.
        /// </exception>
        public FileCertVerifier(byte[] signCertRawData)
        {
            _fileSignCertificate = new X509Certificate2(signCertRawData);
        }

        /// <summary>
        /// Initializes a new instance of the <c>FileCertVerifier</c> class using a root and sign certificate file names.
        /// </summary>
        /// <param name="rootCertFilename">Root certificate filename (*.cer)</param>
        /// <param name="signCertFilename">An sign certificate filename (*.cer)</param>
        /// <exception cref="CryptographicException">
        /// An error with the certificate occurs.
        /// For example: 
        ///     The certificate file does not exist. 
        ///     The certificate is invalid. 
        ///     The certificate's password is incorrect.
        /// </exception>
        public FileCertVerifier(string rootCertFilename, string signCertFilename)
        {
            _rootCertificate = new X509Certificate2(rootCertFilename);
            _fileSignCertificate = new X509Certificate2(signCertFilename);
        }

        /// <summary>
        /// Initializes a new instance of the <c>FileCertVerifier</c> class using information from a byte arrays
        /// </summary>
        /// <param name="rootCertRawData">A byte array containing data from an X.509 root certificate</param>
        /// <param name="signCertRawData">A byte array containing data from an X.509 sign certificate</param>
        /// <exception cref="CryptographicException">
        /// An error with the certificate occurs.
        /// For example: 
        ///     The certificate is invalid. 
        ///     The certificate's password is incorrect.
        /// </exception>
        public FileCertVerifier(byte[] rootCertRawData, byte[] signCertRawData)
        {
            _rootCertificate = new X509Certificate2(rootCertRawData);
            _fileSignCertificate = new X509Certificate2(signCertRawData);
        }

        public void Dispose()
        {
            DisposableUtils.Dispose(_rootCertificate);
            DisposableUtils.Dispose(_fileSignCertificate);
        }

        /// <summary>
        /// Verify file is signed only with one self-signed certificate and verify this certificate chain.
        /// </summary>
        /// <param name="filename">An filename to verify.</param>
        /// <returns>true if the file signed with certificate; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">The filename is null.</exception> 
        /// <exception cref="CryptographicException">The certificate is unreadable.</exception> 
        public bool VerifyFileCertificate(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            _logger.Info("Verify file certificate");
            using var fileCertificateCollection = DynamicDisposable<X509Certificate2Collection>.CreateNonNull(new X509Certificate2Collection());
            fileCertificateCollection.Object.Import(filename);
            if (fileCertificateCollection.Object.Count != 1)
            {
                _logger.Error($"Invalid certificates count: {fileCertificateCollection.Object.Count}");
                return false;
            }
            var fileCertificate = fileCertificateCollection.Object[0];
            if (fileCertificate.RawData.SequenceEqual(_fileSignCertificate.RawData))
            {
                using var chain = DynamicDisposable<X509Chain>.CreateNonNull(X509Chain.Create());
                if (_rootCertificate != null)
                {
                    chain.Object.ChainPolicy.ExtraStore.Add(_rootCertificate); // add CA cert for verification
                }
                chain.Object.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // no revocation checking
                chain.Object.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
                chain.Object.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                return chain.Object.Build(fileCertificate) && VerifyChain(chain.Object);
            }
            _logger.Error("Invalid certificate data");
            return false;
        }

        /// <summary>
        /// Verify file signature for current signed certificate.
        /// </summary>
        /// <param name="filename">An filename to verify.</param>
        /// <returns>true if the file signature is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">The filename is null.</exception> 
        /// <remarks>
        /// Warning: 
        ///     This method do not check certificate matched expected. 
        ///     Before use it always call <see cref="VerifyFileCertificate(string)"/>
        ///     or use method which verify both <see cref="VerifyFile(string)"/>
        /// </remarks>
        public bool VerifyFileSignature(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));
            _logger.Info("Verify file signature");
            var result = WinTrust.VerifyEmbeddedSignature(filename);
            _logger.Info($"Verify file signature result: {result}");
            return result == WinVerifyTrustResult.Success ||
                   result == WinVerifyTrustResult.UntrustedRoot ||
                   result == WinVerifyTrustResult.CertChaining;
        }

        /// <summary>
        /// Verify file is signed only with one self-signed certificate and verify it signature.
        /// </summary>
        /// <param name="filename">An filename to verify.</param>
        /// <returns>true if the file signed with certificate and signature is valid; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">The filename is null.</exception> 
        /// <exception cref="CryptographicException">The certificate is unreadable.</exception>
        public bool VerifyFile(string filename) => VerifyFileCertificate(filename) && VerifyFileSignature(filename);

        private bool VerifyChain(X509Chain chain)
        {
            _logger.Info("Verify certificate chain");
            if (VerifyChainStatus(chain.ChainStatus) && (chain.ChainElements.Count > 0))
            {
                if (_rootCertificate != null)
                {
                    return chain.ChainElements[chain.ChainElements.Count - 1].Certificate.RawData.SequenceEqual(_rootCertificate.RawData);
                }
                return true;
            }
            _logger.Error("Invalid certificate chain");
            return false;
        }

        private bool VerifyChainStatus(X509ChainStatus[] chainStatusArray)
        {
            if (chainStatusArray.Length == 1)
            {
                var chainStatus = chainStatusArray.First().Status;
                if (chainStatus == X509ChainStatusFlags.NoError || chainStatus == X509ChainStatusFlags.UntrustedRoot ||
                    (_rootCertificate == null && chainStatus == X509ChainStatusFlags.PartialChain))
                {
                    return true;
                }
                _logger.Error($"Invalid certificate chain status: {chainStatus}");
                return false;
            }
            _logger.Error($"Invalid certificate chain length: {chainStatusArray.Length}");
            return false;
        }
    }
}
