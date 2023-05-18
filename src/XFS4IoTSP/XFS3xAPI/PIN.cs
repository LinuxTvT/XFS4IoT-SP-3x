using BOOL = System.Boolean;
using DWORD = System.UInt32;
using HRESULT = System.Int32;
using LPDWORD = System.IntPtr;
using LPSTR = System.IntPtr;
using LPWORD = System.IntPtr;
using ULONG = System.UInt32;
using USHORT = System.UInt16;
using WORD = System.UInt16;
using LPWFSXDATA = System.IntPtr;
using System.Runtime.InteropServices;

namespace XFS3xAPI.PIN
{
    internal static class CLASS
    {
        #pragma warning disable format
        /* values of WFSPINCAPS.wClass */
        public const DWORD WFS_SERVICE_CLASS_PIN            = (4);
        public const DWORD WFS_SERVICE_CLASS_VERSION_PIN    = (0x2803); /* Version 3.40 */
        public const string WFS_SERVICE_CLASS_NAME_PIN      = "PIN";

        public const DWORD PIN_SERVICE_OFFSET               = (WFS_SERVICE_CLASS_PIN * 100);

        #pragma warning restore format
    }

    public static class CMD
    {
        #pragma warning disable format
        public const DWORD PIN_SERVICE_OFFSET = CLASS.PIN_SERVICE_OFFSET;
        /* PIN Info Commands */

        public const DWORD WFS_INF_PIN_STATUS                          =(PIN_SERVICE_OFFSET + 1);
        public const DWORD WFS_INF_PIN_CAPABILITIES                    =(PIN_SERVICE_OFFSET + 2);
        public const DWORD WFS_INF_PIN_KEY_DETAIL                      =(PIN_SERVICE_OFFSET + 4);
        public const DWORD WFS_INF_PIN_FUNCKEY_DETAIL                  =(PIN_SERVICE_OFFSET + 5);
        public const DWORD WFS_INF_PIN_HSM_TDATA                       =(PIN_SERVICE_OFFSET + 6);
        public const DWORD WFS_INF_PIN_KEY_DETAIL_EX                   =(PIN_SERVICE_OFFSET + 7);
        public const DWORD WFS_INF_PIN_SECUREKEY_DETAIL                =(PIN_SERVICE_OFFSET + 8);
        public const DWORD WFS_INF_PIN_QUERY_LOGICAL_HSM_DETAIL        =(PIN_SERVICE_OFFSET + 9);
        public const DWORD WFS_INF_PIN_QUERY_PCIPTS_DEVICE_ID          =(PIN_SERVICE_OFFSET + 10);
        public const DWORD WFS_INF_PIN_GET_LAYOUT                      =(PIN_SERVICE_OFFSET + 11);
        public const DWORD WFS_INF_PIN_KEY_DETAIL_340                  =(PIN_SERVICE_OFFSET + 12);

/* PIN Command Verbs */

