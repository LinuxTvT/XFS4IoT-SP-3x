using System.Runtime.InteropServices;
using System.Text;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using DWORD = System.UInt32;
using HAPP = System.IntPtr;
using HRESULT = System.Int32;
using HSERVICE = System.UInt16;
using HWND = System.Int32;
using LPSTR = System.IntPtr;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using REQUESTID = System.UInt32;
using WORD = System.UInt16;

namespace XFS3xAPI
{
    public static class APIExtension
    {
        public static List<string> ReadLPSZ(this WFSRESULT wfsResult, int maxElement)
        {
            List<string> retList = new();
            LPSTR ptr = wfsResult.lpBuffer;
            while (ptr != IntPtr.Zero)
            {
                var str = Marshal.PtrToStringAnsi(ptr);
                // Console.WriteLine($"STR: [{str}]");
                if (str != null)
                {
                    retList.Add(str);
                    // GRGBanking SP bug, not add double null for last string
                    if (retList.Count == maxElement)
                    {
                        break;
                    }
                    ptr += (str.Length + 1);
                }
                else
                {
                    break;
                }

            }
            return retList;
        }
    }
    /// <summary>
    /// ****** String lengths **************************************************
    /// </summary>
    public static class STR_LEN
    {
        public const int WFSDDESCRIPTION_LEN = 256;
        public const int WFSDSYSSTATUS_LEN = 256;
    }

    public static class RESULT
    {
        /****** Error codes ******************************************************/
        #pragma warning disable format
        public const HRESULT WFS_SUCCESS                             =(0);
        public const HRESULT WFS_ERR_ALREADY_STARTED                 =(-1);
        public const HRESULT WFS_ERR_API_VER_TOO_HIGH                =(-2);
        public const HRESULT WFS_ERR_API_VER_TOO_LOW                 =(-3);
        public const HRESULT WFS_ERR_CANCELED                        =(-4);
        public const HRESULT WFS_ERR_CFG_INVALID_HKEY                =(-5);
        public const HRESULT WFS_ERR_CFG_INVALID_NAME                =(-6);
        public const HRESULT WFS_ERR_CFG_INVALID_SUBKEY              =(-7);
        public const HRESULT WFS_ERR_CFG_INVALID_VALUE               =(-8);
        public const HRESULT WFS_ERR_CFG_KEY_NOT_EMPTY               =(-9);
        public const HRESULT WFS_ERR_CFG_NAME_TOO_LONG               =(-10);
        public const HRESULT WFS_ERR_CFG_NO_MORE_ITEMS               =(-11);
        public const HRESULT WFS_ERR_CFG_VALUE_TOO_LONG              =(-12);
        public const HRESULT WFS_ERR_DEV_NOT_READY                   =(-13);
        public const HRESULT WFS_ERR_HARDWARE_ERROR                  =(-14);
        public const HRESULT WFS_ERR_INTERNAL_ERROR                  =(-15);
        public const HRESULT WFS_ERR_INVALID_ADDRESS                 =(-16);
        public const HRESULT WFS_ERR_INVALID_APP_HANDLE              =(-17);
        public const HRESULT WFS_ERR_INVALID_BUFFER                  =(-18);
        public const HRESULT WFS_ERR_INVALID_CATEGORY                =(-19);
        public const HRESULT WFS_ERR_INVALID_COMMAND                 =(-20);
        public const HRESULT WFS_ERR_INVALID_EVENT_CLASS             =(-21);
        public const HRESULT WFS_ERR_INVALID_HSERVICE                =(-22);
        public const HRESULT WFS_ERR_INVALID_HPROVIDER               =(-23);
        public const HRESULT WFS_ERR_INVALID_HWND                    =(-24);
        public const HRESULT WFS_ERR_INVALID_HWNDREG                 =(-25);
        public const HRESULT WFS_ERR_INVALID_POINTER                 =(-26);
        public const HRESULT WFS_ERR_INVALID_REQ_ID                  =(-27);
        public const HRESULT WFS_ERR_INVALID_RESULT                  =(-28);
        public const HRESULT WFS_ERR_INVALID_SERVPROV                =(-29);
        public const HRESULT WFS_ERR_INVALID_TIMER                   =(-30);
        public const HRESULT WFS_ERR_INVALID_TRACELEVEL              =(-31);
        public const HRESULT WFS_ERR_LOCKED                          =(-32);
        public const HRESULT WFS_ERR_NO_BLOCKING_CALL                =(-33);
        public const HRESULT WFS_ERR_NO_SERVPROV                     =(-34);
        public const HRESULT WFS_ERR_NO_SUCH_THREAD                  =(-35);
        public const HRESULT WFS_ERR_NO_TIMER                        =(-36);
        public const HRESULT WFS_ERR_NOT_LOCKED                      =(-37);
        public const HRESULT WFS_ERR_NOT_OK_TO_UNLOAD                =(-38);
        public const HRESULT WFS_ERR_NOT_STARTED                     =(-39);
        public const HRESULT WFS_ERR_NOT_REGISTERED                  =(-40);
        public const HRESULT WFS_ERR_OP_IN_PROGRESS                  =(-41);
        public const HRESULT WFS_ERR_OUT_OF_MEMORY                   =(-42);
        public const HRESULT WFS_ERR_SERVICE_NOT_FOUND               =(-43);
        public const HRESULT WFS_ERR_SPI_VER_TOO_HIGH                =(-44);
        public const HRESULT WFS_ERR_SPI_VER_TOO_LOW                 =(-45);
        public const HRESULT WFS_ERR_SRVC_VER_TOO_HIGH               =(-46);
        public const HRESULT WFS_ERR_SRVC_VER_TOO_LOW                =(-47);
        public const HRESULT WFS_ERR_TIMEOUT                         =(-48);
        public const HRESULT WFS_ERR_UNSUPP_CATEGORY                 =(-49);
        public const HRESULT WFS_ERR_UNSUPP_COMMAND                  =(-50);
        public const HRESULT WFS_ERR_VERSION_ERROR_IN_SRVC           =(-51);
        public const HRESULT WFS_ERR_INVALID_DATA                    =(-52);
        public const HRESULT WFS_ERR_SOFTWARE_ERROR                  =(-53);
        public const HRESULT WFS_ERR_CONNECTION_LOST                 =(-54);
        public const HRESULT WFS_ERR_USER_ERROR                      =(-55);
        public const HRESULT WFS_ERR_UNSUPP_DATA                     =(-56);
        public const HRESULT WFS_ERR_FRAUD_ATTEMPT                   =(-57);
        public const HRESULT WFS_ERR_SEQUENCE_ERROR                  =(-58);
        #pragma warning restore format

