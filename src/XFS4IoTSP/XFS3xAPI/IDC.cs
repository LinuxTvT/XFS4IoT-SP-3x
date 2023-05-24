using System.Runtime.InteropServices;
using System.Text;
using XFS4IoT.CardReader.Completions;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using static XFS4IoTFramework.CardReader.MovePosition;
using static XFS4IoTFramework.CardReader.ReadCardRequest;
using static XFS4IoTFramework.CardReader.ReadCardResult;
using BOOL = System.Boolean;
using DWORD = System.UInt32;
using HRESULT = System.Int32;
using LPDWORD = System.IntPtr;
using LPSTR = System.IntPtr;
using LPVOID = System.IntPtr;
using LPWORD = System.IntPtr;
using ULONG = System.UInt32;
using USHORT = System.UInt16;
using WORD = System.UInt16;

namespace XFS3xAPI.IDC
{
    public static class IDCExtension
    {
        public static WFSIDCSTATUS_300 ReadIDCStatus300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSIDCSTATUS_300>(wfsResult.lpBuffer);
        public static WFSIDCCAPS_300 ReadIDCCaps300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSIDCCAPS_300>(wfsResult.lpBuffer);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <code>
    /// /* values of WFSIDCCAPS.wClass */
    ///
    /// #define     WFS_SERVICE_CLASS_IDC               (2)
    /// #define     WFS_SERVICE_CLASS_NAME_IDC          "IDC"
    /// #define     WFS_SERVICE_CLASS_VERSION_IDC       (0x2803) /* Version 3.40 */
    ///
    /// #define     IDC_SERVICE_OFFSET                  (WFS_SERVICE_CLASS_IDC * 100)
    /// </code>
    /// </remarks>
    internal static class CLASS
    {
        #pragma warning disable format
        public const DWORD  WFS_SERVICE_CLASS_IDC           = 2;
        public const string WFS_SERVICE_CLASS_NAME_IDC      = "IDC";
        public const DWORD  WFS_SERVICE_CLASS_VERSION_IDC   = 2;
        public const DWORD  IDC_SERVICE_OFFSET              = WFS_SERVICE_CLASS_IDC * 100;
        #pragma warning restore format
    }

    public static class DEF
    {
        #pragma warning disable format

        /* Size and max index of dwGuidLights array */
        public const int  WFS_IDC_GUIDLIGHTS_SIZE             = (32);
        public const int  WFS_IDC_GUIDLIGHTS_MAX              = (WFS_IDC_GUIDLIGHTS_SIZE - 1);
        #pragma warning restore format
    }

    public static class EVENT
    {
        #pragma warning disable format
        /* IDC Messages */
 
        public const DWORD IDC_SERVICE_OFFSET               = CLASS.IDC_SERVICE_OFFSET;

        public const DWORD WFS_EXEE_IDC_INVALIDTRACKDATA    = (IDC_SERVICE_OFFSET + 1);
        public const DWORD WFS_EXEE_IDC_MEDIAINSERTED       = (IDC_SERVICE_OFFSET + 3);
        public const DWORD WFS_SRVE_IDC_MEDIAREMOVED        = (IDC_SERVICE_OFFSET + 4);
        public const DWORD WFS_SRVE_IDC_CARDACTION          = (IDC_SERVICE_OFFSET + 5);
        public const DWORD WFS_USRE_IDC_RETAINBINTHRESHOLD  = (IDC_SERVICE_OFFSET + 6);
        public const DWORD WFS_EXEE_IDC_INVALIDMEDIA        = (IDC_SERVICE_OFFSET + 7);
        public const DWORD WFS_EXEE_IDC_MEDIARETAINED       = (IDC_SERVICE_OFFSET + 8);
        public const DWORD WFS_SRVE_IDC_MEDIADETECTED       = (IDC_SERVICE_OFFSET + 9);
        public const DWORD WFS_SRVE_IDC_RETAINBININSERTED   = (IDC_SERVICE_OFFSET + 10);
        public const DWORD WFS_SRVE_IDC_RETAINBINREMOVED    = (IDC_SERVICE_OFFSET + 11);
        public const DWORD WFS_EXEE_IDC_INSERTCARD          = (IDC_SERVICE_OFFSET + 12);
        public const DWORD WFS_SRVE_IDC_DEVICEPOSITION      = (IDC_SERVICE_OFFSET + 13);
        public const DWORD WFS_SRVE_IDC_POWER_SAVE_CHANGE   = (IDC_SERVICE_OFFSET + 14);
        #pragma warning restore format

        public static string ToString(DWORD val) => val switch
        {
            WFS_EXEE_IDC_INVALIDTRACKDATA => "WFS_EXEE_IDC_INVALIDTRACKDATA",
            WFS_EXEE_IDC_MEDIAINSERTED => "WFS_EXEE_IDC_MEDIAINSERTED",
            WFS_SRVE_IDC_MEDIAREMOVED => "WFS_SRVE_IDC_MEDIAREMOVED",
            WFS_SRVE_IDC_CARDACTION => "WFS_SRVE_IDC_CARDACTION",
            WFS_USRE_IDC_RETAINBINTHRESHOLD => "WFS_USRE_IDC_RETAINBINTHRESHOLD",
            WFS_EXEE_IDC_INVALIDMEDIA => "WFS_EXEE_IDC_INVALIDMEDIA",
            WFS_EXEE_IDC_MEDIARETAINED => "WFS_EXEE_IDC_MEDIARETAINED",
            WFS_SRVE_IDC_MEDIADETECTED => "WFS_SRVE_IDC_MEDIADETECTED",
            WFS_SRVE_IDC_RETAINBININSERTED => "WFS_SRVE_IDC_RETAINBININSERTED",
            WFS_SRVE_IDC_RETAINBINREMOVED => "WFS_SRVE_IDC_RETAINBINREMOVED",
            WFS_EXEE_IDC_INSERTCARD => "WFS_EXEE_IDC_INSERTCARD",
            WFS_SRVE_IDC_DEVICEPOSITION => "WFS_SRVE_IDC_DEVICEPOSITION",
            WFS_SRVE_IDC_POWER_SAVE_CHANGE => "WFS_SRVE_IDC_POWER_SAVE_CHANGE",
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_COMMAND, $"Unknown IDC Event [{val}]")
        };
    }

    /// <summary>
    /// Execute command code
    /// </summary>
    public static class CMD
    {
        #pragma warning disable format
        public const DWORD IDC_SERVICE_OFFSET = CLASS.IDC_SERVICE_OFFSET;
        
