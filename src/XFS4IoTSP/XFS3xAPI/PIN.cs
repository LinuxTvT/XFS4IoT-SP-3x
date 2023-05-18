using System.Runtime.InteropServices;
using XFS4IoTFramework.Keyboard;
using BOOL = System.Boolean;
using DWORD = System.UInt32;
using LPPWFSPINKEY = System.IntPtr;
using LPWFSXDATA = System.IntPtr;
using ULONG = System.UInt32;
using USHORT = System.UInt16;
using WORD = System.UInt16;

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

    public static class EVENT
    {
        #pragma warning disable format
        public const DWORD PIN_SERVICE_OFFSET                          = CLASS.PIN_SERVICE_OFFSET;
        /* PIN Messages */

        public const DWORD WFS_EXEE_PIN_KEY                            = (PIN_SERVICE_OFFSET + 1);
        public const DWORD WFS_SRVE_PIN_INITIALIZED                    = (PIN_SERVICE_OFFSET + 2);
        public const DWORD WFS_SRVE_PIN_ILLEGAL_KEY_ACCESS             = (PIN_SERVICE_OFFSET + 3);
        public const DWORD WFS_SRVE_PIN_OPT_REQUIRED                   = (PIN_SERVICE_OFFSET + 4);
        public const DWORD WFS_SRVE_PIN_HSM_TDATA_CHANGED              = (PIN_SERVICE_OFFSET + 5);
        public const DWORD WFS_SRVE_PIN_CERTIFICATE_CHANGE             = (PIN_SERVICE_OFFSET + 6);
        public const DWORD WFS_SRVE_PIN_HSM_CHANGED                    = (PIN_SERVICE_OFFSET + 7);
        public const DWORD WFS_EXEE_PIN_ENTERDATA                      = (PIN_SERVICE_OFFSET + 8);
        public const DWORD WFS_SRVE_PIN_DEVICEPOSITION                 = (PIN_SERVICE_OFFSET + 9);
        public const DWORD WFS_SRVE_PIN_POWER_SAVE_CHANGE              = (PIN_SERVICE_OFFSET + 10);
        public const DWORD WFS_EXEE_PIN_LAYOUT                         = (PIN_SERVICE_OFFSET + 11);
        public const DWORD WFS_EXEE_PIN_DUKPT_KSN                      = (PIN_SERVICE_OFFSET + 12);
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

    [StructLayout(LayoutKind.Explicit)]
    public struct WFSPINGETDATA
    {
        [FieldOffset(0)] public USHORT usMaxLen;
        [FieldOffset(2)] public BOOL bAutoEnd;
        [FieldOffset(6)] public ULONG ulActiveFDKs;
        [FieldOffset(10)] public ULONG ulActiveKeys;
        [FieldOffset(14)] public ULONG ulTerminateFDKs;
        [FieldOffset(18)] public ULONG ulTerminateKeys;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct WFSPINDATA
    {
        [FieldOffset(0)] public USHORT usKeys;
        [FieldOffset(2)] public LPPWFSPINKEY lpPinKeys;
        [FieldOffset(6)] public WORD wCompletion;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct WFSPINKEY
    {
        [FieldOffset(0)] public WORD wCompletion;
        [FieldOffset(2)] public ULONG ulDigit;
    }

    public static class WCompletion
    {
        #pragma warning disable format
        /* values of WFSPINENTRY.wCompletion */

        public const WORD WFS_PIN_COMPAUTO      = (0);
        public const WORD WFS_PIN_COMPENTER     = (1);
        public const WORD WFS_PIN_COMPCANCEL    = (2);
        public const WORD WFS_PIN_COMPCONTINUE  = (6);
        public const WORD WFS_PIN_COMPCLEAR     = (7);
        public const WORD WFS_PIN_COMPBACKSPACE = (8);
        public const WORD WFS_PIN_COMPFDK       = (9);
        public const WORD WFS_PIN_COMPHELP      = (10);
        public const WORD WFS_PIN_COMPFK        = (11);
        public const WORD WFS_PIN_COMPCONTFDK   = (12);
#pragma warning restore format

        public static EntryCompletionEnum ToEnum(WORD val) => val switch
        {
            WFS_PIN_COMPAUTO => EntryCompletionEnum.Auto,
            WFS_PIN_COMPENTER => EntryCompletionEnum.Enter,
            WFS_PIN_COMPCANCEL => EntryCompletionEnum.Cancel,
            WFS_PIN_COMPCONTINUE => EntryCompletionEnum.Continue,
            WFS_PIN_COMPCLEAR => EntryCompletionEnum.Clear,
            WFS_PIN_COMPBACKSPACE => EntryCompletionEnum.Backspace,
            WFS_PIN_COMPFDK => EntryCompletionEnum.FDK,
            WFS_PIN_COMPHELP => EntryCompletionEnum.Help,
            WFS_PIN_COMPFK => EntryCompletionEnum.FK,
            WFS_PIN_COMPCONTFDK => EntryCompletionEnum.ContinueFDK,
            _ => throw new UnknowConstException(val, typeof(WCompletion))
        };
    }

    public static class UlKeyMask
    {
        #pragma warning disable format
        /* values of WFSPINFUNCKEYDETAIL.ulFuncMask */
        public const ULONG WFS_PIN_FK_0                                = (0x00000001);
        public const ULONG WFS_PIN_FK_1                                = (0x00000002);
        public const ULONG WFS_PIN_FK_2                                = (0x00000004);
        public const ULONG WFS_PIN_FK_3                                = (0x00000008);
        public const ULONG WFS_PIN_FK_4                                = (0x00000010);
        public const ULONG WFS_PIN_FK_5                                = (0x00000020);
        public const ULONG WFS_PIN_FK_6                                = (0x00000040);
        public const ULONG WFS_PIN_FK_7                                = (0x00000080);
        public const ULONG WFS_PIN_FK_8                                = (0x00000100);
        public const ULONG WFS_PIN_FK_9                                = (0x00000200);
        public const ULONG WFS_PIN_FK_ENTER                            = (0x00000400);
        public const ULONG WFS_PIN_FK_CANCEL                           = (0x00000800);
        public const ULONG WFS_PIN_FK_CLEAR                            = (0x00001000);
        public const ULONG WFS_PIN_FK_BACKSPACE                        = (0x00002000);
        public const ULONG WFS_PIN_FK_HELP                             = (0x00004000);
        public const ULONG WFS_PIN_FK_DECPOINT                         = (0x00008000);
        public const ULONG WFS_PIN_FK_00                               = (0x00010000);
        public const ULONG WFS_PIN_FK_000                              = (0x00020000);
        public const ULONG WFS_PIN_FK_RES1                             = (0x00040000);
        public const ULONG WFS_PIN_FK_RES2                             = (0x00080000);
        public const ULONG WFS_PIN_FK_RES3                             = (0x00100000);
        public const ULONG WFS_PIN_FK_RES4                             = (0x00200000);
        public const ULONG WFS_PIN_FK_RES5                             = (0x00400000);
        public const ULONG WFS_PIN_FK_RES6                             = (0x00800000);
        public const ULONG WFS_PIN_FK_RES7                             = (0x01000000);
        public const ULONG WFS_PIN_FK_RES8                             = (0x02000000);
        public const ULONG WFS_PIN_FK_OEM1                             = (0x04000000);
        public const ULONG WFS_PIN_FK_OEM2                             = (0x08000000);
        public const ULONG WFS_PIN_FK_OEM3                             = (0x10000000);
        public const ULONG WFS_PIN_FK_OEM4                             = (0x20000000);
        public const ULONG WFS_PIN_FK_OEM5                             = (0x40000000);
        public const ULONG WFS_PIN_FK_OEM6                             = (0x80000000);

        /* values of WFSPINFDK.ulFDK */

       public const ULONG WFS_PIN_FK_FDK01                            = (0x00000001);
       public const ULONG WFS_PIN_FK_FDK02                            = (0x00000002);
       public const ULONG WFS_PIN_FK_FDK03                            = (0x00000004);
       public const ULONG WFS_PIN_FK_FDK04                            = (0x00000008);
       public const ULONG WFS_PIN_FK_FDK05                            = (0x00000010);
       public const ULONG WFS_PIN_FK_FDK06                            = (0x00000020);
       public const ULONG WFS_PIN_FK_FDK07                            = (0x00000040);
       public const ULONG WFS_PIN_FK_FDK08                            = (0x00000080);
       public const ULONG WFS_PIN_FK_FDK09                            = (0x00000100);
       public const ULONG WFS_PIN_FK_FDK10                            = (0x00000200);
       public const ULONG WFS_PIN_FK_FDK11                            = (0x00000400);
       public const ULONG WFS_PIN_FK_FDK12                            = (0x00000800);
       public const ULONG WFS_PIN_FK_FDK13                            = (0x00001000);
       public const ULONG WFS_PIN_FK_FDK14                            = (0x00002000);
       public const ULONG WFS_PIN_FK_FDK15                            = (0x00004000);
       public const ULONG WFS_PIN_FK_FDK16                            = (0x00008000);
       public const ULONG WFS_PIN_FK_FDK17                            = (0x00010000);
       public const ULONG WFS_PIN_FK_FDK18                            = (0x00020000);
       public const ULONG WFS_PIN_FK_FDK19                            = (0x00040000);
       public const ULONG WFS_PIN_FK_FDK20                            = (0x00080000);
       public const ULONG WFS_PIN_FK_FDK21                            = (0x00100000);
       public const ULONG WFS_PIN_FK_FDK22                            = (0x00200000);
       public const ULONG WFS_PIN_FK_FDK23                            = (0x00400000);
       public const ULONG WFS_PIN_FK_FDK24                            = (0x00800000);
       public const ULONG WFS_PIN_FK_FDK25                            = (0x01000000);
       public const ULONG WFS_PIN_FK_FDK26                            = (0x02000000);
       public const ULONG WFS_PIN_FK_FDK27                            = (0x04000000);
       public const ULONG WFS_PIN_FK_FDK28                            = (0x08000000);
       public const ULONG WFS_PIN_FK_FDK29                            = (0x10000000);
       public const ULONG WFS_PIN_FK_FDK30                            = (0x20000000);
       public const ULONG WFS_PIN_FK_FDK31                            = (0x40000000);
       public const ULONG WFS_PIN_FK_FDK32                            = (0x80000000);
#pragma warning restore format

        public static ULONG FromString(string val) => val switch
        {
            #pragma warning disable format
            "zero"      => WFS_PIN_FK_0,
            "one"       => WFS_PIN_FK_1,
            "two"       => WFS_PIN_FK_2,
            "three"     => WFS_PIN_FK_3,
            "four"      => WFS_PIN_FK_4,
            "five"      => WFS_PIN_FK_5,
            "six"       => WFS_PIN_FK_6,
            "seven"     => WFS_PIN_FK_7,
            "eight"     => WFS_PIN_FK_8,
            "nine"      => WFS_PIN_FK_9,
            "enter"     => WFS_PIN_FK_ENTER,
            "cancel"    => WFS_PIN_FK_CANCEL,
            "clear"     => WFS_PIN_FK_CLEAR,
            "backspace" => WFS_PIN_FK_BACKSPACE,
            "help"      => WFS_PIN_FK_HELP,
            "decPoint"  => WFS_PIN_FK_DECPOINT,
            "doubleZero"=> WFS_PIN_FK_00,
            "tripleZero"=> WFS_PIN_FK_000,
            "fd01" => WFS_PIN_FK_FDK01,
            "fd02" => WFS_PIN_FK_FDK02,
            "fd03" => WFS_PIN_FK_FDK03,
            "fd04" => WFS_PIN_FK_FDK04,
            "fd05" => WFS_PIN_FK_FDK05,
            "fd06" => WFS_PIN_FK_FDK06,
            "fd07" => WFS_PIN_FK_FDK07,
            "fd08" => WFS_PIN_FK_FDK08,
            "fd09" => WFS_PIN_FK_FDK09,
            "fd10" => WFS_PIN_FK_FDK10,
            "fd11" => WFS_PIN_FK_FDK11,
            "fd12" => WFS_PIN_FK_FDK12,
            "fd13" => WFS_PIN_FK_FDK13,
            "fd14" => WFS_PIN_FK_FDK14,
            "fd15" => WFS_PIN_FK_FDK15,
            "fd16" => WFS_PIN_FK_FDK16,
            "fd17" => WFS_PIN_FK_FDK17,
            "fd18" => WFS_PIN_FK_FDK18,
            "fd19" => WFS_PIN_FK_FDK19,
            "fd20" => WFS_PIN_FK_FDK20,
            "fd21" => WFS_PIN_FK_FDK21,
            "fd22" => WFS_PIN_FK_FDK22,
            "fd23" => WFS_PIN_FK_FDK23,
            "fd24" => WFS_PIN_FK_FDK24,
            "fd25" => WFS_PIN_FK_FDK25,
            "fd26" => WFS_PIN_FK_FDK26,
            "fd27" => WFS_PIN_FK_FDK27,
            "fd28" => WFS_PIN_FK_FDK28,
            "fd29" => WFS_PIN_FK_FDK29,
            "fd30" => WFS_PIN_FK_FDK30,
            "fd31" => WFS_PIN_FK_FDK31,
            "fd32" => WFS_PIN_FK_FDK32,
            _ => throw new InternalException($"Unknow Key [{val}]")
#pragma warning restore format
        };

        public static string ToString(ULONG val) => val switch
        {
            #pragma warning disable format
            WFS_PIN_FK_0 => "zero",
            WFS_PIN_FK_1 => "one",
            WFS_PIN_FK_2 => "two",
            WFS_PIN_FK_3 => "three",
            WFS_PIN_FK_4 => "four",
            WFS_PIN_FK_5 => "five",
            WFS_PIN_FK_6 => "six",
            WFS_PIN_FK_7 => "seven",
            WFS_PIN_FK_8 => "eight",
            WFS_PIN_FK_9 => "nine",
            WFS_PIN_FK_ENTER => "enter",
            WFS_PIN_FK_CANCEL => "cancel",
            WFS_PIN_FK_CLEAR => "clear",
            WFS_PIN_FK_BACKSPACE => "backspace",
            WFS_PIN_FK_HELP => "help",
            WFS_PIN_FK_DECPOINT => "decPoint",
            WFS_PIN_FK_00 => "doubleZero",
            WFS_PIN_FK_000 => "tripleZero",
            /*
            WFS_PIN_FK_FDK01 => "fd01",
            WFS_PIN_FK_FDK02 => "fd02",
            WFS_PIN_FK_FDK03 => "fd03",
            WFS_PIN_FK_FDK04 => "fd04",
            WFS_PIN_FK_FDK05 => "fd05",
            WFS_PIN_FK_FDK06 => "fd06",
            WFS_PIN_FK_FDK07 => "fd07",
            WFS_PIN_FK_FDK08 => "fd08",
            WFS_PIN_FK_FDK09 => "fd09",
            WFS_PIN_FK_FDK10 => "fd10",
            WFS_PIN_FK_FDK11 => "fd11",
            WFS_PIN_FK_FDK12 => "fd12",
            WFS_PIN_FK_FDK13 => "fd13",
            WFS_PIN_FK_FDK14 => "fd14",
            WFS_PIN_FK_FDK15 => "fd15",
            WFS_PIN_FK_FDK16 => "fd16",
            WFS_PIN_FK_FDK17 => "fd17",
            WFS_PIN_FK_FDK18 => "fd18",
            WFS_PIN_FK_FDK19 => "fd19",
            WFS_PIN_FK_FDK20 => "fd20",
            WFS_PIN_FK_FDK21 => "fd21",
            WFS_PIN_FK_FDK22 => "fd22",
            WFS_PIN_FK_FDK23 => "fd23",
            WFS_PIN_FK_FDK24 => "fd24",
            WFS_PIN_FK_FDK25 => "fd25",
            WFS_PIN_FK_FDK26 => "fd26",
            WFS_PIN_FK_FDK27 => "fd27",
            WFS_PIN_FK_FDK28 => "fd28",
            WFS_PIN_FK_FDK29 => "fd29",
            WFS_PIN_FK_FDK30 => "fd30",
            WFS_PIN_FK_FDK31 => "fd31",
            WFS_PIN_FK_FDK32 => "fd32",
            */
            _ => throw new InternalException($"Unknow Key [{val}]")
#pragma warning restore format
        };

        public static bool IsFDK(this ActiveKeyClass key)
        {
            return (key.KeyName.StartsWith("fd"));
        }

        public static void ParseActiveKeys(
                                List<ActiveKeyClass> keys,
                                out ULONG activeFDKs,
                                out ULONG activeKeys,
                                out ULONG terminateFDKs,
                                out ULONG terminateKeys)
        {
            activeFDKs = 0x00000000;
            activeKeys = 0x00000000;
            terminateFDKs = 0x00000000;
            terminateKeys = 0x00000000;
            foreach (var key in keys)
            {
                ULONG keyMask = FromString(key.KeyName);
                if (key.IsFDK())
                {
                    activeFDKs |= keyMask;
                    if (key.Terminate)
                    {
                        terminateFDKs |= keyMask;
                    }
                }
                else
                {
                    activeKeys |= keyMask;
                    if (key.Terminate)
                    {
                        terminateKeys |= keyMask;
                    }
                }
            }
        }

    }

}