        public static MessagePayload.CompletionCodeEnum ToCompletionCode(HRESULT result) => result switch
        {
            WFS_SUCCESS => MessagePayload.CompletionCodeEnum.Success,
            WFS_ERR_ALREADY_STARTED => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_API_VER_TOO_HIGH => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_API_VER_TOO_LOW => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CANCELED => MessagePayload.CompletionCodeEnum.Canceled,
            WFS_ERR_CFG_INVALID_HKEY => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_INVALID_NAME => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_INVALID_SUBKEY => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_INVALID_VALUE => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_KEY_NOT_EMPTY => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_NAME_TOO_LONG => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_NO_MORE_ITEMS => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CFG_VALUE_TOO_LONG => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_DEV_NOT_READY => MessagePayload.CompletionCodeEnum.DeviceNotReady,
            WFS_ERR_HARDWARE_ERROR => MessagePayload.CompletionCodeEnum.HardwareError,
            WFS_ERR_INTERNAL_ERROR => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_ADDRESS => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_APP_HANDLE => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_BUFFER => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_CATEGORY => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_COMMAND => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_EVENT_CLASS => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_HSERVICE => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_HPROVIDER => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_HWND => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_HWNDREG => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_POINTER => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_REQ_ID => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_RESULT => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_SERVPROV => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_TIMER => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_TRACELEVEL => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_LOCKED => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NO_BLOCKING_CALL => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NO_SERVPROV => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NO_SUCH_THREAD => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NO_TIMER => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NOT_LOCKED => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NOT_OK_TO_UNLOAD => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NOT_STARTED => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_NOT_REGISTERED => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_OP_IN_PROGRESS => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_OUT_OF_MEMORY => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SERVICE_NOT_FOUND => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SPI_VER_TOO_HIGH => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SPI_VER_TOO_LOW => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SRVC_VER_TOO_HIGH => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SRVC_VER_TOO_LOW => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_TIMEOUT => MessagePayload.CompletionCodeEnum.TimeOut,
            WFS_ERR_UNSUPP_CATEGORY => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_UNSUPP_COMMAND => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_VERSION_ERROR_IN_SRVC => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_INVALID_DATA => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SOFTWARE_ERROR => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_CONNECTION_LOST => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_USER_ERROR => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_UNSUPP_DATA => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_FRAUD_ATTEMPT => MessagePayload.CompletionCodeEnum.InternalError,
            WFS_ERR_SEQUENCE_ERROR => MessagePayload.CompletionCodeEnum.InternalError,
            _ => MessagePayload.CompletionCodeEnum.InternalError
        };

