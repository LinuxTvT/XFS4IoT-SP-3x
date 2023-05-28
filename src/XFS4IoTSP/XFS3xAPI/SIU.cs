using System.Runtime.InteropServices;
using XFS4IoTFramework.Common;
using BOOL = System.Int32;
using DWORD = System.UInt32;
using LPDWORD = System.IntPtr;
using LPSTR = System.IntPtr;
using LPWFSSIUTRMINFO = System.IntPtr;
using USHORT = System.UInt16;
using WORD = System.UInt16;

namespace XFS3xAPI.SIU
{
    public static class SIUExtension
    {
        public static WFSSIUCAPS300 ReadSIUCaps300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSSIUCAPS300>(wfsResult.lpBuffer);
        public static WFSSIUCAPS ReadSIUCaps(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSSIUCAPS>(wfsResult.lpBuffer);
        public static WFSSIUSTATUS300 ReadSIUStatus300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSSIUSTATUS300>(wfsResult.lpBuffer);
        public static WFSSIUSTATUS ReadSIUStatus(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSSIUSTATUS>(wfsResult.lpBuffer);

    }
    internal static class CLASS
    {
        #pragma warning disable format
        /* values of WFSSIUCAPS.wClass */

        public const DWORD  WFS_SERVICE_CLASS_SIU           = (8);
        public const string WFS_SERVICE_CLASS_NAME_SIU      = "SIU";
        public const DWORD  WFS_SERVICE_CLASS_VERSION_SIU   = (0x2803); /* Version 3.40 */
        public const DWORD  SIU_SERVICE_OFFSET              = (WFS_SERVICE_CLASS_SIU * 100);
        #pragma warning restore format
    }

    public static class DEF
    {
        #pragma warning disable format
		/* Size and max index of fwSensors array */

        public const int WFS_SIU_SENSORS_SIZE                = (32);
        public const int WFS_SIU_SENSORS_MAX                 = (WFS_SIU_SENSORS_SIZE - 1);

        /* Size and max index of fwDoors array */

        public const int WFS_SIU_DOORS_SIZE                  = (16);
        public const int WFS_SIU_DOORS_MAX                   = (WFS_SIU_DOORS_SIZE - 1);

        /* Size and max index of fwIndicators array */

        public const int WFS_SIU_INDICATORS_SIZE             = (16);
        public const int WFS_SIU_INDICATORS_MAX              = (WFS_SIU_INDICATORS_SIZE - 1);

        /* Size max index of fwAuxiliaries array */

        public const int WFS_SIU_AUXILIARIES_SIZE            = (16);
        public const int WFS_SIU_AUXILIARIES_MAX             = (WFS_SIU_AUXILIARIES_SIZE - 1);

        /* Size and max index of fwGuidLights array */

        public const int WFS_SIU_GUIDLIGHTS_SIZE             = (16);
        public const int WFS_SIU_GUIDLIGHTS_MAX              = (WFS_SIU_GUIDLIGHTS_SIZE - 1);

        /* Size and max index of fwGuidLightsEx array */

        public const int WFS_SIU_GUIDLIGHTS_SIZE_EX          = (32);
        public const int WFS_SIU_GUIDLIGHTS_MAX_EX           = (WFS_SIU_GUIDLIGHTS_SIZE_EX - 1);

        /* Values of WFSSIUSTATUS.fwSensors [...]
          WFSSIUSTATUS.fwDoors [...]
          WFSSIUSTATUS.fwIndicators [...]
          WFSSIUSTATUS.fwAuxiliaries [...]
          WFSSIUSTATUS.fwGuidLights [...]
          WFSSIUCAPS.fwSensors [...]
          WFSSIUCAPS.fwDoors [...]
          WFSSIUCAPS.fwIndicators [...]
          WFSSIUCAPS.fwAuxiliaries [...]
          WFSSIUCAPS.fwGuidLights [...] */

        public const WORD WFS_SIU_NOT_AVAILABLE = (0x0000);
        public const WORD WFS_SIU_AVAILABLE     = (0x0001);

        #pragma warning restore format
    }

    public static class CMD
    {
        #pragma warning disable format
        public const DWORD SIU_SERVICE_OFFSET = CLASS.SIU_SERVICE_OFFSET;

        /* SIU Info Commands */
        public const DWORD WFS_INF_SIU_STATUS                  = (SIU_SERVICE_OFFSET + 1);
        public const DWORD WFS_INF_SIU_CAPABILITIES            = (SIU_SERVICE_OFFSET + 2);
        public const DWORD WFS_INF_SIU_GET_AUTOSTARTUP_TIME    = (SIU_SERVICE_OFFSET + 3);

/* SIU Command Verbs */
        public const DWORD WFS_CMD_SIU_ENABLE_EVENTS           = (SIU_SERVICE_OFFSET + 1);
        public const DWORD WFS_CMD_SIU_SET_PORTS               = (SIU_SERVICE_OFFSET + 2);
        public const DWORD WFS_CMD_SIU_SET_DOOR                = (SIU_SERVICE_OFFSET + 3);
        public const DWORD WFS_CMD_SIU_SET_INDICATOR           = (SIU_SERVICE_OFFSET + 4);
        public const DWORD WFS_CMD_SIU_SET_AUXILIARY           = (SIU_SERVICE_OFFSET + 5);
        public const DWORD WFS_CMD_SIU_SET_GUIDLIGHT           = (SIU_SERVICE_OFFSET + 6);
        public const DWORD WFS_CMD_SIU_RESET                   = (SIU_SERVICE_OFFSET + 7);
        public const DWORD WFS_CMD_SIU_POWER_SAVE_CONTROL      = (SIU_SERVICE_OFFSET + 8);
        public const DWORD WFS_CMD_SIU_SET_AUTOSTARTUP_TIME    = (SIU_SERVICE_OFFSET + 9);
        public const DWORD WFS_CMD_SIU_SYNCHRONIZE_COMMAND     = (SIU_SERVICE_OFFSET + 10);
        public const DWORD WFS_CMD_SIU_SET_GUIDLIGHT_EX        = (SIU_SERVICE_OFFSET + 11);
        #pragma warning restore format
    }

    public static class EVENT
    {
        #pragma warning disable format
        public const DWORD SIU_SERVICE_OFFSET = CLASS.SIU_SERVICE_OFFSET;

        /* SIU Messages */
        public const DWORD WFS_SRVE_SIU_PORT_STATUS            = (SIU_SERVICE_OFFSET + 1);
        public const DWORD WFS_EXEE_SIU_PORT_ERROR             = (SIU_SERVICE_OFFSET + 2);
        public const DWORD WFS_SRVE_SIU_POWER_SAVE_CHANGE      = (SIU_SERVICE_OFFSET + 3);
        #pragma warning restore format
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSSIUCAPS300
    {
        WORD wClass;
        WORD fwType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_SENSORS_SIZE)]
        WORD[] fwSensors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_DOORS_SIZE)]
        WORD[] fwDoors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_INDICATORS_SIZE)]
        WORD[] fwIndicators;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_AUXILIARIES_SIZE)]
        WORD[] fwAuxiliaries;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE)]
        WORD[] fwGuidLights;
        LPSTR lpszExtra;
        /* 3.40
        BOOL bPowerSaveControl;
        WORD fwAutoStartupMode;
        BOOL bAntiFraudModule;
        LPDWORD lpdwSynchronizableCommands;
        LPWFSSIUTRMINFO lpTerminalInformation;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE_EX)]
        DWORD[] fwGuidLightsEx;
        */

        public Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsCapabilitiesClass.Light> Lights()
        {
            Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsCapabilitiesClass.Light> ret = new();
            for (USHORT i = 0; i < DEF.WFS_SIU_GUIDLIGHTS_SIZE; i++)
            {
                if (fwGuidLights[i] == DEF.WFS_SIU_AVAILABLE)
                {
                    ret.Add(WGuidLights.ToDeviceEnum(i), Light);
                }
            }
            return ret;
        }

        public static LightsCapabilitiesClass.ColorEnum Color => LightsCapabilitiesClass.ColorEnum.Default;

        public static LightsCapabilitiesClass.DirectionEnum Direction => LightsCapabilitiesClass.DirectionEnum.NotSupported;

        public static LightsCapabilitiesClass.LightPostionEnum LightPostion => LightsCapabilitiesClass.LightPostionEnum.Center;

        public static LightsCapabilitiesClass.FlashRateEnum FlashRate => LightsCapabilitiesClass.FlashRateEnum.Continuous |
                                                                            LightsCapabilitiesClass.FlashRateEnum.Medium |
                                                                            LightsCapabilitiesClass.FlashRateEnum.Quick |
                                                                            LightsCapabilitiesClass.FlashRateEnum.Slow |
                                                                            LightsCapabilitiesClass.FlashRateEnum.Off;
        public static LightsCapabilitiesClass.Light Light = new(FlashRate, Color, Direction, LightPostion);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSSIUCAPS
    {
        WORD wClass;
        WORD fwType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_SENSORS_SIZE)]
        WORD[] fwSensors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_DOORS_SIZE)]
        WORD[] fwDoors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_INDICATORS_SIZE)]
        WORD[] fwIndicators;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_AUXILIARIES_SIZE)]
        WORD[] fwAuxiliaries;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE)]
        WORD[] fwGuidLights;
        LPSTR lpszExtra;
        BOOL bPowerSaveControl;
        WORD fwAutoStartupMode;
        BOOL bAntiFraudModule;
        LPDWORD lpdwSynchronizableCommands;
        LPWFSSIUTRMINFO lpTerminalInformation;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE_EX)]
        DWORD[] fwGuidLightsEx;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSSIUSTATUS300
    {
        public WORD fwDevice;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_SENSORS_SIZE)]
        WORD[] fwSensors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_DOORS_SIZE)]
        WORD[] fwDoors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_INDICATORS_SIZE)]
        WORD[] fwIndicators;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_AUXILIARIES_SIZE)]
        WORD[] fwAuxiliaries;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE)]
        WORD[] fwGuidLights;
        LPSTR lpszExtra;
        /* 3.40
        USHORT usPowerSaveRecoveryTime;
        WORD wAntiFraudModule;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE_EX)]
        DWORD[] fwGuidLightsEx;
        */

        public static LightsStatusClass.LightOperation.PositionEnum LightPosition = LightsStatusClass.LightOperation.PositionEnum.Center;
        public static LightsStatusClass.LightOperation.ColourEnum LightColour = LightsStatusClass.LightOperation.ColourEnum.Default;
        public static LightsStatusClass.LightOperation.DirectionEnum LightDirection = LightsStatusClass.LightOperation.DirectionEnum.None;
        public static LightsStatusClass.LightOperation ToLightOperation(WORD val)
            => new(LightPosition, FwCommand.ToFlashRate(val), LightColour, LightDirection);

        public Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> LightsStatus()
        {
            Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> ret = new();
            for (USHORT i = 0; i < DEF.WFS_SIU_GUIDLIGHTS_SIZE; i++)
            {
                if (fwGuidLights[i] != DEF.WFS_SIU_NOT_AVAILABLE)
                {
                    ret.Add(WGuidLights.ToDeviceEnum(i), ToLightOperation(fwGuidLights[i]));
                }
            }
            return ret;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSSIUSTATUS
    {
        public WORD fwDevice;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_SENSORS_SIZE)]
        WORD[] fwSensors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_DOORS_SIZE)]
        WORD[] fwDoors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_INDICATORS_SIZE)]
        WORD[] fwIndicators;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_AUXILIARIES_SIZE)]
        WORD[] fwAuxiliaries;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE)]
        WORD[] fwGuidLights;
        LPSTR lpszExtra;
        USHORT usPowerSaveRecoveryTime;
        WORD wAntiFraudModule;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE_EX)]
        DWORD[] fwGuidLightsEx;
    }

    /* values of WFSSIUSTATUS.fwDevice */
    public static class FwDevice
    {
            #pragma warning disable format
            public const WORD WFS_SIU_DEVONLINE             = WFSDEVSTATUS.FwState.WFS_STAT_DEVONLINE;
            public const WORD WFS_SIU_DEVOFFLINE            = WFSDEVSTATUS.FwState.WFS_STAT_DEVOFFLINE;
            public const WORD WFS_SIU_DEVPOWEROFF           = WFSDEVSTATUS.FwState.WFS_STAT_DEVPOWEROFF;
            public const WORD WFS_SIU_DEVNODEVICE           = WFSDEVSTATUS.FwState.WFS_STAT_DEVNODEVICE;
            public const WORD WFS_SIU_DEVHWERROR            = WFSDEVSTATUS.FwState.WFS_STAT_DEVHWERROR;
            public const WORD WFS_SIU_DEVUSERERROR          = WFSDEVSTATUS.FwState.WFS_STAT_DEVUSERERROR;
            public const WORD WFS_SIU_DEVBUSY               = WFSDEVSTATUS.FwState.WFS_STAT_DEVBUSY;
            public const WORD WFS_SIU_DEVFRAUDATTEMPT       = WFSDEVSTATUS.FwState.WFS_STAT_DEVFRAUDATTEMPT;
            public const WORD WFS_SIU_DEVPOTENTIALFRAUD     = WFSDEVSTATUS.FwState.WFS_STAT_DEVPOTENTIALFRAUD;
            #pragma warning restore format
            public static string ToString(WORD state) => state switch
            {
                WFS_SIU_DEVONLINE => "WFS_SIU_DEVONLINE",
                WFS_SIU_DEVOFFLINE => "WFS_SIU_DEVOFFLINE",
                WFS_SIU_DEVPOWEROFF => "WFS_SIU_DEVPOWEROFF",
                WFS_SIU_DEVNODEVICE => "WFS_SIU_DEVNODEVICE",
                WFS_SIU_DEVHWERROR => "WFS_SIU_DEVHWERROR",
                WFS_SIU_DEVUSERERROR => "WFS_SIU_DEVUSERERROR",
                WFS_SIU_DEVBUSY => "WFS_SIU_DEVBUSY",
                WFS_SIU_DEVFRAUDATTEMPT => "WFS_SIU_DEVFRAUDATTEMPT",
                WFS_SIU_DEVPOTENTIALFRAUD => "WFS_SIU_DEVPOTENTIALFRAUD",
                _ => throw new UnknowConstException(state, typeof(FwDevice))
            };

        public static CommonStatusClass.DeviceEnum ToEnum(WORD state) => WFSDEVSTATUS.FwState.ToDeviceEnum(state);
    }

    public static class WGuidLights
    {
        #pragma warning disable format
        /* Indices of   WFSSIUSTATUS.fwGuidLights [...]
                        WFSSIUCAPS.fwGuidLights [...]
                        WFSSIUENABLE.fwGuidLights [...]
                        WFSSIUSETPORTS.wGuidLights [...]
                        WFSSIUSETGUIDLIGHT.wGuidLight
                        WFSSIUPORTEVENT.wPortIndex
                        WFSSIUPORTERROR.wPortIndex */

        public const WORD WFS_SIU_CARDUNIT                    = (0);
        public const WORD WFS_SIU_PINPAD                      = (1);
        public const WORD WFS_SIU_NOTESDISPENSER              = (2);
        public const WORD WFS_SIU_COINDISPENSER               = (3);
        public const WORD WFS_SIU_RECEIPTPRINTER              = (4);
        public const WORD WFS_SIU_PASSBOOKPRINTER             = (5);
        public const WORD WFS_SIU_ENVDEPOSITORY               = (6);
        public const WORD WFS_SIU_CHEQUEUNIT                  = (7);
        public const WORD WFS_SIU_BILLACCEPTOR                = (8);
        public const WORD WFS_SIU_ENVDISPENSER                = (9);
        public const WORD WFS_SIU_DOCUMENTPRINTER             = (10);
        public const WORD WFS_SIU_COINACCEPTOR                = (11);
        public const WORD WFS_SIU_SCANNER                     = (12);
        public const WORD WFS_SIU_CONTACTLESS                 = (16);
        public const WORD WFS_SIU_CARDUNIT2                   = (17);
        public const WORD WFS_SIU_NOTESDISPENSER2             = (18);
        public const WORD WFS_SIU_BILLACCEPTOR2               = (19);
        #pragma warning restore format

        public static LightsCapabilitiesClass.DeviceEnum ToDeviceEnum(WORD val) => val switch
        {
            WFS_SIU_CARDUNIT => LightsCapabilitiesClass.DeviceEnum.CardReader,
            WFS_SIU_PINPAD => LightsCapabilitiesClass.DeviceEnum.PinPad,
            WFS_SIU_NOTESDISPENSER => LightsCapabilitiesClass.DeviceEnum.NotesDispenser,
            WFS_SIU_COINDISPENSER => LightsCapabilitiesClass.DeviceEnum.CoinDispenser,
            WFS_SIU_RECEIPTPRINTER => LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter,
            WFS_SIU_PASSBOOKPRINTER => LightsCapabilitiesClass.DeviceEnum.PassbookPrinter,
            WFS_SIU_ENVDEPOSITORY => LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository,
            //WFS_SIU_CHEQUEUNIT => LightsCapabilitiesClass.DeviceEnum,
            WFS_SIU_BILLACCEPTOR => LightsCapabilitiesClass.DeviceEnum.BillAcceptor,
            WFS_SIU_ENVDISPENSER => LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser,
            WFS_SIU_DOCUMENTPRINTER => LightsCapabilitiesClass.DeviceEnum.DocumentPrinter,
            WFS_SIU_COINACCEPTOR => LightsCapabilitiesClass.DeviceEnum.CoinAcceptor,
            WFS_SIU_SCANNER => LightsCapabilitiesClass.DeviceEnum.Scanner,
            WFS_SIU_CONTACTLESS => LightsCapabilitiesClass.DeviceEnum.Contactless,
            WFS_SIU_CARDUNIT2 => LightsCapabilitiesClass.DeviceEnum.CardReader2,
            WFS_SIU_NOTESDISPENSER2 => LightsCapabilitiesClass.DeviceEnum.NotesDispenser2,
            WFS_SIU_BILLACCEPTOR2 => LightsCapabilitiesClass.DeviceEnum.BillAcceptor2,
            _ => throw new UnknowConstException(val, typeof(WGuidLights))
        };

        public static WORD FormDeviceEnum(LightsCapabilitiesClass.DeviceEnum val) => val switch
        {
            LightsCapabilitiesClass.DeviceEnum.CardReader => WFS_SIU_CARDUNIT,
            LightsCapabilitiesClass.DeviceEnum.PinPad => WFS_SIU_PINPAD,
            LightsCapabilitiesClass.DeviceEnum.NotesDispenser => WFS_SIU_NOTESDISPENSER,
            LightsCapabilitiesClass.DeviceEnum.CoinDispenser => WFS_SIU_COINDISPENSER,
            LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter => WFS_SIU_RECEIPTPRINTER,
            LightsCapabilitiesClass.DeviceEnum.PassbookPrinter => WFS_SIU_PASSBOOKPRINTER,
            LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository => WFS_SIU_ENVDEPOSITORY,
            //WFS_SIU_CHEQUEUNIT => LightsCapabilitiesClass.DeviceEnum,
            LightsCapabilitiesClass.DeviceEnum.BillAcceptor => WFS_SIU_BILLACCEPTOR,
            LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser => WFS_SIU_ENVDISPENSER,
            LightsCapabilitiesClass.DeviceEnum.DocumentPrinter => WFS_SIU_DOCUMENTPRINTER,
            LightsCapabilitiesClass.DeviceEnum.CoinAcceptor => WFS_SIU_COINACCEPTOR,
            LightsCapabilitiesClass.DeviceEnum.Scanner => WFS_SIU_SCANNER,
            LightsCapabilitiesClass.DeviceEnum.Contactless => WFS_SIU_CONTACTLESS,
            LightsCapabilitiesClass.DeviceEnum.CardReader2 => WFS_SIU_CARDUNIT2,
            LightsCapabilitiesClass.DeviceEnum.NotesDispenser2 => WFS_SIU_NOTESDISPENSER2,
            LightsCapabilitiesClass.DeviceEnum.BillAcceptor2 => WFS_SIU_BILLACCEPTOR2,
            _ => throw new UnknowEnumException(val, typeof(WGuidLights))
        };
    }

    public static class FwCommand
    {
        #pragma warning disable format
         /* Values of WFSSIUSTATUS.fwSensors [...]
                        WFSSIUSTATUS.fwIndicators [...]
                        WFSSIUSTATUS.fwAuxiliaries [...]
                        WFSSIUSTATUS.fwGuidLights [...]
                        WFSSIUCAPS.fwSensors [...]
                        WFSSIUCAPS.fwIndicators [...]
                        WFSSIUCAPS.fwGuidLights [...]
                        WFSSIUSETPORTS.fwIndicators [...]
                        WFSSIUSETPORTS.fwAuxiliaries [...]
                        WFSSIUSETPORTS.fwGuidLights [...]
                        WFSSIUSETINDICATOR.fwCommand [...]
                        WFSSIUSETAUXILIARY.fwCommand [...]
                        WFSSIUSETGUIDLIGHT.fwCommand [...]
                        WFSSIUPORTEVENT.wPortStatus
                        WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_OFF                         = (0x0001);
        public const WORD WFS_SIU_ON                          = (0x0002);
        public const WORD WFS_SIU_SLOW_FLASH                  = (0x0004);
        public const WORD WFS_SIU_MEDIUM_FLASH                = (0x0008);
        public const WORD WFS_SIU_QUICK_FLASH                 = (0x0010);
        public const WORD WFS_SIU_CONTINUOUS                  = (0x0080);
        public const DWORD WFS_SIU_GUIDANCE_RED                = (0x00000100);
        public const DWORD WFS_SIU_GUIDANCE_GREEN              = (0x00000200);
        public const DWORD WFS_SIU_GUIDANCE_YELLOW             = (0x00000400);
        public const DWORD WFS_SIU_GUIDANCE_BLUE               = (0x00000800);
        public const DWORD WFS_SIU_GUIDANCE_CYAN               = (0x00001000);
        public const DWORD WFS_SIU_GUIDANCE_MAGENTA            = (0x00002000);
        public const DWORD WFS_SIU_GUIDANCE_WHITE              = (0x00004000);
        public const DWORD WFS_SIU_GUIDANCE_ENTRY              = (0x00100000);
        public const DWORD WFS_SIU_GUIDANCE_EXIT               = (0x00200000);
        #pragma warning restore format

        public static WORD FormFlashRateEnum(LightsStatusClass.LightOperation.FlashRateEnum val) => val switch
        {
            LightsStatusClass.LightOperation.FlashRateEnum.Off => WFS_SIU_OFF,
            LightsStatusClass.LightOperation.FlashRateEnum.Slow => WFS_SIU_SLOW_FLASH,
            LightsStatusClass.LightOperation.FlashRateEnum.Medium => WFS_SIU_MEDIUM_FLASH,
            LightsStatusClass.LightOperation.FlashRateEnum.Quick => WFS_SIU_QUICK_FLASH,
            LightsStatusClass.LightOperation.FlashRateEnum.Continuous => WFS_SIU_CONTINUOUS,

            _ => throw new UnknowEnumException(val, typeof(WGuidLights))
        };

        public static LightsStatusClass.LightOperation.FlashRateEnum ToFlashRate(WORD val) => val switch
        {
            WFS_SIU_OFF => LightsStatusClass.LightOperation.FlashRateEnum.Off,
            WFS_SIU_SLOW_FLASH => LightsStatusClass.LightOperation.FlashRateEnum.Slow,
            WFS_SIU_MEDIUM_FLASH => LightsStatusClass.LightOperation.FlashRateEnum.Medium,
            WFS_SIU_QUICK_FLASH => LightsStatusClass.LightOperation.FlashRateEnum.Quick,
            WFS_SIU_CONTINUOUS => LightsStatusClass.LightOperation.FlashRateEnum.Continuous,
            _ => throw new UnknowConstException(val, typeof(WGuidLights))
        };
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSSIUSETGUIDLIGHT
    {
        [FieldOffset(0)] WORD wGuidLight;
        [FieldOffset(2)] WORD fwCommand;

        public static CommandData BuildCommandData(LightsCapabilitiesClass.DeviceEnum device, LightsStatusClass.LightOperation operation)
        {
            WFSSIUSETGUIDLIGHT dataCommand = new()
            {
                wGuidLight = WGuidLights.FormDeviceEnum(device),
                fwCommand = FwCommand.FormFlashRateEnum(operation.FlashRate)
            };

            return CommandData.FromStructureNoIntPtr(ref dataCommand);
        }
    }
}