        public const DWORD WFS_CMD_PIN_CRYPT                           =(PIN_SERVICE_OFFSET + 1);
        public const DWORD WFS_CMD_PIN_IMPORT_KEY                      =(PIN_SERVICE_OFFSET + 3);
        public const DWORD WFS_CMD_PIN_GET_PIN                         =(PIN_SERVICE_OFFSET + 5);
        public const DWORD WFS_CMD_PIN_GET_PINBLOCK                    =(PIN_SERVICE_OFFSET + 7);
        public const DWORD WFS_CMD_PIN_GET_DATA                        =(PIN_SERVICE_OFFSET + 8);
        public const DWORD WFS_CMD_PIN_INITIALIZATION                  =(PIN_SERVICE_OFFSET + 9);
        public const DWORD WFS_CMD_PIN_LOCAL_DES                       =(PIN_SERVICE_OFFSET + 10);
        public const DWORD WFS_CMD_PIN_LOCAL_EUROCHEQUE                =(PIN_SERVICE_OFFSET + 11);
        public const DWORD WFS_CMD_PIN_LOCAL_VISA                      =(PIN_SERVICE_OFFSET + 12);
        public const DWORD WFS_CMD_PIN_CREATE_OFFSET                   =(PIN_SERVICE_OFFSET + 13);
        public const DWORD WFS_CMD_PIN_DERIVE_KEY                      =(PIN_SERVICE_OFFSET + 14);
        public const DWORD WFS_CMD_PIN_PRESENT_IDC                     =(PIN_SERVICE_OFFSET + 15);
        public const DWORD WFS_CMD_PIN_LOCAL_BANKSYS                   =(PIN_SERVICE_OFFSET + 16);
        public const DWORD WFS_CMD_PIN_BANKSYS_IO                      =(PIN_SERVICE_OFFSET + 17);
        public const DWORD WFS_CMD_PIN_RESET                           =(PIN_SERVICE_OFFSET + 18);
        public const DWORD WFS_CMD_PIN_HSM_SET_TDATA                   =(PIN_SERVICE_OFFSET + 19);
        public const DWORD WFS_CMD_PIN_SECURE_MSG_SEND                 =(PIN_SERVICE_OFFSET + 20);
        public const DWORD WFS_CMD_PIN_SECURE_MSG_RECEIVE              =(PIN_SERVICE_OFFSET + 21);
        public const DWORD WFS_CMD_PIN_GET_JOURNAL                     =(PIN_SERVICE_OFFSET + 22);
        public const DWORD WFS_CMD_PIN_IMPORT_KEY_EX                   =(PIN_SERVICE_OFFSET + 23);
        public const DWORD WFS_CMD_PIN_ENC_IO                          =(PIN_SERVICE_OFFSET + 24);
        public const DWORD WFS_CMD_PIN_HSM_INIT                        =(PIN_SERVICE_OFFSET + 25);
        public const DWORD WFS_CMD_PIN_IMPORT_RSA_PUBLIC_KEY           =(PIN_SERVICE_OFFSET + 26);
        public const DWORD WFS_CMD_PIN_EXPORT_RSA_ISSUER_SIGNED_ITEM   =(PIN_SERVICE_OFFSET + 27);
        public const DWORD WFS_CMD_PIN_IMPORT_RSA_SIGNED_DES_KEY       =(PIN_SERVICE_OFFSET + 28);
        public const DWORD WFS_CMD_PIN_GENERATE_RSA_KEY_PAIR           =(PIN_SERVICE_OFFSET + 29);
        public const DWORD WFS_CMD_PIN_EXPORT_RSA_EPP_SIGNED_ITEM      =(PIN_SERVICE_OFFSET + 30);
        public const DWORD WFS_CMD_PIN_LOAD_CERTIFICATE                =(PIN_SERVICE_OFFSET + 31);
        public const DWORD WFS_CMD_PIN_GET_CERTIFICATE                 =(PIN_SERVICE_OFFSET + 32);
        public const DWORD WFS_CMD_PIN_REPLACE_CERTIFICATE             =(PIN_SERVICE_OFFSET + 33);
        public const DWORD WFS_CMD_PIN_START_KEY_EXCHANGE              =(PIN_SERVICE_OFFSET + 34);
        public const DWORD WFS_CMD_PIN_IMPORT_RSA_ENCIPHERED_PKCS7_KEY =(PIN_SERVICE_OFFSET + 35);
        public const DWORD WFS_CMD_PIN_EMV_IMPORT_PUBLIC_KEY           =(PIN_SERVICE_OFFSET + 36);
        public const DWORD WFS_CMD_PIN_DIGEST                          =(PIN_SERVICE_OFFSET + 37);
        public const DWORD WFS_CMD_PIN_SECUREKEY_ENTRY                 =(PIN_SERVICE_OFFSET + 38);
        public const DWORD WFS_CMD_PIN_GENERATE_KCV                    =(PIN_SERVICE_OFFSET + 39);
        public const DWORD WFS_CMD_PIN_SET_GUIDANCE_LIGHT              =(PIN_SERVICE_OFFSET + 41);
        public const DWORD WFS_CMD_PIN_MAINTAIN_PIN                    =(PIN_SERVICE_OFFSET + 42);
        public const DWORD WFS_CMD_PIN_KEYPRESS_BEEP                   =(PIN_SERVICE_OFFSET + 43);
        public const DWORD WFS_CMD_PIN_SET_PINBLOCK_DATA               =(PIN_SERVICE_OFFSET + 44);
        public const DWORD WFS_CMD_PIN_SET_LOGICAL_HSM                 =(PIN_SERVICE_OFFSET + 45);
        public const DWORD WFS_CMD_PIN_IMPORT_KEYBLOCK                 =(PIN_SERVICE_OFFSET + 46);
        public const DWORD WFS_CMD_PIN_POWER_SAVE_CONTROL              =(PIN_SERVICE_OFFSET + 47);
        public const DWORD WFS_CMD_PIN_LOAD_CERTIFICATE_EX             =(PIN_SERVICE_OFFSET + 48);
        public const DWORD WFS_CMD_PIN_IMPORT_RSA_ENCIPHERED_PKCS7_KEY_EX =(PIN_SERVICE_OFFSET + 49);
        public const DWORD WFS_CMD_PIN_DEFINE_LAYOUT                   =(PIN_SERVICE_OFFSET + 50);
        public const DWORD WFS_CMD_PIN_START_AUTHENTICATE              =(PIN_SERVICE_OFFSET + 51);
        public const DWORD WFS_CMD_PIN_AUTHENTICATE                    =(PIN_SERVICE_OFFSET + 52);
        public const DWORD WFS_CMD_PIN_GET_PINBLOCK_EX                 =(PIN_SERVICE_OFFSET + 53);
        public const DWORD WFS_CMD_PIN_SYNCHRONIZE_COMMAND             =(PIN_SERVICE_OFFSET + 54);
        public const DWORD WFS_CMD_PIN_CRYPT_340                       =(PIN_SERVICE_OFFSET + 55);
        public const DWORD WFS_CMD_PIN_IMPORT_KEY_340                  =(PIN_SERVICE_OFFSET + 56);
        public const DWORD WFS_CMD_PIN_GET_PINBLOCK_340                =(PIN_SERVICE_OFFSET + 57);
        #pragma warning restore format
    }