        public static string ToString(HRESULT result) => result switch
        {
            WFS_SUCCESS => "WFS_SUCCESS",
            WFS_ERR_ALREADY_STARTED => "WFS_ERR_ALREADY_STARTED",
            WFS_ERR_API_VER_TOO_HIGH => "WFS_ERR_API_VER_TOO_HIGH",
            WFS_ERR_API_VER_TOO_LOW => "WFS_ERR_API_VER_TOO_LOW",
            WFS_ERR_CANCELED => "WFS_ERR_CANCELED",
            WFS_ERR_CFG_INVALID_HKEY => "WFS_ERR_CFG_INVALID_HKEY",
            WFS_ERR_CFG_INVALID_NAME => "WFS_ERR_CFG_INVALID_NAME",
            WFS_ERR_CFG_INVALID_SUBKEY => "WFS_ERR_CFG_INVALID_SUBKEY",
            WFS_ERR_CFG_INVALID_VALUE => "WFS_ERR_CFG_INVALID_VALUE",
            WFS_ERR_CFG_KEY_NOT_EMPTY => "WFS_ERR_CFG_KEY_NOT_EMPTY",
            WFS_ERR_CFG_NAME_TOO_LONG => "WFS_ERR_CFG_NAME_TOO_LONG",
            WFS_ERR_CFG_NO_MORE_ITEMS => "WFS_ERR_CFG_NO_MORE_ITEMS",
            WFS_ERR_CFG_VALUE_TOO_LONG => "WFS_ERR_CFG_VALUE_TOO_LONG",
            WFS_ERR_DEV_NOT_READY => "WFS_ERR_DEV_NOT_READY",
            WFS_ERR_HARDWARE_ERROR => "WFS_ERR_HARDWARE_ERROR",
            WFS_ERR_INTERNAL_ERROR => "WFS_ERR_INTERNAL_ERROR",
            WFS_ERR_INVALID_ADDRESS => "WFS_ERR_INVALID_ADDRESS",
            WFS_ERR_INVALID_APP_HANDLE => "WFS_ERR_INVALID_APP_HANDLE",
            WFS_ERR_INVALID_BUFFER => "WFS_ERR_INVALID_BUFFER",
            WFS_ERR_INVALID_CATEGORY => "WFS_ERR_INVALID_CATEGORY",
            WFS_ERR_INVALID_COMMAND => "WFS_ERR_INVALID_COMMAND",
            WFS_ERR_INVALID_EVENT_CLASS => "WFS_ERR_INVALID_EVENT_CLASS",
            WFS_ERR_INVALID_HSERVICE => "WFS_ERR_INVALID_HSERVICE",
            WFS_ERR_INVALID_HPROVIDER => "WFS_ERR_INVALID_HPROVIDER",
            WFS_ERR_INVALID_HWND => "WFS_ERR_INVALID_HWND",
            WFS_ERR_INVALID_HWNDREG => "WFS_ERR_INVALID_HWNDREG",
            WFS_ERR_INVALID_POINTER => "WFS_ERR_INVALID_POINTER",
            WFS_ERR_INVALID_REQ_ID => "WFS_ERR_INVALID_REQ_ID",
            WFS_ERR_INVALID_RESULT => "WFS_ERR_INVALID_RESULT",
            WFS_ERR_INVALID_SERVPROV => "WFS_ERR_INVALID_SERVPROV",
            WFS_ERR_INVALID_TIMER => "WFS_ERR_INVALID_TIMER",
            WFS_ERR_INVALID_TRACELEVEL => "WFS_ERR_INVALID_TRACELEVEL",
            WFS_ERR_LOCKED => "WFS_ERR_LOCKED",
            WFS_ERR_NO_BLOCKING_CALL => "WFS_ERR_NO_BLOCKING_CALL",
            WFS_ERR_NO_SERVPROV => "WFS_ERR_NO_SERVPROV",
            WFS_ERR_NO_SUCH_THREAD => "WFS_ERR_NO_SUCH_THREAD",
            WFS_ERR_NO_TIMER => "WFS_ERR_NO_TIMER",
            WFS_ERR_NOT_LOCKED => "WFS_ERR_NOT_LOCKED",
            WFS_ERR_NOT_OK_TO_UNLOAD => "WFS_ERR_NOT_OK_TO_UNLOAD",
            WFS_ERR_NOT_STARTED => "WFS_ERR_NOT_STARTED",
            WFS_ERR_NOT_REGISTERED => "WFS_ERR_NOT_REGISTERED",
            WFS_ERR_OP_IN_PROGRESS => "WFS_ERR_OP_IN_PROGRESS",
            WFS_ERR_OUT_OF_MEMORY => "WFS_ERR_OUT_OF_MEMORY",
            WFS_ERR_SERVICE_NOT_FOUND => "WFS_ERR_SERVICE_NOT_FOUND",
            WFS_ERR_SPI_VER_TOO_HIGH => "WFS_ERR_SPI_VER_TOO_HIGH",
            WFS_ERR_SPI_VER_TOO_LOW => "WFS_ERR_SPI_VER_TOO_LOW",
            WFS_ERR_SRVC_VER_TOO_HIGH => "WFS_ERR_SRVC_VER_TOO_HIGH",
            WFS_ERR_SRVC_VER_TOO_LOW => "WFS_ERR_SRVC_VER_TOO_LOW",
            WFS_ERR_TIMEOUT => "WFS_ERR_TIMEOUT",
            WFS_ERR_UNSUPP_CATEGORY => "WFS_ERR_UNSUPP_CATEGORY",
            WFS_ERR_UNSUPP_COMMAND => "WFS_ERR_UNSUPP_COMMAND",
            WFS_ERR_VERSION_ERROR_IN_SRVC => "WFS_ERR_VERSION_ERROR_IN_SRVC",
            WFS_ERR_INVALID_DATA => "WFS_ERR_INVALID_DATA",
            WFS_ERR_SOFTWARE_ERROR => "WFS_ERR_SOFTWARE_ERROR",
            WFS_ERR_CONNECTION_LOST => "WFS_ERR_CONNECTION_LOST",
            WFS_ERR_USER_ERROR => "WFS_ERR_USER_ERROR",
            WFS_ERR_UNSUPP_DATA => "WFS_ERR_UNSUPP_DATA",
            WFS_ERR_FRAUD_ATTEMPT => "WFS_ERR_FRAUD_ATTEMPT",
            WFS_ERR_SEQUENCE_ERROR => "WFS_ERR_SEQUENCE_ERROR",
            _ => $"WFS_ERR_[{result}]"
        };