        /* IDC Info Commands */

        public const DWORD WFS_INF_IDC_STATUS                  =(IDC_SERVICE_OFFSET + 1);
        public const DWORD WFS_INF_IDC_CAPABILITIES            =(IDC_SERVICE_OFFSET + 2);
        public const DWORD WFS_INF_IDC_FORM_LIST               =(IDC_SERVICE_OFFSET + 3);
        public const DWORD WFS_INF_IDC_QUERY_FORM              =(IDC_SERVICE_OFFSET + 4);
        public const DWORD WFS_INF_IDC_QUERY_IFM_IDENTIFIER    =(IDC_SERVICE_OFFSET + 5);
        public const DWORD WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS =(IDC_SERVICE_OFFSET + 6);

        /* IDC Execute Commands */

        public const DWORD WFS_CMD_IDC_READ_TRACK              =(IDC_SERVICE_OFFSET + 1);
        public const DWORD WFS_CMD_IDC_WRITE_TRACK             =(IDC_SERVICE_OFFSET + 2);
        public const DWORD WFS_CMD_IDC_EJECT_CARD              =(IDC_SERVICE_OFFSET + 3);
        public const DWORD WFS_CMD_IDC_RETAIN_CARD             =(IDC_SERVICE_OFFSET + 4);
        public const DWORD WFS_CMD_IDC_RESET_COUNT             =(IDC_SERVICE_OFFSET + 5);
        public const DWORD WFS_CMD_IDC_SETKEY                  =(IDC_SERVICE_OFFSET + 6);
        public const DWORD WFS_CMD_IDC_READ_RAW_DATA           =(IDC_SERVICE_OFFSET + 7);
        public const DWORD WFS_CMD_IDC_WRITE_RAW_DATA          =(IDC_SERVICE_OFFSET + 8);
        public const DWORD WFS_CMD_IDC_CHIP_IO                 =(IDC_SERVICE_OFFSET + 9);
        public const DWORD WFS_CMD_IDC_RESET                   =(IDC_SERVICE_OFFSET + 10);
        public const DWORD WFS_CMD_IDC_CHIP_POWER              =(IDC_SERVICE_OFFSET + 11);
        public const DWORD WFS_CMD_IDC_PARSE_DATA              =(IDC_SERVICE_OFFSET + 12);
        public const DWORD WFS_CMD_IDC_SET_GUIDANCE_LIGHT      =(IDC_SERVICE_OFFSET + 13);
        public const DWORD WFS_CMD_IDC_POWER_SAVE_CONTROL      =(IDC_SERVICE_OFFSET + 14);
        public const DWORD WFS_CMD_IDC_PARK_CARD               =(IDC_SERVICE_OFFSET + 15);
        public const DWORD WFS_CMD_IDC_EMVCLESS_CONFIGURE      =(IDC_SERVICE_OFFSET + 16);
        public const DWORD WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION =(IDC_SERVICE_OFFSET + 17);
        public const DWORD WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE   =(IDC_SERVICE_OFFSET + 18);
        public const DWORD WFS_CMD_IDC_SYNCHRONIZE_COMMAND     =(IDC_SERVICE_OFFSET + 19);
        #pragma warning restore format

        public static string ToInfoCommandString(DWORD cmd) => cmd switch
        {
            WFS_INF_IDC_STATUS => "WFS_INF_IDC_STATUS",
            WFS_INF_IDC_CAPABILITIES => "WFS_INF_IDC_CAPABILITIES",
            WFS_INF_IDC_FORM_LIST => "WFS_INF_IDC_FORM_LIST",
            WFS_INF_IDC_QUERY_FORM => "WFS_INF_IDC_QUERY_FORM",
            WFS_INF_IDC_QUERY_IFM_IDENTIFIER => "WFS_INF_IDC_QUERY_IFM_IDENTIFIER",
            WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS => "WFS_INF_IDC_EMVCLESS_QUERY_APPLICATIONS",
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_COMMAND, $"Unknown IDC Info command [{cmd}]")
        };

