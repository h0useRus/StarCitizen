using System;
using System.Runtime.InteropServices;

namespace NSW.StarCitizen.Tools.Lib.Helpers
{
    #region WinTrustData struct field enums
    internal enum WinTrustDataUiChoice : uint
    {
        All = 1,
        None = 2,
        NoBad = 3,
        NoGood = 4
    }

    internal enum WinTrustDataRevocationChecks : uint
    {
        None = 0x00000000,
        WholeChain = 0x00000001
    }

    internal enum WinTrustDataChoice : uint
    {
        File = 1,
        Catalog = 2,
        Blob = 3,
        Signer = 4,
        Certificate = 5
    }

    internal enum WinTrustDataStateAction : uint
    {
        Ignore = 0x00000000,
        Verify = 0x00000001,
        Close = 0x00000002,
        AutoCache = 0x00000003,
        AutoCacheFlush = 0x00000004
    }

    [Flags]
    enum WinTrustDataProvFlags : uint
    {
        UseIe4TrustFlag = 0x00000001,
        NoIe4ChainFlag = 0x00000002,
        NoPolicyUsageFlag = 0x00000004,
        RevocationCheckNone = 0x00000010,
        RevocationCheckEndCert = 0x00000020,
        RevocationCheckChain = 0x00000040,
        RevocationCheckChainExcludeRoot = 0x00000080,
        SaferFlag = 0x00000100,                         // Used by software restriction policies. Should not be used.
        HashOnlyFlag = 0x00000200,
        UseDefaultOsVersionCheck = 0x00000400,
        LifetimeSigningFlag = 0x00000800,
        CacheOnlyUrlRetrieval = 0x00001000,             // affects CRL retrieval and AIA retrieval
        DisableMd2AndMd4 = 0x00002000                   // Win7 SP1+: Disallows use of MD2 or MD4 in the chain except for the root
    }

    internal enum WinTrustDataUiContext : uint
    {
        Execute = 0,
        Install = 1
    }

    [Flags]
    internal enum WinTrustSignatureSettingsFlags : uint
    {
        VerifySpecificFlag = 0x00000001,
        GetSecondarySigCountFlag = 0x00000002
    }

    #endregion

    #region WinTrust structures
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class WinTrustFileInfo : IDisposable
    {
        uint StructSize = (uint)Marshal.SizeOf(typeof(WinTrustFileInfo));
        IntPtr pszFilePath;                     // required, file name to be verified
        IntPtr hFile = IntPtr.Zero;             // optional, open handle to FilePath
        IntPtr pgKnownSubject = IntPtr.Zero;    // optional, subject type if it is known

        public WinTrustFileInfo(string filePath)
        {
            pszFilePath = Marshal.StringToCoTaskMemAuto(filePath);
        }
        public void Dispose()
        {
            if (pszFilePath != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(pszFilePath);
                pszFilePath = IntPtr.Zero;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class WinTrustSignatureSettings : IDisposable
    {
        uint StructSize = (uint)Marshal.SizeOf(typeof(WinTrustSignatureSettings));
        uint dwIndex = 0;
        WinTrustSignatureSettingsFlags dwFlags = WinTrustSignatureSettingsFlags.VerifySpecificFlag;
        uint cSecondarySigs = 0;
        uint dwVerifiedSigIndex = 0;
        IntPtr pCryptoPolicy = IntPtr.Zero;

        public void Dispose()
        {
            if (pCryptoPolicy != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(pCryptoPolicy);
                pCryptoPolicy = IntPtr.Zero;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class WinTrustData : IDisposable
    {
        uint StructSize = (uint)Marshal.SizeOf(typeof(WinTrustData));
        IntPtr PolicyCallbackData = IntPtr.Zero;
        IntPtr SIPClientData = IntPtr.Zero;
        // required: UI choice
        WinTrustDataUiChoice UIChoice = WinTrustDataUiChoice.None;
        // required: certificate revocation check options
        WinTrustDataRevocationChecks RevocationChecks = WinTrustDataRevocationChecks.None;
        // required: which structure is being passed in?
        WinTrustDataChoice UnionChoice = WinTrustDataChoice.File;
        // individual file
        IntPtr FileInfoPtr;
        WinTrustDataStateAction StateAction = WinTrustDataStateAction.Ignore;
        IntPtr StateData = IntPtr.Zero;
        string? URLReference = null;
        WinTrustDataProvFlags ProvFlags = WinTrustDataProvFlags.RevocationCheckNone | WinTrustDataProvFlags.DisableMd2AndMd4;
        WinTrustDataUiContext UIContext = WinTrustDataUiContext.Execute;
        IntPtr SignatureSettings = IntPtr.Zero;

        // constructor for silent WinTrustDataChoice.File check
        public WinTrustData(WinTrustFileInfo fileInfo)
        {
            var wtfiData = fileInfo;
            FileInfoPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(WinTrustFileInfo)));
            Marshal.StructureToPtr(wtfiData, FileInfoPtr, false);

            using var signatureSettings = new WinTrustSignatureSettings();
            SignatureSettings = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(WinTrustSignatureSettings)));
            Marshal.StructureToPtr(signatureSettings, SignatureSettings, false);
        }
        public void Dispose()
        {
            if (FileInfoPtr != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(FileInfoPtr);
                FileInfoPtr = IntPtr.Zero;
            }
            if (SignatureSettings != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(SignatureSettings);
                SignatureSettings = IntPtr.Zero;
            }
        }
    }
    #endregion

    internal enum WinVerifyTrustResult : uint
    {
        Success = 0,
        ProviderUnknown = 0x800b0001,               // Trust provider is not recognized on this system
        ActionUnknown = 0x800b0002,                 // Trust provider does not support the specified action
        SubjectFormUnknown = 0x800b0003,            // Trust provider does not support the form specified for the subject
        SubjectNotTrusted = 0x800b0004,             // Subject failed the specified verification action
        FileNotSigned = 0x800B0100,                 // TRUST_E_NOSIGNATURE - File was not signed
        SubjectExplicitlyDistrusted = 0x800B0111,   // Signer's certificate is in the Untrusted Publishers store
        SignatureOrFileCorrupt = 0x80096010,        // TRUST_E_BAD_DIGEST - file was probably corrupt
        SubjectCertExpired = 0x800B0101,            // CERT_E_EXPIRED - Signer's certificate was expired
        SubjectCertificateRevoked = 0x800B010C,     // CERT_E_REVOKED Subject's certificate was revoked
        UntrustedRoot = 0x800B0109,                 // CERT_E_UNTRUSTEDROOT - A certification chain processed correctly but terminated in a root certificate that is not trusted by the trust provider.
        CertChaining = 0x800B010A,                  // CERT_E_CHAINING - A certificate chain could not be built to a trusted root authority.
    }

    internal static class WinTrust
    {
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        // GUID of the action to perform
        private static readonly string WINTRUST_ACTION_GENERIC_VERIFY_V2 = "{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}";

        [DllImport("wintrust", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
        private static extern WinVerifyTrustResult WinVerifyTrust(
            [In] IntPtr hwnd,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID,
            [In] WinTrustData pWVTData
        );

        public static WinVerifyTrustResult VerifyEmbeddedSignature(string fileName)
        {
            using var wtfi = new WinTrustFileInfo(fileName);
            using var wtd = new WinTrustData(wtfi);
            var guidAction = new Guid(WINTRUST_ACTION_GENERIC_VERIFY_V2);
            return WinVerifyTrust(INVALID_HANDLE_VALUE, guidAction, wtd);
        }
    }
}