        public static bool IsGenericError(HRESULT result) => (result > -100);
        public static bool IsSuccess(HRESULT result) => (result == WFS_SUCCESS);
    }

    public static class MSG
    {
        #pragma warning disable format
        // System.
        public const int WM_CREATE                               = 0x0001;
        /*
         * * Message-No = (WM_USER + No) *
         */
        public const int WM_USER                                 = 0x0400;
        public const int WFS_OPEN_COMPLETE                       =(WM_USER + 1);
        public const int WFS_CLOSE_COMPLETE                      =(WM_USER + 2);
        public const int WFS_LOCK_COMPLETE                       =(WM_USER + 3);
        public const int WFS_UNLOCK_COMPLETE                     =(WM_USER + 4);
        public const int WFS_REGISTER_COMPLETE                   =(WM_USER + 5);
        public const int WFS_DEREGISTER_COMPLETE                 =(WM_USER + 6);
        public const int WFS_GETINFO_COMPLETE                    =(WM_USER + 7);
        public const int WFS_EXECUTE_COMPLETE                    =(WM_USER + 8);

        public const int WFS_EXECUTE_EVENT                       =(WM_USER + 20);
        public const int WFS_SERVICE_EVENT                       =(WM_USER + 21);
        public const int WFS_USER_EVENT                          =(WM_USER + 22);
        public const int WFS_SYSTEM_EVENT                        =(WM_USER + 23);

        public const int WFS_TIMER_EVENT                         =(WM_USER + 100);

        public const int WM_USER_MAX                             =(WFS_TIMER_EVENT + 1);
        #pragma warning restore format

        public static string ToString(int msg) => msg switch
        {
            WFS_OPEN_COMPLETE => "WFS_OPEN_COMPLETE",
            WFS_CLOSE_COMPLETE => "WFS_CLOSE_COMPLETE",
            WFS_LOCK_COMPLETE => "WFS_LOCK_COMPLETE",
            WFS_UNLOCK_COMPLETE => "WFS_UNLOCK_COMPLETE",
            WFS_REGISTER_COMPLETE => "WFS_REGISTER_COMPLETE",
            WFS_DEREGISTER_COMPLETE => "WFS_DEREGISTER_COMPLETE",
            WFS_GETINFO_COMPLETE => "WFS_GETINFO_COMPLETE",
            WFS_EXECUTE_COMPLETE => "WFS_EXECUTE_COMPLETE",

            WFS_EXECUTE_EVENT => "WFS_EXECUTE_EVENT",
            WFS_SERVICE_EVENT => "WFS_SERVICE_EVENT",
            WFS_USER_EVENT => "WFS_USER_EVENT",
            WFS_SYSTEM_EVENT => "WFS_SYSTEM_EVENT",

            WFS_TIMER_EVENT => "WFS_TIMER_EVENT",
            _ => msg.ToString()
        };
    }

    /****** Event Classes ***************************************************/
    public static class EVENT_CLASSES
    { 
            #pragma warning disable format
            public const DWORD SERVICE_EVENTS       = (1);
            public const DWORD USER_EVENTS          = (2);
            public const DWORD SYSTEM_EVENTS        = (4);
            public const DWORD EXECUTE_EVENTS       = (8);
            #pragma warning restore format

            public static string ToString(DWORD evt)
        {
            StringBuilder sb = new StringBuilder();
            if ((evt & SERVICE_EVENTS) == SERVICE_EVENTS)
            {
                sb.Append($"|{nameof(SERVICE_EVENTS)}");
            }
            if ((evt & USER_EVENTS) == USER_EVENTS)
            {
                sb.Append($"|{nameof(USER_EVENTS)}");
            }
            if ((evt & SYSTEM_EVENTS) == SYSTEM_EVENTS)
            {
                sb.Append($"|{nameof(SYSTEM_EVENTS)}");
            }
            if ((evt & EXECUTE_EVENTS) == EXECUTE_EVENTS)
            {
                sb.Append($"|{nameof(EXECUTE_EVENTS)}");
            }
            return sb.ToString();
        }