    public struct WFSPINKEYDETAIL
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpsKeyName;
        public WORD fwUse;
        public BOOL bLoaded;
        public LPWFSXDATA lpxKeyBlockHeader;
    }

    public static class FwUse
    {
        /* values of WFSPINKEYDETAIL.fwUse and values of WFSPINKEYDETAILEX.dwUse */
        #pragma warning disable format
        public const WORD WFS_PIN_USECRYPT                  = (0x0001);
        public const WORD WFS_PIN_USEFUNCTION               = (0x0002);
        public const WORD WFS_PIN_USEMACING                 = (0x0004);
        public const WORD WFS_PIN_USEKEYENCKEY              = (0x0020);
        public const WORD WFS_PIN_USENODUPLICATE            = (0x0040);
        public const WORD WFS_PIN_USESVENCKEY               = (0x0080);
        public const WORD WFS_PIN_USECONSTRUCT              = (0x0100);
        public const WORD WFS_PIN_USESECURECONSTRUCT        = (0x0200);
        public const WORD WFS_PIN_USEANSTR31MASTER          = (0x0400);
        public const WORD WFS_PIN_USERESTRICTEDKEYENCKEY    = (0x0800);
        public const WORD WFS_PIN_USEKEYDERKEY              = (0x1000);
        #pragma warning restore format

        public static string ToKeyUsage(WORD val) => val switch
        {
            WFS_PIN_USECRYPT => "D0",
            WFS_PIN_USEFUNCTION => "P0",
            WFS_PIN_USEMACING => "M0",
            WFS_PIN_USEKEYENCKEY => "K0",
            _ => throw new UnknowConstException(val, typeof(FwUse))
        };
    }


}