        public static string ToExecuteCommandString(DWORD cmd) => cmd switch
        {
            WFS_CMD_IDC_READ_TRACK => "WFS_CMD_IDC_READ_TRACK",
            WFS_CMD_IDC_WRITE_TRACK => "WFS_CMD_IDC_WRITE_TRACK",
            WFS_CMD_IDC_EJECT_CARD => "WFS_CMD_IDC_EJECT_CARD",
            WFS_CMD_IDC_RETAIN_CARD => "WFS_CMD_IDC_RETAIN_CARD",
            WFS_CMD_IDC_RESET_COUNT => "WFS_CMD_IDC_RESET_COUNT",
            WFS_CMD_IDC_SETKEY => "WFS_CMD_IDC_SETKEY",
            WFS_CMD_IDC_READ_RAW_DATA => "WFS_CMD_IDC_READ_RAW_DATA",
            WFS_CMD_IDC_WRITE_RAW_DATA => "WFS_CMD_IDC_WRITE_RAW_DATA",
            WFS_CMD_IDC_CHIP_IO => "WFS_CMD_IDC_CHIP_IO",
            WFS_CMD_IDC_RESET => "WFS_CMD_IDC_RESET",
            WFS_CMD_IDC_CHIP_POWER => "WFS_CMD_IDC_CHIP_POWER",
            WFS_CMD_IDC_PARSE_DATA => "WFS_CMD_IDC_PARSE_DATA",
            WFS_CMD_IDC_SET_GUIDANCE_LIGHT => "WFS_CMD_IDC_SET_GUIDANCE_LIGHT",
            WFS_CMD_IDC_POWER_SAVE_CONTROL => "WFS_CMD_IDC_POWER_SAVE_CONTROL",
            WFS_CMD_IDC_PARK_CARD => "WFS_CMD_IDC_PARK_CARD",
            WFS_CMD_IDC_EMVCLESS_CONFIGURE => "WFS_CMD_IDC_EMVCLESS_CONFIGURE",
            WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION => "WFS_CMD_IDC_EMVCLESS_PERFORM_TRANSACTION",
            WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE => "WFS_CMD_IDC_EMVCLESS_ISSUERUPDATE",
            WFS_CMD_IDC_SYNCHRONIZE_COMMAND => "WFS_CMD_IDC_SYNCHRONIZE_COMMAND",
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_COMMAND, $"Unknown IDC Execute command [{cmd}]")
        };

        public static DWORD FromMoveCardRequest(MoveCardRequest moveCardRequest) => (moveCardRequest.From.Position, moveCardRequest.To.Position) switch
        {
            (MovePositionEnum.Transport, MovePositionEnum.Exit) => WFS_CMD_IDC_EJECT_CARD,
            (_, MovePositionEnum.Storage) => WFS_CMD_IDC_RETAIN_CARD,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_COMMAND, $"Can not detect CMD for [{moveCardRequest}]")
        };
    }

    public static class ERROR
    {
        #pragma warning disable format
        public const int IDC_SERVICE_OFFSET                       = (int)CLASS.IDC_SERVICE_OFFSET;
        /* WOSA/XFS IDC Errors */

        public const HRESULT WFS_ERR_IDC_MEDIAJAM                   =(-(IDC_SERVICE_OFFSET + 0));
        public const HRESULT WFS_ERR_IDC_NOMEDIA                    =(-(IDC_SERVICE_OFFSET + 1));
        public const HRESULT WFS_ERR_IDC_MEDIARETAINED              =(-(IDC_SERVICE_OFFSET + 2));
        public const HRESULT WFS_ERR_IDC_RETAINBINFULL              =(-(IDC_SERVICE_OFFSET + 3));
        public const HRESULT WFS_ERR_IDC_INVALIDDATA                =(-(IDC_SERVICE_OFFSET + 4));
        public const HRESULT WFS_ERR_IDC_INVALIDMEDIA               =(-(IDC_SERVICE_OFFSET + 5));
        public const HRESULT WFS_ERR_IDC_FORMNOTFOUND               =(-(IDC_SERVICE_OFFSET + 6));
        public const HRESULT WFS_ERR_IDC_FORMINVALID                =(-(IDC_SERVICE_OFFSET + 7));
        public const HRESULT WFS_ERR_IDC_DATASYNTAX                 =(-(IDC_SERVICE_OFFSET + 8));
        public const HRESULT WFS_ERR_IDC_SHUTTERFAIL                =(-(IDC_SERVICE_OFFSET + 9));
        public const HRESULT WFS_ERR_IDC_SECURITYFAIL               =(-(IDC_SERVICE_OFFSET + 10));
        public const HRESULT WFS_ERR_IDC_PROTOCOLNOTSUPP            =(-(IDC_SERVICE_OFFSET + 11));
        public const HRESULT WFS_ERR_IDC_ATRNOTOBTAINED             =(-(IDC_SERVICE_OFFSET + 12));
        public const HRESULT WFS_ERR_IDC_INVALIDKEY                 =(-(IDC_SERVICE_OFFSET + 13));
        public const HRESULT WFS_ERR_IDC_WRITE_METHOD               =(-(IDC_SERVICE_OFFSET + 14));
        public const HRESULT WFS_ERR_IDC_CHIPPOWERNOTSUPP           =(-(IDC_SERVICE_OFFSET + 15));
        public const HRESULT WFS_ERR_IDC_CARDTOOSHORT               =(-(IDC_SERVICE_OFFSET + 16));
        public const HRESULT WFS_ERR_IDC_CARDTOOLONG                =(-(IDC_SERVICE_OFFSET + 17));
        public const HRESULT WFS_ERR_IDC_INVALID_PORT               =(-(IDC_SERVICE_OFFSET + 18));
        public const HRESULT WFS_ERR_IDC_POWERSAVETOOSHORT          =(-(IDC_SERVICE_OFFSET + 19));
        public const HRESULT WFS_ERR_IDC_POWERSAVEMEDIAPRESENT      =(-(IDC_SERVICE_OFFSET + 20));
        public const HRESULT WFS_ERR_IDC_CARDPRESENT                =(-(IDC_SERVICE_OFFSET + 21));
        public const HRESULT WFS_ERR_IDC_POSITIONINVALID            =(-(IDC_SERVICE_OFFSET + 22));
        public const HRESULT WFS_ERR_IDC_INVALIDTERMINALDATA        =(-(IDC_SERVICE_OFFSET + 23));
        public const HRESULT WFS_ERR_IDC_INVALIDAIDDATA             =(-(IDC_SERVICE_OFFSET + 24));
        public const HRESULT WFS_ERR_IDC_INVALIDKEYDATA             =(-(IDC_SERVICE_OFFSET + 25));
        public const HRESULT WFS_ERR_IDC_READERNOTCONFIGURED        =(-(IDC_SERVICE_OFFSET + 26));
        public const HRESULT WFS_ERR_IDC_TRANSACTIONNOTINITIATED    =(-(IDC_SERVICE_OFFSET + 27));
        public const HRESULT WFS_ERR_IDC_COMMANDUNSUPP              =(-(IDC_SERVICE_OFFSET + 28));
        public const HRESULT WFS_ERR_IDC_SYNCHRONIZEUNSUPP          =(-(IDC_SERVICE_OFFSET + 29));
        public const HRESULT WFS_ERR_IDC_CARDCOLLISION              =(-(IDC_SERVICE_OFFSET + 30));
        #pragma warning restore format

        public static string ToString(HRESULT result) => result switch
        {
            WFS_ERR_IDC_MEDIAJAM => "WFS_ERR_IDC_MEDIAJAM",
            WFS_ERR_IDC_NOMEDIA => "WFS_ERR_IDC_NOMEDIA",
            WFS_ERR_IDC_MEDIARETAINED => "WFS_ERR_IDC_MEDIARETAINED",
            WFS_ERR_IDC_RETAINBINFULL => "WFS_ERR_IDC_RETAINBINFULL",
            WFS_ERR_IDC_INVALIDDATA => "WFS_ERR_IDC_INVALIDDATA",
            WFS_ERR_IDC_INVALIDMEDIA => "WFS_ERR_IDC_INVALIDMEDIA",
            WFS_ERR_IDC_FORMNOTFOUND => "WFS_ERR_IDC_FORMNOTFOUND",
            WFS_ERR_IDC_FORMINVALID => "WFS_ERR_IDC_FORMINVALID",
            WFS_ERR_IDC_DATASYNTAX => "WFS_ERR_IDC_DATASYNTAX",
            WFS_ERR_IDC_SHUTTERFAIL => "WFS_ERR_IDC_SHUTTERFAIL",
            WFS_ERR_IDC_SECURITYFAIL => "WFS_ERR_IDC_SECURITYFAIL",
            WFS_ERR_IDC_PROTOCOLNOTSUPP => "WFS_ERR_IDC_PROTOCOLNOTSUPP",
            WFS_ERR_IDC_ATRNOTOBTAINED => "WFS_ERR_IDC_ATRNOTOBTAINED",
            WFS_ERR_IDC_INVALIDKEY => "WFS_ERR_IDC_INVALIDKEY",
            WFS_ERR_IDC_WRITE_METHOD => "WFS_ERR_IDC_WRITE_METHOD",
            WFS_ERR_IDC_CHIPPOWERNOTSUPP => "WFS_ERR_IDC_CHIPPOWERNOTSUPP",
            WFS_ERR_IDC_CARDTOOSHORT => "WFS_ERR_IDC_CARDTOOSHORT",
            WFS_ERR_IDC_CARDTOOLONG => "WFS_ERR_IDC_CARDTOOLONG",
            WFS_ERR_IDC_INVALID_PORT => "WFS_ERR_IDC_INVALID_PORT",
            WFS_ERR_IDC_POWERSAVETOOSHORT => "WFS_ERR_IDC_POWERSAVETOOSHORT",
            WFS_ERR_IDC_POWERSAVEMEDIAPRESENT => "WFS_ERR_IDC_POWERSAVEMEDIAPRESENT",
            WFS_ERR_IDC_CARDPRESENT => "WFS_ERR_IDC_CARDPRESENT",
            WFS_ERR_IDC_POSITIONINVALID => "WFS_ERR_IDC_POSITIONINVALID",
            WFS_ERR_IDC_INVALIDTERMINALDATA => "WFS_ERR_IDC_INVALIDTERMINALDATA",
            WFS_ERR_IDC_INVALIDAIDDATA => "WFS_ERR_IDC_INVALIDAIDDATA",
            WFS_ERR_IDC_INVALIDKEYDATA => "WFS_ERR_IDC_INVALIDKEYDATA",
            WFS_ERR_IDC_READERNOTCONFIGURED => "WFS_ERR_IDC_READERNOTCONFIGURED",
            WFS_ERR_IDC_TRANSACTIONNOTINITIATED => "WFS_ERR_IDC_TRANSACTIONNOTINITIATED",
            WFS_ERR_IDC_COMMANDUNSUPP => "WFS_ERR_IDC_COMMANDUNSUPP",
            WFS_ERR_IDC_SYNCHRONIZEUNSUPP => "WFS_ERR_IDC_SYNCHRONIZEUNSUPP",
            WFS_ERR_IDC_CARDCOLLISION => "WFS_ERR_IDC_CARDCOLLISION",
            _ => $"WFS_ERR_IDC_[{result}]"
        };

        public static MoveCompletion.PayloadData.ErrorCodeEnum ToMoveCompletionErrorCode(HRESULT result) => result switch
        {
            WFS_ERR_IDC_MEDIAJAM => MoveCompletion.PayloadData.ErrorCodeEnum.MediaJam,
            WFS_ERR_IDC_SHUTTERFAIL => MoveCompletion.PayloadData.ErrorCodeEnum.ShutterFail,
            WFS_ERR_IDC_NOMEDIA => MoveCompletion.PayloadData.ErrorCodeEnum.NoMedia,
            // No for MoveCompletion.PayloadData.ErrorCodeEnum.Occupied
            WFS_ERR_IDC_RETAINBINFULL => MoveCompletion.PayloadData.ErrorCodeEnum.Full,
            WFS_ERR_IDC_MEDIARETAINED => MoveCompletion.PayloadData.ErrorCodeEnum.MediaRetained,
            _ => throw new Xfs3xException(result, $"Can not convert ErrorCode")
        };
    }

    //[StructLayout(LayoutKind.Sequential)]
    [StructLayout(LayoutKind.Explicit)]
    public struct WFSIDCSTATUS_300
    {
        [FieldOffset(0)] public WORD fwDevice;
        [FieldOffset(2)] public WORD fwMedia;
        [FieldOffset(4)] public WORD fwRetainBin;
        [FieldOffset(6)] public WORD fwSecurity;
        [FieldOffset(8)] public USHORT usCards;
        [FieldOffset(10)] public WORD fwChipPower;
        [FieldOffset(12)] public LPSTR lpszExtra;

        public List<string> Extra => API.GetExtra(lpszExtra, 3);
    }

    //[StructLayout(LayoutKind.Sequential)]
    [StructLayout(LayoutKind.Explicit)]
    public struct WFSIDCSTATUS
    {
        [FieldOffset(0)] public WORD fwDevice;
        [FieldOffset(2)] public WORD fwMedia;
        [FieldOffset(4)] public WORD fwRetainBin;
        [FieldOffset(6)] public WORD fwSecurity;
        [FieldOffset(8)] public USHORT usCards;
        [FieldOffset(10)] public WORD fwChipPower;
        [FieldOffset(12)] public LPSTR lpszExtra;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_IDC_GUIDLIGHTS_SIZE)]
        [FieldOffset(16)] public byte[] dwGuidLights;
        [FieldOffset(48)] public WORD fwChipModule;
        [FieldOffset(50)] public WORD fwMagReadModule;
        [FieldOffset(52)] public WORD fwMagWriteModule;
        [FieldOffset(54)] public WORD fwFrontImageModule;
        [FieldOffset(56)] public WORD fwBackImageModule;
        [FieldOffset(58)] public WORD wDevicePosition;
        [FieldOffset(60)] public USHORT usPowerSaveRecoveryTime;
        [FieldOffset(62)] public LPWORD lpwParkingStationMedia;
        [FieldOffset(66)] public WORD wAntiFraudModule;
        /* values of WFSIDCSTATUS.fwDevice */
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct WFSIDCRETAINCARD
    {
        [FieldOffset(0)] public USHORT usCount;
        [FieldOffset(2)] public WORD fwPosition;

        public static WFSIDCRETAINCARD ReadStructure(ref WFSRESULT wfsResult) => ResultData.ReadStructure<WFSIDCRETAINCARD>(wfsResult.lpBuffer);
    }

    /// <summary>
    /// Card data structure
    /// </summary>
    /// <remarks>
    /// <code>
    /// typedef struct _wfs_idc_card_data
    /// {
    ///     WORD wDataSource;
    ///     WORD wStatus;
    ///     ULONG ulDataLength;
    ///     LPBYTE lpbData;
    ///     WORD fwWriteMethod;
    /// } WFSIDCCARDDATA, * LPWFSIDCCARDDATA;
    /// </code>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct WFSIDCCARDDATA
    {
        [FieldOffset(0)] public WORD wDataSource;
        [FieldOffset(2)] public WORD wStatus;
        [FieldOffset(4)] public ULONG ulDataLength;
        [FieldOffset(8)] public IntPtr lpbData;
        [FieldOffset(12)] public WORD fwWriteMethod;

        public string GetDataAsString()
        {
            return Encoding.UTF8.GetString(Data());
        }

        public byte[] Data()
        {
            byte[] bytes = new byte[ulDataLength];
            if (ulDataLength > 0)
            {
                Marshal.Copy(lpbData, bytes, 0, (int)ulDataLength);
            }
            return bytes;
        }

        public List<byte> DataAsList()
        {
            return Data().ToList();
        }

        public ReadCardResult.CardData.DataStatusEnum DataStatus => wStatus switch
        {
            _ => ReadCardResult.CardData.DataStatusEnum.Ok
        };

        public static void ReadCardData(ref WFSRESULT wfsResult, ref Dictionary<CardDataTypesEnum, CardData> outData)
        {
            if (wfsResult.lpBuffer != LPVOID.Zero)
            {
                for (int idx = 0; ; idx++)
                {
                    WFSIDCCARDDATA wfsCardData = ResultData.ReadStructure<WFSIDCCARDDATA>(wfsResult.lpBuffer, idx, out bool isNull);
                    if (isNull)
                    {
                        break;
                    }
                    else
                    {
                        outData.Add(WDataSource.ToEnum(wfsCardData.wDataSource), new CardData(wfsCardData.DataStatus, wfsCardData.DataAsList()));
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    public struct WFSIDCCAPS_300
    {
        public WORD wClass;
        public WORD fwType;
        public BOOL bCompound;
        public WORD fwReadTracks;
        public WORD fwWriteTracks;
        public WORD fwChipProtocols;
        public USHORT usCards;
        public WORD fwSecType;
        public WORD fwPowerOnOption;
        public WORD fwPowerOffOption;
        public BOOL bFluxSensorProgrammable;
        public BOOL bReadWriteAccessFollowingEject;
        public WORD fwWriteMode;
        public WORD fwChipPower;
        public LPSTR lpszExtra;

        public List<string> Extra => API.GetExtra(lpszExtra, 3);
    }

    public struct WFSIDCCAPS
    {
        public WORD wClass;
        public WORD fwType;
        public BOOL bCompound;
        public WORD fwReadTracks;
        public WORD fwWriteTracks;
        public WORD fwChipProtocols;
        public USHORT usCards;
        public WORD fwSecType;
        public WORD fwPowerOnOption;
        public WORD fwPowerOffOption;
        public BOOL bFluxSensorProgrammable;
        public BOOL bReadWriteAccessFollowingEject;
        public WORD fwWriteMode;
        public WORD fwChipPower;
        public LPSTR lpszExtra;
        public WORD fwDIPMode;
        public LPWORD lpwMemoryChipProtocols;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_IDC_GUIDLIGHTS_SIZE)]
        public DWORD[] dwGuidLights;
        public WORD fwEjectPosition;
        public BOOL bPowerSaveControl;
        public USHORT usParkingStations;
        public BOOL bAntiFraudModule;
        public LPDWORD lpdwSynchronizableCommands;
    }

    public static class FwDevice
    {
            #pragma warning disable format
            public const WORD WFS_IDC_DEVONLINE             = WFSDEVSTATUS.FwState.WFS_STAT_DEVONLINE;
            public const WORD WFS_IDC_DEVOFFLINE            = WFSDEVSTATUS.FwState.WFS_STAT_DEVOFFLINE;
            public const WORD WFS_IDC_DEVPOWEROFF           = WFSDEVSTATUS.FwState.WFS_STAT_DEVPOWEROFF;
            public const WORD WFS_IDC_DEVNODEVICE           = WFSDEVSTATUS.FwState.WFS_STAT_DEVNODEVICE;
            public const WORD WFS_IDC_DEVHWERROR            = WFSDEVSTATUS.FwState.WFS_STAT_DEVHWERROR;
            public const WORD WFS_IDC_DEVUSERERROR          = WFSDEVSTATUS.FwState.WFS_STAT_DEVUSERERROR;
            public const WORD WFS_IDC_DEVBUSY               = WFSDEVSTATUS.FwState.WFS_STAT_DEVBUSY;
            public const WORD WFS_IDC_DEVFRAUDATTEMPT       = WFSDEVSTATUS.FwState.WFS_STAT_DEVFRAUDATTEMPT;
            public const WORD WFS_IDC_DEVPOTENTIALFRAUD     = WFSDEVSTATUS.FwState.WFS_STAT_DEVPOTENTIALFRAUD;
            #pragma warning restore format
            public static string ToString(WORD state) => state switch
            {
                WFS_IDC_DEVONLINE => "WFS_IDC_DEVONLINE",
                WFS_IDC_DEVOFFLINE => "WFS_IDC_DEVOFFLINE",
                WFS_IDC_DEVPOWEROFF => "WFS_IDC_DEVPOWEROFF",
                WFS_IDC_DEVNODEVICE => "WFS_IDC_DEVNODEVICE",
                WFS_IDC_DEVHWERROR => "WFS_IDC_DEVHWERROR",
                WFS_IDC_DEVUSERERROR => "WFS_IDC_DEVUSERERROR",
                WFS_IDC_DEVBUSY => "WFS_IDC_DEVBUSY",
                WFS_IDC_DEVFRAUDATTEMPT => "WFS_IDC_DEVFRAUDATTEMPT",
                WFS_IDC_DEVPOTENTIALFRAUD => "WFS_IDC_DEVPOTENTIALFRAUD",
                _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown WFS_IDC_[{state}]")
            };

        public static CommonStatusClass.DeviceEnum ToEnum(WORD state) => WFSDEVSTATUS.FwState.ToDeviceEnum(state);
    }

    /* values of WFSIDCSTATUS.wDevicePosition
                                   WFSIDCDEVICEPOSITION.wPosition */
    public static class WDevicePosition
    {
        #pragma warning disable format
        public const WORD WFS_IDC_DEVICEINPOSITION            = (0);
        public const WORD WFS_IDC_DEVICENOTINPOSITION         = (1);
        public const WORD WFS_IDC_DEVICEPOSUNKNOWN            = (2);
        public const WORD WFS_IDC_DEVICEPOSNOTSUPP            = (3);
        #pragma warning restore format

        public static CommonStatusClass.PositionStatusEnum ToEnum(WORD pos) => pos switch
        {
            WFS_IDC_DEVICEINPOSITION => CommonStatusClass.PositionStatusEnum.InPosition,
            WFS_IDC_DEVICENOTINPOSITION => CommonStatusClass.PositionStatusEnum.NotInPosition,
            WFS_IDC_DEVICEPOSUNKNOWN => CommonStatusClass.PositionStatusEnum.Unknown,
            WFS_IDC_DEVICEPOSNOTSUPP => CommonStatusClass.PositionStatusEnum.Unknown,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown WFS_IDC_DEVICE[{pos}]")

        };
    }

    /* values of WFSIDCSTATUS.wAntiFraudModule */
    public static class WAntiFraudModule
    { 
        #pragma warning disable format
        public const WORD WFS_IDC_AFMNOTSUPP        = (0);
        public const WORD WFS_IDC_AFMOK             = (1);
        public const WORD WFS_IDC_AFMINOP           = (2);
        public const WORD WFS_IDC_AFMDEVICEDETECTED = (3);
        public const WORD WFS_IDC_AFMUNKNOWN        = (4);
        #pragma warning restore format
        public static CommonStatusClass.AntiFraudModuleEnum ToEnum(WORD val) => val switch
        {
            WFS_IDC_AFMNOTSUPP => CommonStatusClass.AntiFraudModuleEnum.NotSupported,
            WFS_IDC_AFMOK => CommonStatusClass.AntiFraudModuleEnum.Ok,
            WFS_IDC_AFMINOP => CommonStatusClass.AntiFraudModuleEnum.Inoperable,
            WFS_IDC_AFMDEVICEDETECTED => CommonStatusClass.AntiFraudModuleEnum.DeviceDetected,
            WFS_IDC_AFMUNKNOWN => CommonStatusClass.AntiFraudModuleEnum.Unknown,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown WFS_IDC_AFM[{val}]")
        };
    }

    public static class FwMedia
    {
        #pragma warning disable format
        public const WORD WFS_IDC_MEDIAPRESENT      = (1);
        public const WORD WFS_IDC_MEDIANOTPRESENT   = (2);
        public const WORD WFS_IDC_MEDIAJAMMED       = (3);
        public const WORD WFS_IDC_MEDIANOTSUPP      = (4);
        public const WORD WFS_IDC_MEDIAUNKNOWN      = (5);
        public const WORD WFS_IDC_MEDIAENTERING     = (6);
        public const WORD WFS_IDC_MEDIALATCHED      = (7);
        #pragma warning restore format
        public static CardReaderStatusClass.MediaEnum ToEnum(WORD val) => val switch
        {
            WFS_IDC_MEDIAPRESENT => CardReaderStatusClass.MediaEnum.Present,
            WFS_IDC_MEDIANOTPRESENT => CardReaderStatusClass.MediaEnum.NotPresent,
            WFS_IDC_MEDIAJAMMED => CardReaderStatusClass.MediaEnum.Jammed,
            WFS_IDC_MEDIANOTSUPP => CardReaderStatusClass.MediaEnum.NotSupported,
            WFS_IDC_MEDIAUNKNOWN => CardReaderStatusClass.MediaEnum.Unknown,
            WFS_IDC_MEDIAENTERING => CardReaderStatusClass.MediaEnum.Entering,
            WFS_IDC_MEDIALATCHED => CardReaderStatusClass.MediaEnum.Latched,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown WFS_IDC_AFM[{val}]")
        };
    }

    /* values of WFSIDCCAPS.fwType */
    public static class FwType
    {
        #pragma warning disable format
        public const WORD WFS_IDC_TYPEMOTOR                     = (1);
        public const WORD WFS_IDC_TYPESWIPE                     = (2);
        public const WORD WFS_IDC_TYPEDIP                       = (3);
        public const WORD WFS_IDC_TYPECONTACTLESS               = (4);
        public const WORD WFS_IDC_TYPELATCHEDDIP                = (5);
        public const WORD WFS_IDC_TYPEPERMANENT                 = (6);
        public const WORD WFS_IDC_TYPEINTELLIGENTCONTACTLESS    = (7);
        #pragma warning restore format
        public static CardReaderCapabilitiesClass.DeviceTypeEnum ToEnum(WORD val) => val switch
        {
            WFS_IDC_TYPEMOTOR => CardReaderCapabilitiesClass.DeviceTypeEnum.Motor,
            WFS_IDC_TYPESWIPE => CardReaderCapabilitiesClass.DeviceTypeEnum.Swipe,
            WFS_IDC_TYPEDIP => CardReaderCapabilitiesClass.DeviceTypeEnum.Dip,
            WFS_IDC_TYPECONTACTLESS => CardReaderCapabilitiesClass.DeviceTypeEnum.Contactless,
            WFS_IDC_TYPELATCHEDDIP => CardReaderCapabilitiesClass.DeviceTypeEnum.LatchedDip,
            WFS_IDC_TYPEPERMANENT => CardReaderCapabilitiesClass.DeviceTypeEnum.Permanent,
            WFS_IDC_TYPEINTELLIGENTCONTACTLESS => CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown {nameof(FwType)}[{val}]")
        };
    }

    /* values of WFSIDCCAPS.fwReadTracks,
                         WFSIDCCAPS.fwWriteTracks,
                         WFSIDCCARDDATA.wDataSource,
                         WFS_CMD_IDC_READ_RAW_DATA */
    public static class FwReadTracks
    {
        #pragma warning disable format
        public const WORD WFS_IDC_TRACK1                      = 0x0001;
        public const WORD WFS_IDC_TRACK2                      = 0x0002;
        public const WORD WFS_IDC_TRACK3                      = 0x0004;
        public const WORD WFS_IDC_FRONT_TRACK_1               = 0x0080;
        public const WORD WFS_IDC_TRACK1_JIS1                 = 0x0400;
        public const WORD WFS_IDC_TRACK3_JIS1                 = 0x0800;
        #pragma warning restore format
        public static CardReaderCapabilitiesClass.ReadableDataTypesEnum ToEnum(WORD val)
        {
            CardReaderCapabilitiesClass.ReadableDataTypesEnum dwRet = CardReaderCapabilitiesClass.ReadableDataTypesEnum.NotSupported;
            if ((val & WFS_IDC_TRACK1) == WFS_IDC_TRACK1)
            {
                dwRet |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1;
            }
            if ((val & WFS_IDC_TRACK2) == WFS_IDC_TRACK2)
            {
                dwRet |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track2;
            }
            if ((val & WFS_IDC_TRACK3) == WFS_IDC_TRACK3)
            {
                dwRet |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3;
            }
            if ((val & WFS_IDC_FRONT_TRACK_1) == WFS_IDC_FRONT_TRACK_1)
            {
                dwRet |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1Front;
            }
            if ((val & WFS_IDC_TRACK1_JIS1) == WFS_IDC_TRACK1_JIS1)
            {
                dwRet |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1JIS;
            }
            if ((val & WFS_IDC_TRACK3_JIS1) == WFS_IDC_TRACK3_JIS1)
            {
                dwRet |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3JIS;
            }
            return dwRet;
        }

        public static WORD FromEnum(ReadCardRequest.CardDataTypesEnum cardDataTypesEnum)
        {
            WORD ret = 0;
            if ((cardDataTypesEnum & CardDataTypesEnum.Track1) == CardDataTypesEnum.Track1)
            {
                ret |= WFS_IDC_TRACK1;
            }
            if ((cardDataTypesEnum & CardDataTypesEnum.Track2) == CardDataTypesEnum.Track2)
            {
                ret |= WFS_IDC_TRACK2;
            }
            if ((cardDataTypesEnum & CardDataTypesEnum.Track3) == CardDataTypesEnum.Track3)
            {
                ret |= WFS_IDC_TRACK3;
            }
            if ((cardDataTypesEnum & CardDataTypesEnum.Track1Front) == CardDataTypesEnum.Track1Front)
            {
                ret |= WFS_IDC_FRONT_TRACK_1;
            }
            if ((cardDataTypesEnum & CardDataTypesEnum.Track1JIS) == CardDataTypesEnum.Track1JIS)
            {
                ret |= WFS_IDC_TRACK1_JIS1;
            }
            if ((cardDataTypesEnum & CardDataTypesEnum.Track3JIS) == CardDataTypesEnum.Track3JIS)
            {
                ret |= WFS_IDC_TRACK3_JIS1;
            }
            return ret;
        }
    }

    public static class WDataSource
    {
        public static ReadCardRequest.CardDataTypesEnum ToEnum(WORD val) => val switch
        {
            FwReadTracks.WFS_IDC_TRACK1 => ReadCardRequest.CardDataTypesEnum.Track1,
            FwReadTracks.WFS_IDC_TRACK2 => ReadCardRequest.CardDataTypesEnum.Track2,
            FwReadTracks.WFS_IDC_TRACK3 => ReadCardRequest.CardDataTypesEnum.Track3,
            FwReadTracks.WFS_IDC_FRONT_TRACK_1 => ReadCardRequest.CardDataTypesEnum.Track1Front,
            FwReadTracks.WFS_IDC_TRACK1_JIS1 => ReadCardRequest.CardDataTypesEnum.Track1JIS,
            FwReadTracks.WFS_IDC_TRACK3_JIS1 => ReadCardRequest.CardDataTypesEnum.Track3JIS,
            _ => ReadCardRequest.CardDataTypesEnum.NoDataRead
        };

    }

    public static class FwWriteTracks
    {
        public static CardReaderCapabilitiesClass.WritableDataTypesEnum ToEnum(WORD val)
        {
            CardReaderCapabilitiesClass.WritableDataTypesEnum dwRet = CardReaderCapabilitiesClass.WritableDataTypesEnum.NotSupported;
            if ((val & FwReadTracks.WFS_IDC_TRACK1) == FwReadTracks.WFS_IDC_TRACK1)
            {
                dwRet |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1;
            }
            if ((val & FwReadTracks.WFS_IDC_TRACK2) == FwReadTracks.WFS_IDC_TRACK2)
            {
                dwRet |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track2;
            }
            if ((val & FwReadTracks.WFS_IDC_TRACK3) == FwReadTracks.WFS_IDC_TRACK3)
            {
                dwRet |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3;
            }
            if ((val & FwReadTracks.WFS_IDC_FRONT_TRACK_1) == FwReadTracks.WFS_IDC_FRONT_TRACK_1)
            {
                dwRet |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1Front;
            }
            if ((val & FwReadTracks.WFS_IDC_TRACK1_JIS1) == FwReadTracks.WFS_IDC_TRACK1_JIS1)
            {
                dwRet |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1JIS;
            }
            if ((val & FwReadTracks.WFS_IDC_TRACK3_JIS1) == FwReadTracks.WFS_IDC_TRACK3_JIS1)
            {
                dwRet |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3JIS;
            }
            return dwRet;
        }
    }

    /* values of WFSIDCCAPS.fwChipProtocols */
    public static class FwChipProtocols
    {
        #pragma warning disable format
        public const WORD WFS_IDC_CHIPT0                        = 0x0001;
        public const WORD WFS_IDC_CHIPT1                        = 0x0002;
        public const WORD WFS_IDC_CHIP_PROTOCOL_NOT_REQUIRED    = 0x0004;
        public const WORD WFS_IDC_CHIPTYPEA_PART3               = 0x0008;
        public const WORD WFS_IDC_CHIPTYPEA_PART4               = 0x0010;
        public const WORD WFS_IDC_CHIPTYPEB                     = 0x0020;
        public const WORD WFS_IDC_CHIPNFC                       = 0x0040;
        #pragma warning restore format
        public static CardReaderCapabilitiesClass.ChipProtocolsEnum ToEnum(WORD val)
        {
            CardReaderCapabilitiesClass.ChipProtocolsEnum wRet = CardReaderCapabilitiesClass.ChipProtocolsEnum.NotSupported;
            if ((val & WFS_IDC_CHIPT0) == WFS_IDC_CHIPT0)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.T0;
            }
            if ((val & WFS_IDC_CHIPT1) == WFS_IDC_CHIPT1)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.T1;
            }
            if ((val & WFS_IDC_CHIP_PROTOCOL_NOT_REQUIRED) == WFS_IDC_CHIP_PROTOCOL_NOT_REQUIRED)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.NotRequired;
            }
            if ((val & WFS_IDC_CHIPTYPEA_PART3) == WFS_IDC_CHIPTYPEA_PART3)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart3;
            }
            if ((val & WFS_IDC_CHIPTYPEA_PART4) == WFS_IDC_CHIPTYPEA_PART4)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart4;
            }
            if ((val & WFS_IDC_CHIPTYPEB) == WFS_IDC_CHIPTYPEB)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeB;
            }
            if ((val & WFS_IDC_CHIPNFC) == WFS_IDC_CHIPNFC)
            {
                wRet |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeNFC;
            }
            return wRet;
        }
    }

    /* values of WFSIDCCAPS.fwSecType */
    public static class FwSecType
    {
        #pragma warning disable format
        public const WORD WFS_IDC_SECNOTSUPP    = (1);
        public const WORD WFS_IDC_SECMMBOX      = (2);
        public const WORD WFS_IDC_SECCIM86      = (3);
        #pragma warning restore format

        public static CardReaderCapabilitiesClass.SecurityTypeEnum ToEnum(WORD val) => val switch
        {
            WFS_IDC_SECNOTSUPP => CardReaderCapabilitiesClass.SecurityTypeEnum.NotSupported,
            WFS_IDC_SECMMBOX => CardReaderCapabilitiesClass.SecurityTypeEnum.Mm,
            WFS_IDC_SECCIM86 => CardReaderCapabilitiesClass.SecurityTypeEnum.Cim86,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown {nameof(FwSecType)}[{val}]")
        };

    }

    /* values of WFSIDCCAPS.fwPowerOnOption,
             WFSIDCCAPS.fwPowerOffOption*/
    public static class FwPowerOffOption
    {
        #pragma warning disable format
        public const WORD WFS_IDC_NOACTION          = (1);
        public const WORD WFS_IDC_EJECT             = (2);
        public const WORD WFS_IDC_RETAIN            = (3);
        public const WORD WFS_IDC_EJECTTHENRETAIN   = (4);
        public const WORD WFS_IDC_READPOSITION      = (5);
        #pragma warning restore format
        public static CardReaderCapabilitiesClass.PowerOptionEnum ToEnum(WORD val) => val switch
        {
            WFS_IDC_NOACTION => CardReaderCapabilitiesClass.PowerOptionEnum.NoAction,
            WFS_IDC_EJECT => CardReaderCapabilitiesClass.PowerOptionEnum.Exit,
            WFS_IDC_RETAIN => CardReaderCapabilitiesClass.PowerOptionEnum.Retain,
            WFS_IDC_EJECTTHENRETAIN => CardReaderCapabilitiesClass.PowerOptionEnum.ExitThenRetain,
            WFS_IDC_READPOSITION => CardReaderCapabilitiesClass.PowerOptionEnum.Transport,
            _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown {nameof(FwPowerOffOption)}[{val}]")
        };
        public static WORD FromEnum(ResetDeviceRequest.ToEnum to) => to switch
        {
            ResetDeviceRequest.ToEnum.CurrentPosition => WFS_IDC_NOACTION,
            ResetDeviceRequest.ToEnum.Exit => WFS_IDC_EJECT,
            ResetDeviceRequest.ToEnum.Retain => WFS_IDC_RETAIN,
            ResetDeviceRequest.ToEnum.Default => WFS_IDC_RETAIN,
            _ => throw new NotImplementedException()
        };
    }

    public static class FwPowerOnOption
    {
        public static CardReaderCapabilitiesClass.PowerOptionEnum ToEnum(WORD val) => FwPowerOffOption.ToEnum(val);
    }

    /* values of WFSIDCCAPS.fwWriteMode,
             WFSIDCWRITETRACK.fwWriteMethod,
             WFSIDCCARDDATA.fwWriteMethod */

    /* Note: WFS_IDC_UNKNOWN was removed as it was an invalid value */
    public static class FwWriteMode
    {
        public const WORD WFS_IDC_LOCO = 0x0002;
        public const WORD WFS_IDC_HICO = 0x0004;
        public const WORD WFS_IDC_AUTO = 0x0008;
        public static CardReaderCapabilitiesClass.WriteMethodsEnum ToEnum(WORD val)
        {
            CardReaderCapabilitiesClass.WriteMethodsEnum wRet = CardReaderCapabilitiesClass.WriteMethodsEnum.NotSupported;
            if ((val & WFS_IDC_LOCO) == WFS_IDC_LOCO)
            {
                wRet |= CardReaderCapabilitiesClass.WriteMethodsEnum.Loco;
            }
            if ((val & WFS_IDC_HICO) == WFS_IDC_HICO)
            {
                wRet |= CardReaderCapabilitiesClass.WriteMethodsEnum.Hico;
            }
            if ((val & WFS_IDC_AUTO) == WFS_IDC_AUTO)
            {
                wRet |= CardReaderCapabilitiesClass.WriteMethodsEnum.Auto;
            }
            return wRet;
        }
    }

    /* values of WFSIDCCAPS.fwChipPower */
    public static class FwChipPower
    {
        #pragma warning disable format
        public const WORD WFS_IDC_CHIPPOWERCOLD = 0x0002;
        public const WORD WFS_IDC_CHIPPOWERWARM = 0x0004;
        public const WORD WFS_IDC_CHIPPOWEROFF  = 0x0008;
        #pragma warning restore format
        public static CardReaderCapabilitiesClass.ChipPowerOptionsEnum ToEnum(WORD val)
        {
            CardReaderCapabilitiesClass.ChipPowerOptionsEnum wRet = CardReaderCapabilitiesClass.ChipPowerOptionsEnum.NotSupported;
            if ((val & WFS_IDC_CHIPPOWERCOLD) == WFS_IDC_CHIPPOWERCOLD)
            {
                wRet |= CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Cold;
            }
            if ((val & WFS_IDC_CHIPPOWERWARM) == WFS_IDC_CHIPPOWERWARM)
            {
                wRet |= CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Warm;
            }
            if ((val & WFS_IDC_CHIPPOWEROFF) == WFS_IDC_CHIPPOWEROFF)
            {
                wRet |= CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Off;
            }
            return wRet;
        }
    }

    /* values of WFSIDCCARDACT.wAction */
    public static class WAction
    {
        #pragma warning disable format
        public const WORD WFS_IDC_CARDRETAINED      = 1;
        public const WORD WFS_IDC_CARDEJECTED       = 2;
        public const WORD WFS_IDC_CARDREADPOSITION  = 3;
        public const WORD WFS_IDC_CARDJAMMED        = 4;
        #pragma warning restore format

        public static MovePosition.MovePositionEnum ToEnum(WORD type) => type switch
        {
            WFS_IDC_CARDRETAINED => MovePosition.MovePositionEnum.Storage,
            WFS_IDC_CARDEJECTED => MovePosition.MovePositionEnum.Exit,
            WFS_IDC_CARDREADPOSITION => MovePosition.MovePositionEnum.Transport,
            _ => MovePosition.MovePositionEnum.Transport
        };

        public static MovePosition.MovePositionEnum ReadResetOut(ref WFSRESULT wfsResult) => ToEnum(ResultData.ReadWord(wfsResult.lpBuffer));

    }
}