        public const DWORD All = SERVICE_EVENTS | USER_EVENTS | SYSTEM_EVENTS | EXECUTE_EVENTS;
    }

    public static class STRACE_LEVEL
    {
        /****** XFS Trace Level ********************************************/
        #pragma warning disable format
        public const DWORD WFS_TRACE_API        = (0x00000001);
        public const DWORD WFS_TRACE_ALL_API    = (0x00000002);
        public const DWORD WFS_TRACE_SPI        = (0x00000004);
        public const DWORD WFS_TRACE_ALL_SPI    = (0x00000008);
        public const DWORD WFS_TRACE_MGR        = (0x00000010);
        public const DWORD All = WFS_TRACE_API | WFS_TRACE_ALL_API | WFS_TRACE_SPI | WFS_TRACE_ALL_SPI | WFS_TRACE_MGR;
        #pragma warning restore format
    }

    public static class WFSDEVSTATUS
    {
        /****** Values of WFSDEVSTATUS.fwState **********************************/
        public static class FwState
        {
            #pragma warning disable format
            public const WORD WFS_STAT_DEVONLINE                      = (0);
            public const WORD WFS_STAT_DEVOFFLINE                     = (1);
            public const WORD WFS_STAT_DEVPOWEROFF                    = (2);
            public const WORD WFS_STAT_DEVNODEVICE                    = (3);
            public const WORD WFS_STAT_DEVHWERROR                     = (4);
            public const WORD WFS_STAT_DEVUSERERROR                   = (5);
            public const WORD WFS_STAT_DEVBUSY                        = (6);
            public const WORD WFS_STAT_DEVFRAUDATTEMPT                = (7);
            public const WORD WFS_STAT_DEVPOTENTIALFRAUD              = (8);
            #pragma warning restore format

            public static CommonStatusClass.DeviceEnum ToDeviceEnum(WORD state) => state switch
            {
                WFS_STAT_DEVONLINE => CommonStatusClass.DeviceEnum.Online,
                WFS_STAT_DEVOFFLINE => CommonStatusClass.DeviceEnum.Offline,
                WFS_STAT_DEVPOWEROFF => CommonStatusClass.DeviceEnum.PowerOff,
                WFS_STAT_DEVNODEVICE => CommonStatusClass.DeviceEnum.NoDevice,
                WFS_STAT_DEVHWERROR => CommonStatusClass.DeviceEnum.HardwareError,
                WFS_STAT_DEVUSERERROR => CommonStatusClass.DeviceEnum.UserError,
                WFS_STAT_DEVBUSY => CommonStatusClass.DeviceEnum.DeviceBusy,
                WFS_STAT_DEVFRAUDATTEMPT => CommonStatusClass.DeviceEnum.FraudAttempt,
                WFS_STAT_DEVPOTENTIALFRAUD => CommonStatusClass.DeviceEnum.PotentialFraud,
                _ => throw new Xfs3xException(RESULT.WFS_ERR_INVALID_DATA, $"Unknown WFS_IDC_[{state}]")
            };
        }
    }

    /// <summary>
    /// This structure is used to return version information from WFSStartUp, WFSOpen and WFPOpen.
    /// </summary>
    /// <remarks>
    /// <code>
    /// typedef struct _wfsversion
    /// {
    ///     WORD wVersion;
    ///     WORD wLowVersion;
    ///     WORD wHighVersion;
    ///     CHAR szDescription[WFSDDESCRIPTION_LEN + 1];
    ///     CHAR szSystemStatus[WFSDSYSSTATUS_LEN + 1];
    /// } WFSVERSION, * LPWFSVERSION;
    /// </code>
    /// </remarks>
    public struct WFSVERSION
    {
        public WORD wVersion;
        public WORD wLowVersion;
        public WORD wHighVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = STR_LEN.WFSDDESCRIPTION_LEN + 1)]
        private byte[] szDescription;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = STR_LEN.WFSDSYSSTATUS_LEN + 1)]
        private byte[] szSystemStatus;

        public string Description => API.ASCIIString(szDescription);
        public string SystemStatus => Encoding.ASCII.GetString(szSystemStatus);
        public new string ToString() => $"Ver:[{VersionString(wVersion)}][{VersionString(wLowVersion)}->{VersionString(wHighVersion)}]-Desc: [{Description}]";

        public static string VersionString(WORD wVersion)
        {
            return $"{wVersion & 0x00ff}.{wVersion >> 8}";
        }

        public static string VersionRequireString(DWORD dwVersion)
        {
            WORD lowVersion = (WORD)(dwVersion >> 16);
            WORD highVersion = (WORD)(dwVersion & 0x0000FFFF);
            return $"{VersionString(lowVersion)} -> {VersionString(highVersion)}";
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi)]
    public struct SYSTEMTIME
    {
        [FieldOffset(0)] public ushort wYear;
        [FieldOffset(2)] public ushort wMonth;
        [FieldOffset(4)] public ushort wDayOfWeek;
        [FieldOffset(6)] public ushort wDay;
        [FieldOffset(8)] public ushort wHour;
        [FieldOffset(10)] public ushort wMinute;
        [FieldOffset(12)] public ushort wSecond;
        [FieldOffset(14)] public ushort wMilliseconds;

        public override string ToString() => $"{wDay}/{wMonth}/{wYear} {wHour}:{wMinute}:{wSecond}-{wMilliseconds}";

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct WFSRESULT
    {
        [FieldOffset(0)] public REQUESTID RequestID;
        [FieldOffset(4)] public HSERVICE hService;
        [FieldOffset(6)] public SYSTEMTIME tsTimestamp;
        [FieldOffset(22)] public HRESULT hResult;
        [FieldOffset(26)] public DWORD dwCommandCode;
        [FieldOffset(26)] public DWORD dwEventID;
        [FieldOffset(30)] public LPVOID lpBuffer;

        public override string ToString() => $"{nameof(RequestID)}={RequestID};" +
                                        $"{nameof(hService)}={hService};" +
                                        $"{nameof(tsTimestamp)}={tsTimestamp};" +
                                        $"{nameof(hResult)}={RESULT.ToString(hResult)};" +
                                        $"Cmd/Event={dwCommandCode}";

        public T? Data<T>(out bool isNull)
        {
            if (lpBuffer != LPVOID.Zero)
            {
                isNull = false;
            }
            else
            {
                isNull = true;
            }
            return ResultData.ReadStructure<T>(lpBuffer);
        }
    };

    public interface ICommandDataStructure
    {
        public void FreeAllPtr();
    }

    public class CommandData : IDisposable
    {
        internal static class CmdDataBuffer
        {
            private static readonly object _lock = new object();
            private static int _byteSize = 1;
            private static int _IntPrtSize = 4;
            private static int _wordSize = 2;
            private static int _dWordSize = 4;
            private static long _writeSizeCount = 0;

            private static Dictionary<int, int> _writeTtems = new();

            public static long Size { get => _writeSizeCount; }

            public static IntPtr Alloc(int size)
            {
                lock (_lock)
                {
                    IntPtr ptr = Marshal.AllocHGlobal(size);
                    if (ptr != IntPtr.Zero)
                    {
                        _writeTtems.Add(ptr.ToInt32(), size);
                        _writeSizeCount += size;
                    }
                    Console.WriteLine($"=>> Alloced: [{ptr}][{size}][{_writeSizeCount}]");
                    return ptr;
                }
            }

            public static void Free(IntPtr ptr)
            {
                lock (_lock)
                {
                    if (ptr != IntPtr.Zero)
                    {
                        if (_writeTtems.Remove(ptr.ToInt32(), out int size))
                        {
                            _writeSizeCount -= size;
                            Marshal.FreeHGlobal(ptr);
                            Console.WriteLine($"<== FREE: [{ptr}][{size}][{_writeSizeCount}]");
                        }
                        else
                        {
                            throw new Exception("Con not find item of Share memor to FREE");
                        }
                    }
                }
            }

            //public static IntPtr AllocPtrArray(int count) => Alloc(count * _IntPrtSize);

            public static IntPtr WriteWord(WORD val)
            {
                IntPtr ptr = Alloc(_wordSize);
                Marshal.WriteInt16(ptr, (Int16)(val));
                return ptr;
            }

            public static IntPtr WriteDWord(uint val)
            {
                IntPtr ptr = Alloc(_dWordSize);
                Marshal.WriteInt32(ptr, (Int32)(val));
                return ptr;
            }

            public static IntPtr WriteArrayPtr(IntPtr[] vals)
            {
                IntPtr ptr = Alloc(_IntPrtSize * vals.Length);
                for (int i = 0; i < vals.Length; i++)
                {
                    Marshal.WriteIntPtr(ptr, i, vals[i]);
                }
                return ptr;
            }

            public static IntPtr WriteArrayByte(byte[] vals)
            {
                IntPtr ptr = Alloc(_byteSize * vals.Length);
                Marshal.Copy(vals, 0, ptr, vals.Length);
                return ptr;
            }

            public static IntPtr WriteLPTRString(string val)
            {
                if (val == null)
                {
                    return IntPtr.Zero;
                }
                else
                {
                    var strLen = val.Length;
                    byte[] bytes = new byte[strLen + 1];
                    Encoding.ASCII.GetBytes(val, 0, strLen, bytes, 0);
                    bytes[strLen] = 0x00;
                    //Console.WriteLine($"Call [{nameof(WriteLPTRString)}]({val}) => len [{bytes.Length}], strlen [{val.Length}]");
                    IntPtr ptr = Alloc(bytes.Length);
                    Marshal.Copy(bytes, 0, ptr, bytes.Length);
                    return ptr;
                }

            }

            public static IntPtr WriteStructure<T>(ref T val, bool fDeleteOld = false)
            {
                if (val == null)
                {
                    return IntPtr.Zero;
                }
                else
                {
                    IntPtr ptr = Alloc(Marshal.SizeOf<T>());
                    Marshal.StructureToPtr<T>(val, ptr, fDeleteOld);
                    return ptr;
                }
            }
        }
        private List<IntPtr> _listPtr = new();
        public IntPtr DataPtr { get; private set; } = IntPtr.Zero;

        internal static CommandData FromStructureNoIntPtr<T>(ref T val)
        {
            return new CommandData(CmdDataBuffer.WriteStructure<T>(ref val));
        }

        public CommandData(IntPtr ptr)
        {
            DataPtr = ptr;
        }

        public CommandData(WORD val) : this(CmdDataBuffer.WriteWord(val)) { }
        public CommandData(DWORD val) : this(CmdDataBuffer.WriteDWord(val)) { }
        public CommandData(string val) : this(CmdDataBuffer.WriteLPTRString(val)) { }

        public CommandData() { DataPtr = IntPtr.Zero; }

        public void AddLPSTRField<T>(string name, string val)
        {
            var fieldPtr = CmdDataBuffer.WriteLPTRString(val);
            Marshal.WriteIntPtr(DataPtr, Marshal.OffsetOf<T>(name).ToInt32(), fieldPtr);
            _listPtr.Add(fieldPtr);
        }

        public void AddLPSZField<T>(string name, List<string> vals)
        {
            IntPtr[] arrPtr = new IntPtr[vals.Count];
            int itemIdx = 0;
            foreach (var item in vals)
            {
                var itemPtr = CmdDataBuffer.WriteLPTRString(item);
                arrPtr[itemIdx++] = itemPtr;
                _listPtr.Add(itemPtr);
            }

            var fieldPtr = CmdDataBuffer.WriteArrayPtr(arrPtr);
            Marshal.WriteIntPtr(DataPtr, Marshal.OffsetOf<T>(name).ToInt32(), fieldPtr);
            _listPtr.Add(fieldPtr);
        }

        public void AddLPBYTEField<T>(string name, List<byte> vals) => AddLPBYTEField<T>(name, vals.ToArray());

        public void AddLPBYTEField<T>(string name, byte[] vals)
        {
            var fieldPtr = CmdDataBuffer.WriteArrayByte(vals);
            Marshal.WriteIntPtr(DataPtr, Marshal.OffsetOf<T>(name).ToInt32(), fieldPtr);
            _listPtr.Add(fieldPtr);
        }

        public void Dispose()
        {
            FreeAllPtr();
            GC.SuppressFinalize(this);
        }
        protected virtual void FreeAllPtr()
        {
            //Console.WriteLine("============================================> Free COmmand");
            if (DataPtr != IntPtr.Zero)
            {
                foreach (var item in _listPtr)
                {
                    CmdDataBuffer.Free(item);
                }
                CmdDataBuffer.Free(DataPtr);
                DataPtr = IntPtr.Zero;
            }
        }
    }

    public class ResultData : IDisposable
    {
        private static int _ptrSize = Marshal.SizeOf(typeof(IntPtr));

        #region Static functions
        public static T? ReadStructure<T>(IntPtr ptr)
        {
            return Marshal.PtrToStructure<T>(ptr);
        }

        public static T? ReadStructure<T>(IntPtr ptr, int offset, out bool isNull)
        {
            IntPtr intPtr = Marshal.ReadIntPtr(ptr, offset * _ptrSize);
            if (intPtr == IntPtr.Zero)
            {
                isNull = true;
                return default;
            }
            else
            {
                isNull = false;
                return Marshal.PtrToStructure<T>(intPtr);
            }
        }

        public static List<T> ReadArrayStructure<T>(IntPtr ptr)
        {
            List<T> retList = new();
            if (ptr != IntPtr.Zero)
            {
                for (int offset = 0; ; offset++)
                {
                    T? item = ReadStructure<T>(ptr, offset, out bool isNull);
                    if (isNull)
                    {
                        break;
                    }
                    else
                    {
                        if (item != null)
                        {
                            retList.Add(item);
                        }
                    }
                }
            }
            return retList;
        }

        public static string? ReadLPSTR(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static WORD ReadWord(IntPtr ptr)
        {
            return (WORD)(Marshal.ReadInt16(ptr));
        }

        public static List<byte> ReadRawData(IntPtr ptr, int len)
        {
            byte[] byteArray = new byte[len];
            Marshal.Copy(ptr, byteArray, 0, len);
            return byteArray.ToList();
        }

        #endregion

        private LPWFSRESULT _ptr;
        public ResultData(LPWFSRESULT ptr) { _ptr = ptr; }

        public void Dispose()
        {
            FreeResult();
            GC.SuppressFinalize(this);
        }

        public WFSRESULT ReadResult() => Marshal.PtrToStructure<WFSRESULT>(_ptr);

        private void FreeResult()
        {
            if (_ptr != LPWFSRESULT.Zero)
            {
                //Console.WriteLine("============================================> Free LPWFSRESULT");
                var result = API.WFSFreeResult(_ptr);
                if (result != RESULT.WFS_SUCCESS)
                {
                    throw new Xfs3xException(result, "Free WFSRESULT");
                }
            }
        }
    }

    internal static class API
    {
        #region Import XFS API (version 3.40)

        [DllImport("msxfs.dll", EntryPoint = "WFSCancelAsyncRequest")]
        public static extern HRESULT WFSCancelAsyncRequest(HSERVICE hService, REQUESTID RequestID);

        [DllImport("msxfs.dll", EntryPoint = "WFSCancelBlockingCall")]
        public static extern HRESULT WFSCancelBlockingCall(DWORD dwThreadID);

        [DllImport("msxfs.dll", EntryPoint = "WFSCleanUp")]
        public static extern HRESULT WFSCleanUp();

        [DllImport("msxfs.dll", EntryPoint = "WFSClose")]
        public static extern HRESULT WFSClose(HSERVICE hService);

        [DllImport("msxfs.dll", EntryPoint = "WFSExecute")]
        public static extern HRESULT WFSExecute(HSERVICE hService, DWORD dwCommand, LPVOID lpCmdData, DWORD dwTimeOut, ref LPWFSRESULT lppResult);

        [DllImport("msxfs.dll", EntryPoint = "WFSFreeResult")]
        public static extern HRESULT WFSFreeResult(LPWFSRESULT lpResult);

        [DllImport("msxfs.dll", EntryPoint = "WFSGetInfo")]
        public static extern HRESULT WFSGetInfo(HSERVICE hService, DWORD dwCategory, LPVOID lpQueryDetails, DWORD dwTimeOut, ref LPWFSRESULT lppResult);

        [DllImport("msxfs.dll", EntryPoint = "WFSAsyncClose")]
        public static extern HRESULT WFSAsyncClose(HSERVICE hService, HWND hWnd, ref REQUESTID lpRequestID);

        [DllImport("msxfs.dll", EntryPoint = "WFSCreateAppHandle")]
        public static extern HRESULT WFSCreateAppHandle(ref HAPP phApp);

        [DllImport("msxfs.dll", EntryPoint = "WFSDeregister")]
        public static extern HRESULT WFSDeregister(HSERVICE hService, DWORD dwEventClass, HWND hWndReg);

        [DllImport("msxfs.dll", EntryPoint = "WFSStartUp")]
        public static extern HRESULT WFSStartUp(DWORD dwVersionsRequired, ref WFSVERSION lpWFSVersion);

        [DllImport("msxfs.dll", EntryPoint = "WFSOpen")]
        public static extern int WFSOpen([MarshalAs(UnmanagedType.LPStr)] string lpszLogicalName, HAPP hApp, [MarshalAs(UnmanagedType.LPStr)] string lpszAppID, DWORD dwTraceLevel, DWORD dwTimeOut, DWORD dwSrvcVersionsRequired, ref WFSVERSION lpSrvcVersion, ref WFSVERSION lpSPIVersion, ref HSERVICE lphService);

        [DllImport("msxfs.dll", EntryPoint = "WFSRegister")]
        public static extern HRESULT WFSRegister(HSERVICE hService, DWORD dwEventClass, HWND hWndReg);

        [DllImport("msxfs.dll", EntryPoint = "WFSAsyncExecute")]
        public static extern HRESULT WFSAsyncExecute(HSERVICE hService, DWORD dwCommand, LPVOID lpCmdData, DWORD dwTimeOut, HWND hWnd, ref REQUESTID lpRequestID);

        [DllImport("msxfs.dll", EntryPoint = "WFMSetTraceLevel")]
        public static extern HRESULT WFMSetTraceLevel(HSERVICE hService, DWORD dwTraceLevel);

        #endregion

        public static string ASCIIString(byte[] data)
        {
            int index = Array.IndexOf<byte>(data, 0x00);

            return Encoding.ASCII.GetString(data, 0, index);
        }

        public static List<string> GetExtra(LPSTR lpStr, int maxElement = 0)
        {
            List<string> retList = new();
            LPSTR ptr = lpStr;
            while (ptr != IntPtr.Zero)
            {
                var str = Marshal.PtrToStringAnsi(ptr);
                // Console.WriteLine($"STR: [{str}]");
                if (str != null)
                {
                    retList.Add(str);
                    // GRGBanking SP bug, not add double null for last string
                    if (retList.Count == maxElement)
                    {
                        break;
                    }
                    ptr += (str.Length + 1);
                }
                else
                {
                    break;
                }

            }
            return retList;
        }
    }
}