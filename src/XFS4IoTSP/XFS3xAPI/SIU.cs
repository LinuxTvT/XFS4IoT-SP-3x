using Microsoft.VisualBasic.Devices;
using System.Runtime.InteropServices;
using XFS4IoTFramework.Common;
using BOOL = System.Int32;
using DWORD = System.UInt32;
using HRESULT = System.Int32;
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
        public static WFSSIUPORTERROR ReadSIUPortError(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSSIUPORTERROR>(wfsResult.lpBuffer);
        public static WFSSIUPORTEVENT300 ReadSIUPortEvent300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSSIUPORTEVENT300>(wfsResult.lpBuffer);

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

        /* Values of WFSSIUENABLE.fwSensors [...]
                  WFSSIUENABLE.fwDoors [...]
                  WFSSIUENABLE.fwIndicators [...]
                  WFSSIUENABLE.fwAuxiliaries [...]
                  WFSSIUENABLE.fwGuidLights [...]
                  WFSSIUSETPORTS.fwDoors [...]
                  WFSSIUSETPORTS.fwIndicators [...]
                  WFSSIUSETPORTS.fwAuxiliaries [...]
                  WFSSIUSETPORTS.fwGuidLights [...] */
        public const WORD WFS_SIU_NO_CHANGE     = (0x0000);
        public const WORD WFS_SIU_ENABLE_EVENT  = (0x0001);
        public const WORD WFS_SIU_DISABLE_EVENT = (0x0002);

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
        public WORD[] fwSensors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_DOORS_SIZE)]
        public WORD[] fwDoors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_INDICATORS_SIZE)]
        public WORD[] fwIndicators;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_AUXILIARIES_SIZE)]
        public WORD[] fwAuxiliaries;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_SIU_GUIDLIGHTS_SIZE)]
        public WORD[] fwGuidLights;
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

        // Sensor

        public AuxiliariesCapabilities GetAuxiliaries()
        {
            return new AuxiliariesCapabilities(OperatorSwitch,
                                                AuxiliariesSupported(),
                                                EnhancedAudioSensor,
                                                HandsetSensor,
                                                HeadsetMicrophoneSensor,
                                                VandalShield,
                                                SupportedDoor(),
                                                Valume,
                                                Ups,
                                                EnhancedAudioControl,
                                                EnhancedMicrophoneControl,
                                                MicrophoneVolume,
                                                AutoStartupMode);
        }

        public AuxiliariesCapabilities.OperatorSwitchEnum OperatorSwitch => FwConst.ToOperatorSwitch(fwSensors[WSensorIndices.WFS_SIU_OPERATORSWITCH]);
        public AuxiliariesCapabilities.AuxiliariesSupportedEnum AuxiliariesSupported()
        {

            AuxiliariesCapabilities.AuxiliariesSupportedEnum ret = AuxiliariesCapabilities.AuxiliariesSupportedEnum.None;
            if (fwSensors[WSensorIndices.WFS_SIU_TAMPER] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.TamperSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_INTTAMPER] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.InternalTamperSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_SEISMIC] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.SeismicSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_HEAT] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.HeatSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_PROXIMITY] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.ProximitySensor;
            if (fwSensors[WSensorIndices.WFS_SIU_AMBLIGHT] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.AmbientLightSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_BOOT_SWITCH] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.BootSwitchSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_CONSUMER_DISPLAY] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.ConsumerDisplaySensor;
            if (fwSensors[WSensorIndices.WFS_SIU_OPERATOR_CALL_BUTTON] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.OperatorCallButtonSensor;
            if (fwSensors[WSensorIndices.WFS_SIU_FASCIAMICROPHONE] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.FasciaMicrophoneSensor;

            if (fwIndicators[WIndicatorIndices.WFS_SIU_OPENCLOSE] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.OpenCloseIndicator;
            if (fwIndicators[WIndicatorIndices.WFS_SIU_AUDIO] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.Audio;
            if (fwIndicators[WIndicatorIndices.WFS_SIU_HEATING] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.Heating;
            if (fwIndicators[WIndicatorIndices.WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.ConsumerDisplayBacklight;
            if (fwIndicators[WIndicatorIndices.WFS_SIU_SIGNAGEDISPLAY] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.SignageDisplay;

            if (fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_AUDIBLE_ALARM] == FwConst.WFS_SIU_AVAILABLE)
                ret |= AuxiliariesCapabilities.AuxiliariesSupportedEnum.AudibleAlarm;

            return ret;
        }
        public AuxiliariesCapabilities.EnhancedAudioCapabilitiesEnum EnhancedAudioSensor => AuxiliariesCapabilities.EnhancedAudioCapabilitiesEnum.NotAvailable;
        public AuxiliariesCapabilities.HandsetSensorCapabilities HandsetSensor => AuxiliariesCapabilities.HandsetSensorCapabilities.NotAvailable;
        public AuxiliariesCapabilities.HeadsetMicrophoneSensorCapabilities HeadsetMicrophoneSensor => AuxiliariesCapabilities.HeadsetMicrophoneSensorCapabilities.NotAvailable;
        public AuxiliariesCapabilities.VandalShieldCapabilities VandalShield => FwConst.ToVandalShield(fwDoors[WDoorIndices.WFS_SIU_VANDALSHIELD]);
        public Dictionary<AuxiliariesCapabilities.DoorType, AuxiliariesCapabilities.DoorCapabilities> SupportedDoor()
        {
            Dictionary<AuxiliariesCapabilities.DoorType, AuxiliariesCapabilities.DoorCapabilities> ret = new();
            CheckSupportedDoor(ret, WDoorIndices.WFS_SIU_SAFE);
            CheckSupportedDoor(ret, WDoorIndices.WFS_SIU_CABINET);
            CheckSupportedDoor(ret, WDoorIndices.WFS_SIU_CABINET_FRONT);
            CheckSupportedDoor(ret, WDoorIndices.WFS_SIU_CABINET_REAR);
            CheckSupportedDoor(ret, WDoorIndices.WFS_SIU_CABINET_LEFT);
            CheckSupportedDoor(ret, WDoorIndices.WFS_SIU_CABINET_LEFT);
            return ret;
        }
        public int Valume => (fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_VOLUME] == FwConst.WFS_SIU_NOT_AVAILABLE) ? 1
                                                                                        : fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_VOLUME];
        public AuxiliariesCapabilities.UpsEnum Ups => FwConst.ToUps(fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_UPS]);
        public AuxiliariesCapabilities.EnhancedAudioControlEnum EnhancedAudioControl
            => FwConst.ToEnhancedAudioControl(fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_ENHANCEDAUDIOCONTROL]);

        public AuxiliariesCapabilities.EnhancedAudioControlEnum EnhancedMicrophoneControl
            => FwConst.ToEnhancedAudioControl(fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_ENHANCEDMICROPHONECONTROL]);

        public int MicrophoneVolume => (fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_MICROPHONEVOLUME] == FwConst.WFS_SIU_NOT_AVAILABLE) ? 1
                                                                                        : fwAuxiliaries[WAuxiliaryIndices.WFS_SIU_MICROPHONEVOLUME];

        public AuxiliariesCapabilities.AutoStartupModes AutoStartupMode => AuxiliariesCapabilities.AutoStartupModes.NotAvailable;

        private void CheckSupportedDoor(Dictionary<AuxiliariesCapabilities.DoorType, AuxiliariesCapabilities.DoorCapabilities> dict, WORD doorIndeice)
        {
            if (fwDoors[doorIndeice] != FwConst.WFS_SIU_NOT_AVAILABLE)
            {
                dict.Add(WDoorIndices.ToDoorType(doorIndeice), FwConst.ToDoor(fwDoors[doorIndeice]));
            }
        }
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

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSSIUPORTERROR
    {
        [FieldOffset(0)] WORD wPortType;
        [FieldOffset(2)] WORD wPortIndex;
        [FieldOffset(4)] HRESULT PortError;
        [FieldOffset(8)] WORD wPortStatus;
        [FieldOffset(10)] LPSTR lpszExtra;
        [FieldOffset(14)] DWORD dwPortStatus;

        public new string ToString()
        {
            return $"{nameof(WFSSIUPORTERROR)}[{wPortType},{wPortIndex},{PortError}]";
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSSIUPORTEVENT300
    {
        [FieldOffset(0)] public WORD wPortType;
        [FieldOffset(2)] public WORD wPortIndex;
        [FieldOffset(4)] public WORD wPortStatus;
        [FieldOffset(8)] LPSTR lpszExtra;

        public new string ToString()
        {
            return $"{nameof(WFSSIUPORTEVENT300)}[{FwType.ToString(wPortType)},{wPortIndex},{wPortStatus}]";
        }

    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSSIUPORTEVENT
    {
        [FieldOffset(0)] WORD wPortType;
        [FieldOffset(2)] WORD wPortIndex;
        [FieldOffset(4)] WORD wPortStatus;
        [FieldOffset(8)] LPSTR lpszExtra;
        [FieldOffset(12)] DWORD dwPortStatus;

        public new string ToString()
        {
            return $"{nameof(WFSSIUPORTEVENT)}[{FwType.ToString(wPortType)},{wPortIndex},{wPortStatus}]";
        }
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

    public static class FwType
    {
        #pragma warning disable format
        /* Values of WFSSIUCAPS.fwType */

        public const WORD WFS_SIU_SENSORS                     = (0x0001);
        public const WORD WFS_SIU_DOORS                       = (0x0002);
        public const WORD WFS_SIU_INDICATORS                  = (0x0004);
        public const WORD WFS_SIU_AUXILIARIES                 = (0x0008);
        public const WORD WFS_SIU_GUIDLIGHTS                  = (0x0010);
        #pragma warning restore format

        public static string ToString(WORD val) => val switch
        {
            WFS_SIU_SENSORS => "WFS_SIU_SENSORS",
            WFS_SIU_DOORS => "WFS_SIU_DOORS",
            WFS_SIU_INDICATORS => "WFS_SIU_INDICATORS",
            WFS_SIU_AUXILIARIES => "WFS_SIU_AUXILIARIES",
            WFS_SIU_GUIDLIGHTS => "WFS_SIU_GUIDLIGHTS",
            _ => throw new UnknowConstException(val, typeof(FwType))
        };

    }

    public static class FwConst
    {
#region Const define
        #pragma warning disable format
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

        public const WORD WFS_SIU_NOT_AVAILABLE               = (0x0000);
        public const WORD WFS_SIU_AVAILABLE                   = (0x0001);

                                                                /* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_OPERATORSWITCH]
                                                                          WFSSIUCAPS.fwSensors [WFS_SIU_OPERATORSWITCH]
                                                                          WFSSIUPORTEVENT.wPortStatus
                                                                          WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_RUN                         = (0x0001);
        public const WORD WFS_SIU_MAINTENANCE                 = (0x0002);
        public const WORD WFS_SIU_SUPERVISOR                  = (0x0004);

                                                                          /* Values of WFSSIUSTATUS.fwDoors [...]
                                                                                    WFSSIUSTATUS.fwIndicators [WFS_SIU_OPENCLOSE]
                                                                                    WFSSIUCAPS.fwDoors [...]
                                                                                    WFSSIUCAPS.fwIndicators [WFS_SIU_OPENCLOSE]
                                                                                    WFSSIUSETPORTS.fwDoors [...]
                                                                                    WFSSIUSETPORTS.fwIndicators [WFS_SIU_OPENCLOSE]
                                                                                    WFSSIUSETDOOR.wDoor
                                                                                    WFSSIUSETINDICATOR.fwCommand
                                                                                    WFSSIUPORTEVENT.wPortStatus
                                                                                    WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_CLOSED                      = (0x0001);
        public const WORD WFS_SIU_OPEN                        = (0x0002);
        public const WORD WFS_SIU_LOCKED                      = (0x0004);
        public const WORD WFS_SIU_BOLTED                      = (0x0008);
        public const WORD WFS_SIU_SERVICE                     = (0x0010);
        public const WORD WFS_SIU_KEYBOARD                    = (0x0020);
        public const WORD WFS_SIU_AJAR                        = (0x0040);
        public const WORD WFS_SIU_JAMMED                      = (0x0080);
        public const WORD WFS_SIU_TAMPERED                    = (0x0100);

                                                                                    /* Values of WFSSIUSTATUS.fwIndicators [WFS_SIU_AUDIO]
                                                                                              WFSSIUSETPORTS.fwIndicators [WFS_SIU_AUDIO]
                                                                                              WFSSIUSETINDICATOR.fwCommand
                                                                                              WFSSIUPORTEVENT.wPortStatus
                                                                                              WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_KEYPRESS                    = (0x0002);
        public const WORD WFS_SIU_EXCLAMATION                 = (0x0004);
        public const WORD WFS_SIU_WARNING                     = (0x0008);
        public const WORD WFS_SIU_ERROR                       = (0x0010);
        public const WORD WFS_SIU_CRITICAL                    = (0x0020);

                                                                                              /* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_CONSUMER_DISPLAY]
                                                                                                        WFSSIUPORTEVENT.wPortStatus
                                                                                                        WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_DISPLAY_ERROR               = (0x0004);

                                                                                                        /* Flags for WFSSIUSTATUS.fwIndicators [WFS_SIU_TRANSINDICATOR]
                                                                                                                  WFSSIUSETPORTS.fwIndicators [WFS_SIU_TRANSINDICATOR]
                                                                                                                  WFSSIUSETINDICATOR.fwCommand
                                                                                                                  WFSSIUPORTEVENT.wPortStatus[WFS_SIU_TRANSINDICATOR]
                                                                                                                  WFSSIUPORTERROR.wPortStatus[WFS_SIU_TRANSINDICATOR] */

        public const WORD WFS_SIU_LAMP1                       = (0x0001);
        public const WORD WFS_SIU_LAMP2                       = (0x0002);
        public const WORD WFS_SIU_LAMP3                       = (0x0004);
        public const WORD WFS_SIU_LAMP4                       = (0x0008);
        public const WORD WFS_SIU_LAMP5                       = (0x0010);
        public const WORD WFS_SIU_LAMP6                       = (0x0020);
        public const WORD WFS_SIU_LAMP7                       = (0x0040);
        public const WORD WFS_SIU_LAMP8                       = (0x0080);
        public const WORD WFS_SIU_LAMP9                       = (0x0100);
        public const WORD WFS_SIU_LAMP10                      = (0x0200);
        public const WORD WFS_SIU_LAMP11                      = (0x0400);
        public const WORD WFS_SIU_LAMP12                      = (0x0800);
        public const WORD WFS_SIU_LAMP13                      = (0x1000);
        public const WORD WFS_SIU_LAMP14                      = (0x2000);
        public const WORD WFS_SIU_LAMP15                      = (0x4000);
        public const WORD WFS_SIU_LAMP16                      = (0x8000);

                                                                                                                  /* Values of WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_REMOTE_STATUS_MONITOR]
                                                                                                                            WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_REMOTE_STATUS_MONITOR]
                                                                                                                            WFSSIUSETAUXILIARY.fwCommand
                                                                                                                            WFSSIUPORTEVENT.wPortStatus
                                                                                                                            WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_GREEN_LED_ON                = (0x0001);
        public const WORD WFS_SIU_GREEN_LED_OFF               = (0x0002);
        public const WORD WFS_SIU_AMBER_LED_ON                = (0x0004);
        public const WORD WFS_SIU_AMBER_LED_OFF               = (0x0008);
        public const WORD WFS_SIU_RED_LED_ON                  = (0x0010);
        public const WORD WFS_SIU_RED_LED_OFF                 = (0x0020);

                                                                                                                            /* Values of WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
                                                                                                                                      WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
                                                                                                                                      WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
                                                                                                                                      WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL]
                                                                                                                                      WFSSIUSETAUXILIARY.fwCommand
                                                                                                                                      WFSSIUPORTEVENT.wPortStatus
                                                                                                                                      WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_PUBLICAUDIO_MANUAL          = (0x0001);
        public const WORD WFS_SIU_PUBLICAUDIO_AUTO            = (0x0002);
        public const WORD WFS_SIU_PUBLICAUDIO_SEMI_AUTO       = (0x0004);
        public const WORD WFS_SIU_PRIVATEAUDIO_MANUAL         = (0x0008);
        public const WORD WFS_SIU_PRIVATEAUDIO_AUTO           = (0x0010);
        public const WORD WFS_SIU_PRIVATEAUDIO_SEMI_AUTO      = (0x0020);

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

                                                                                                                                                /* Flags for WFSSIUSTATUS.fwSensors [WFS_SIU_GENERALINPUTPORT]
                                                                                                                                                          WFSSIUSTATUS.fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
                                                                                                                                                          WFSSIUSETPORTS.fwIndicators [WFS_SIU_GENERALOUTPUTPORT]
                                                                                                                                                          WFSSIUSETINDICATOR.fwCommand
                                                                                                                                                          WFSSIUPORTEVENT.wPortStatus[WFS_SIU_GENERALINPUTPORT]
                                                                                                                                                          WFSSIUPORTEVENT.wPortStatus[WFS_SIU_GENERALOUTPUTPORT]
                                                                                                                                                          WFSSIUPORTERROR.wPortStatus[WFS_SIU_GENERALINPUTPORT]
                                                                                                                                                          WFSSIUPORTERROR.wPortStatus[WFS_SIU_GENERALOUTPUTPORT] */

        public const WORD WFS_SIU_GPP1                        = (0x0001);
        public const WORD WFS_SIU_GPP2                        = (0x0002);
        public const WORD WFS_SIU_GPP3                        = (0x0004);
        public const WORD WFS_SIU_GPP4                        = (0x0008);
        public const WORD WFS_SIU_GPP5                        = (0x0010);
        public const WORD WFS_SIU_GPP6                        = (0x0020);
        public const WORD WFS_SIU_GPP7                        = (0x0040);
        public const WORD WFS_SIU_GPP8                        = (0x0080);
        public const WORD WFS_SIU_GPP9                        = (0x0100);
        public const WORD WFS_SIU_GPP10                       = (0x0200);
        public const WORD WFS_SIU_GPP11                       = (0x0400);
        public const WORD WFS_SIU_GPP12                       = (0x0800);
        public const WORD WFS_SIU_GPP13                       = (0x1000);
        public const WORD WFS_SIU_GPP14                       = (0x2000);
        public const WORD WFS_SIU_GPP15                       = (0x4000);
        public const WORD WFS_SIU_GPP16                       = (0x8000);

                                                                                                                                                          /* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_PROXIMITY]
                                                                                                                                                                    WFSSIUSTATUS.fwSensors [WFS_SIU_ENHANCEDAUDIO]
                                                                                                                                                                    WFSSIUPORTEVENT.wPortStatus
                                                                                                                                                                    WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_PRESENT                     = (0x0001);
        public const WORD WFS_SIU_NOT_PRESENT                 = (0x0002);

                                                                                                                                                                    /* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_HANDSETSENSOR] */

        public const WORD WFS_SIU_OFF_THE_HOOK                = (0x0001);
        public const WORD WFS_SIU_ON_THE_HOOK                 = (0x0002);

/* Values of WFSSIUCAPS.fwSensors [WFS_SIU_ENHANCEDAUDIO]
          WFSSIUCAPS.fwSensors [WFS_SIU_HANDSETSENSOR] */

        public const WORD WFS_SIU_MANUAL                      = (0x0001);
        public const WORD WFS_SIU_AUTO                        = (0x0002);
        public const WORD WFS_SIU_SEMI_AUTO                   = (0X0004);

          /* Values of WFSSIUCAPS.fwSensors [WFS_SIU_HANDSETSENSOR] */

        public const WORD WFS_SIU_MICROPHONE                  = (0x0010);

/* Values of WFSSIUCAPS.fwSensors [WFS_SIU_ENHANCEDAUDIO] */

        public const WORD WFS_SIU_BIDIRECTIONAL               = (0x0020);

/* Values of WFSSIUSTATUS.fwSensors [WFS_SIU_AMBLIGHT]
          WFSSIUCAPS.fwSensors [WFS_SIU_AMBLIGHT]
          WFSSIUPORTEVENT.wPortStatus
          WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_VERY_DARK                   = (0x0001);
        public const WORD WFS_SIU_DARK                        = (0x0002);
        public const WORD WFS_SIU_MEDIUM_LIGHT                = (0x0004);
        public const WORD WFS_SIU_LIGHT                       = (0x0008);
        public const WORD WFS_SIU_VERY_LIGHT                  = (0x0010);

          /* Values of WFSSIUSTATUS.fwAuxiliaries [WFS_SIU_UPS]
                       WFSSIUCAPS.fwAuxiliaries [WFS_SIU_UPS]
                       WFSSIUPORTEVENT.wPortStatus
                       WFSSIUPORTERROR.wPortStatus */

        public const WORD WFS_SIU_LOW                             = (0x0002);
        public const WORD WFS_SIU_ENGAGED                         = (0x0004);
        public const WORD WFS_SIU_POWERING                        = (0x0008);
        public const WORD WFS_SIU_RECOVERED                       = (0x0010);

                       /* Values of WFSSIUCAPS.fwType */

        public const WORD WFS_SIU_SENSORS                     = (0x0001);
        public const WORD WFS_SIU_DOORS                       = (0x0002);
        public const WORD WFS_SIU_INDICATORS                  = (0x0004);
        public const WORD WFS_SIU_AUXILIARIES                 = (0x0008);
        public const WORD WFS_SIU_GUIDLIGHTS                  = (0x0010);

/* Values of WFSSIUCAPS.fwAuxiliaries [WFS_SIU_ENHANCEDAUDIOCONTROL]
          WFSSIUCAPS.fwAuxiliaries [WFS_SIU_ENHANCEDMICROPHONECONTROL] */
        public const WORD WFS_SIU_HEADSET_DETECTION           = (0x0001);
        public const WORD WFS_SIU_MODE_CONTROLLABLE           = (0x0002);

          /* Values of WFSSIUENABLE.fwSensors [...]
                    WFSSIUENABLE.fwDoors [...]
                    WFSSIUENABLE.fwIndicators [...]
                    WFSSIUENABLE.fwAuxiliaries [...]
                    WFSSIUENABLE.fwGuidLights [...]
                    WFSSIUSETPORTS.fwDoors [...]
                    WFSSIUSETPORTS.fwIndicators [...]
                    WFSSIUSETPORTS.fwAuxiliaries [...]
                    WFSSIUSETPORTS.fwGuidLights [...] */

        public const WORD WFS_SIU_NO_CHANGE                   = (0x0000);
        public const WORD WFS_SIU_ENABLE_EVENT                = (0x0001);
        public const WORD WFS_SIU_DISABLE_EVENT               = (0x0002);

                    /* Values of WFSSIUSETPORTS.fwDoors [...]
                              WFSSIUSETDOOR.fwCommand [...] */

        public const WORD WFS_SIU_BOLT                        = (0x0001);
        public const WORD WFS_SIU_UNBOLT                      = (0x0002);

                              /* Values of WFSSIUSETPORTS.fwAuxiliaries [WFS_SIU_UPS]
                                    WFSSIUSETAUXILIARY.wAuxiliary [WFS_SIU_UPS] */

        public const WORD WFS_SIU_ENGAGE                      = (0x0001);
        public const WORD WFS_SIU_DISENGAGE                   = (0x0002);

                                    /* Values of WFSSIUCAPS.fwAutoStartupMode
                                              WFSSIUSETSTARTUPTIME.wMode
                                              WFSSIUGETSTARTUPTIME.wMode */

        public const WORD WFS_SIU_AUTOSTARTUP_CLEAR           = (0x0001);
        public const WORD WFS_SIU_AUTOSTARTUP_SPECIFIC        = (0x0002);
        public const WORD WFS_SIU_AUTOSTARTUP_DAILY           = (0x0004);
        public const WORD WFS_SIU_AUTOSTARTUP_WEEKLY          = (0x0008);

                                              /* Values of WFSSIUSTATUS.wAntiFraudModule */

        public const WORD WFS_SIU_AFMNOTSUPP                  = (0);
        public const WORD WFS_SIU_AFMOK                       = (1);
        public const WORD WFS_SIU_AFMINOP                     = (2);
        public const WORD WFS_SIU_AFMDEVICEDETECTED           = (3);
        public const WORD WFS_SIU_AFMUNKNOWN                  = (4);

        #pragma warning restore format
 #endregion

        public static AuxiliariesCapabilities.OperatorSwitchEnum ToOperatorSwitch(WORD val)
        {
            AuxiliariesCapabilities.OperatorSwitchEnum ret = AuxiliariesCapabilities.OperatorSwitchEnum.NotAvailable;
            if ((val & WFS_SIU_RUN) == WFS_SIU_RUN) ret |= AuxiliariesCapabilities.OperatorSwitchEnum.Run;
            if ((val & WFS_SIU_MAINTENANCE) == WFS_SIU_MAINTENANCE) ret |= AuxiliariesCapabilities.OperatorSwitchEnum.Maintenance;
            if ((val & WFS_SIU_SUPERVISOR) == WFS_SIU_SUPERVISOR) ret |= AuxiliariesCapabilities.OperatorSwitchEnum.Supervisor;
            return ret;
        }

        public static AuxiliariesStatus.OperatorSwitchEnum ToOperatorSwitchStatus(WORD val) => val switch
        {
            WFS_SIU_RUN => AuxiliariesStatus.OperatorSwitchEnum.Run,
            WFS_SIU_MAINTENANCE => AuxiliariesStatus.OperatorSwitchEnum.Maintenance,
            WFS_SIU_SUPERVISOR => AuxiliariesStatus.OperatorSwitchEnum.Supervisor,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.SensorEnum ToSensorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.SensorEnum.NotAvailable,
            WFS_SIU_ON => AuxiliariesStatus.SensorEnum.On,
            WFS_SIU_OFF => AuxiliariesStatus.SensorEnum.Off,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.PresenceSensorEnum ToPresenceSensorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.PresenceSensorEnum.NotAvailable,
            WFS_SIU_NOT_PRESENT => AuxiliariesStatus.PresenceSensorEnum.NotPresent,
            WFS_SIU_PRESENT => AuxiliariesStatus.PresenceSensorEnum.Present,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.AmbientLightSensorEnum ToAmbientLightSensorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.AmbientLightSensorEnum.NotAvailable,
            WFS_SIU_VERY_DARK => AuxiliariesStatus.AmbientLightSensorEnum.VeryDark,
            WFS_SIU_DARK => AuxiliariesStatus.AmbientLightSensorEnum.Dark,
            WFS_SIU_MEDIUM_LIGHT => AuxiliariesStatus.AmbientLightSensorEnum.MediumLight,
            WFS_SIU_LIGHT => AuxiliariesStatus.AmbientLightSensorEnum.Light,
            WFS_SIU_VERY_LIGHT => AuxiliariesStatus.AmbientLightSensorEnum.VeryLight,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.DisplaySensorEnum ToDisplaySensorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.DisplaySensorEnum.NotAvailable,
            WFS_SIU_ON => AuxiliariesStatus.DisplaySensorEnum.On,
            WFS_SIU_OFF => AuxiliariesStatus.DisplaySensorEnum.Off,
            WFS_SIU_DISPLAY_ERROR => AuxiliariesStatus.DisplaySensorEnum.DisplayError,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.HandsetSensorStatusEnum ToHandsetSensorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.HandsetSensorStatusEnum.NotAvailable,
            WFS_SIU_OFF_THE_HOOK => AuxiliariesStatus.HandsetSensorStatusEnum.OffTheHook,
            WFS_SIU_ON_THE_HOOK => AuxiliariesStatus.HandsetSensorStatusEnum.OnTheHook,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.DoorStatusEnum ToDoorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.DoorStatusEnum.NotAvailable,
            WFS_SIU_CLOSED => AuxiliariesStatus.DoorStatusEnum.Closed,
            WFS_SIU_OPEN => AuxiliariesStatus.DoorStatusEnum.Open,
            WFS_SIU_LOCKED => AuxiliariesStatus.DoorStatusEnum.Locked,
            WFS_SIU_BOLTED => AuxiliariesStatus.DoorStatusEnum.Bolted,
            WFS_SIU_TAMPERED => AuxiliariesStatus.DoorStatusEnum.Tampered,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.VandalShieldStatusEnum ToVandalShieldStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.VandalShieldStatusEnum.NotAvailable,
            WFS_SIU_CLOSED => AuxiliariesStatus.VandalShieldStatusEnum.Closed,
            WFS_SIU_OPEN => AuxiliariesStatus.VandalShieldStatusEnum.Open,
            WFS_SIU_LOCKED => AuxiliariesStatus.VandalShieldStatusEnum.Locked,
            WFS_SIU_SERVICE => AuxiliariesStatus.VandalShieldStatusEnum.Service,
            WFS_SIU_KEYBOARD => AuxiliariesStatus.VandalShieldStatusEnum.Keyboard,
            //WFS_SIU_PARTIALLY_OPEN => AuxiliariesStatus.VandalShieldStatusEnum.PartiallyOpen,
            WFS_SIU_JAMMED => AuxiliariesStatus.VandalShieldStatusEnum.Jammed,
            WFS_SIU_TAMPERED => AuxiliariesStatus.VandalShieldStatusEnum.Tampered,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.OpenClosedIndicatorEnum ToOpenClosedIndicatorStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.OpenClosedIndicatorEnum.NotAvailable,
            WFS_SIU_CLOSED => AuxiliariesStatus.OpenClosedIndicatorEnum.Closed,
            WFS_SIU_OPEN => AuxiliariesStatus.OpenClosedIndicatorEnum.Open,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static  (AuxiliariesStatus.AudioRateEnum, AuxiliariesStatus.AudioSignalEnum) ToAudioStatus(WORD val) 
        {
            AuxiliariesStatus.AudioRateEnum rate = AuxiliariesStatus.AudioRateEnum.Off;
            AuxiliariesStatus.AudioSignalEnum signal;
            if ((val & WFS_SIU_OFF) == WFS_SIU_OFF)
            {
                rate = AuxiliariesStatus.AudioRateEnum.Off;
            }

            if ((val & WFS_SIU_CONTINUOUS) == WFS_SIU_CONTINUOUS)
            {
                rate = AuxiliariesStatus.AudioRateEnum.On;
            }

            WORD valAsSignal = (ushort)(val & 0xFFFE);

            signal = valAsSignal switch
            {
                WFS_SIU_KEYPRESS => AuxiliariesStatus.AudioSignalEnum.Keypress,
                WFS_SIU_EXCLAMATION => AuxiliariesStatus.AudioSignalEnum.Exclamation,
                WFS_SIU_WARNING => AuxiliariesStatus.AudioSignalEnum.Warning,
                WFS_SIU_ERROR => AuxiliariesStatus.AudioSignalEnum.Error,
                WFS_SIU_CRITICAL => AuxiliariesStatus.AudioSignalEnum.Critical,
                _ => throw new UnknowConstException(val, typeof(FwConst))
            };

            return (rate, signal);
        }

        public static AuxiliariesStatus.UpsStatusEnum ToUpsStatus(WORD val)
        {
            if (val == WFS_SIU_NOT_AVAILABLE)
            {
                return AuxiliariesStatus.UpsStatusEnum.NotAvailable;
            } else
            {
                AuxiliariesStatus.UpsStatusEnum ret = AuxiliariesStatus.UpsStatusEnum.NotAvailable;
                if ((val & WFS_SIU_LOW) == WFS_SIU_LOW) ret |= AuxiliariesStatus.UpsStatusEnum.Low;
                if ((val & WFS_SIU_ENGAGED) == WFS_SIU_ENGAGED) ret |= AuxiliariesStatus.UpsStatusEnum.Engaged;
                if ((val & WFS_SIU_POWERING) == WFS_SIU_POWERING) ret |= AuxiliariesStatus.UpsStatusEnum.Powering;
                if ((val & WFS_SIU_RECOVERED) == WFS_SIU_RECOVERED) ret |= AuxiliariesStatus.UpsStatusEnum.Recovered;
                return ret;
            }

        }

        public static AuxiliariesStatus.EnhancedAudioControlEnum ToEnhancedAudioControlStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => AuxiliariesStatus.EnhancedAudioControlEnum.NotAvailable,
            WFS_SIU_PUBLICAUDIO_MANUAL => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual,
            WFS_SIU_PUBLICAUDIO_AUTO => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto,
            WFS_SIU_PUBLICAUDIO_SEMI_AUTO => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioSemiAuto,
            WFS_SIU_PRIVATEAUDIO_MANUAL => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual,
            WFS_SIU_PRIVATEAUDIO_AUTO => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto,
            WFS_SIU_PRIVATEAUDIO_SEMI_AUTO => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
            _ => throw new UnknowConstException(val, typeof(FwConst))
        };

        public static AuxiliariesStatus.MicrophoneVolumeStatus ToMicrophoneVolumeStatus(WORD val) => val switch
        {
            WFS_SIU_NOT_AVAILABLE => new AuxiliariesStatus.MicrophoneVolumeStatus(false, 0),
            _ => new AuxiliariesStatus.MicrophoneVolumeStatus(true, val)
        };

        public static AuxiliariesCapabilities.VandalShieldCapabilities ToVandalShield(WORD val)
        {
            AuxiliariesCapabilities.VandalShieldCapabilities ret = AuxiliariesCapabilities.VandalShieldCapabilities.NotAvailable;
            if ((val & WFS_SIU_CLOSED) == WFS_SIU_CLOSED)
                ret |= AuxiliariesCapabilities.VandalShieldCapabilities.Closed;
            if ((val & WFS_SIU_OPEN) == WFS_SIU_OPEN)
                ret |= AuxiliariesCapabilities.VandalShieldCapabilities.Open;
            if ((val & WFS_SIU_LOCKED) == WFS_SIU_LOCKED)
                ret |= AuxiliariesCapabilities.VandalShieldCapabilities.Locked;
            if ((val & WFS_SIU_SERVICE) == WFS_SIU_SERVICE)
                ret |= AuxiliariesCapabilities.VandalShieldCapabilities.Service;
            if ((val & WFS_SIU_TAMPERED) == WFS_SIU_TAMPERED)
                ret |= AuxiliariesCapabilities.VandalShieldCapabilities.Tampered;
            return ret;
        }

        public static AuxiliariesCapabilities.DoorCapabilities ToDoor(WORD val)
        {
            AuxiliariesCapabilities.DoorCapabilities ret = 0;
            if ((val & WFS_SIU_CLOSED) == WFS_SIU_CLOSED)
                ret |= AuxiliariesCapabilities.DoorCapabilities.Closed;
            if ((val & WFS_SIU_OPEN) == WFS_SIU_OPEN)
                ret |= AuxiliariesCapabilities.DoorCapabilities.Open;
            if ((val & WFS_SIU_LOCKED) == WFS_SIU_LOCKED)
                ret |= AuxiliariesCapabilities.DoorCapabilities.Locked;
            if ((val & WFS_SIU_BOLTED) == WFS_SIU_BOLTED)
                ret |= AuxiliariesCapabilities.DoorCapabilities.Bolted;
            if ((val & WFS_SIU_TAMPERED) == WFS_SIU_TAMPERED)
                ret |= AuxiliariesCapabilities.DoorCapabilities.Tampered;
            return ret;
        }

        public static AuxiliariesCapabilities.UpsEnum ToUps(WORD val)
        {
            AuxiliariesCapabilities.UpsEnum ret = AuxiliariesCapabilities.UpsEnum.NotAvailable;
            if ((val & WFS_SIU_LOW) == WFS_SIU_LOW)
                ret |= AuxiliariesCapabilities.UpsEnum.Low;
            if ((val & WFS_SIU_ENGAGED) == WFS_SIU_ENGAGED)
                ret |= AuxiliariesCapabilities.UpsEnum.Engaged;
            if ((val & WFS_SIU_POWERING) == WFS_SIU_POWERING)
                ret |= AuxiliariesCapabilities.UpsEnum.Powering;
            if ((val & WFS_SIU_RECOVERED) == WFS_SIU_RECOVERED)
                ret |= AuxiliariesCapabilities.UpsEnum.Recovered;

            return ret;
        }

        public static AuxiliariesCapabilities.EnhancedAudioControlEnum ToEnhancedAudioControl(WORD val)
        {
            AuxiliariesCapabilities.EnhancedAudioControlEnum ret = AuxiliariesCapabilities.EnhancedAudioControlEnum.NotAvailable;
            if ((val & WFS_SIU_HEADSET_DETECTION) == WFS_SIU_HEADSET_DETECTION)
                ret |= AuxiliariesCapabilities.EnhancedAudioControlEnum.HeadsetDetection;
            if ((val & WFS_SIU_MODE_CONTROLLABLE) == WFS_SIU_MODE_CONTROLLABLE)
                ret |= AuxiliariesCapabilities.EnhancedAudioControlEnum.ModeControllable;

            return ret;
        }
    }

    public static class WSensorIndices
    {
        #pragma warning disable format
 		/* Indices of WFSSIUSTATUS.fwSensors [...]
          WFSSIUCAPS.fwSensors [...]
          WFSSIUENABLE.fwSensors [...]
          WFSSIUPORTEVENT.wPortIndex
          WFSSIUPORTERROR.wPortIndex */

        public const WORD WFS_SIU_OPERATORSWITCH              = (0);
        public const WORD WFS_SIU_TAMPER                      = (1);
        public const WORD WFS_SIU_INTTAMPER                   = (2);
        public const WORD WFS_SIU_SEISMIC                     = (3);
        public const WORD WFS_SIU_HEAT                        = (4);
        public const WORD WFS_SIU_PROXIMITY                   = (5);
        public const WORD WFS_SIU_AMBLIGHT                    = (6);
        public const WORD WFS_SIU_ENHANCEDAUDIO               = (7);
        public const WORD WFS_SIU_BOOT_SWITCH                 = (8);
        public const WORD WFS_SIU_CONSUMER_DISPLAY            = (9);
        public const WORD WFS_SIU_OPERATOR_CALL_BUTTON        = (10);
        public const WORD WFS_SIU_HANDSETSENSOR               = (11);
        public const WORD WFS_SIU_GENERALINPUTPORT            = (12);
        public const WORD WFS_SIU_HEADSETMICROPHONE           = (13);
        public const WORD WFS_SIU_FASCIAMICROPHONE            = (14);

        #pragma warning restore format

    }

    public static class WIndicatorIndices
    {
        #pragma warning disable format
                     /* Indices of WFSSIUSTATUS.fwIndicators [...]
                                WFSSIUCAPS.fwIndicators [...]
                                WFSSIUENABLE.fwIndicators [...]
                                WFSSIUSETPORTS.wIndicators [...]
                                WFSSIUSETINDICATOR.wIndicator
                                WFSSIUPORTEVENT.wPortIndex
                                WFSSIUPORTERROR.wPortIndex */

        public const WORD WFS_SIU_OPENCLOSE                   = (0);
        public const WORD WFS_SIU_FASCIALIGHT                 = (1);
        public const WORD WFS_SIU_AUDIO                       = (2);
        public const WORD WFS_SIU_HEATING                     = (3);
        public const WORD WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT  = (4);
        public const WORD WFS_SIU_SIGNAGEDISPLAY              = (5);
        public const WORD WFS_SIU_TRANSINDICATOR              = (6);
        public const WORD WFS_SIU_GENERALOUTPUTPORT           = (7);
        #pragma warning restore format

    }

    public static class WAuxiliaryIndices
    {
        #pragma warning disable format
        /* Indices of WFSSIUSTATUS.fwAuxiliaries [...]
            WFSSIUCAPS.fwAuxiliaries [...]
            WFSSIUENABLE.fwAuxiliaries [...]
            WFSSIUSETPORTS.wAuxiliaries [...]
            WFSSIUSETAUXILIARY.wAuxiliary
            WFSSIUPORTEVENT.wPortIndex
            WFSSIUPORTERROR.wPortIndex */

        public const WORD WFS_SIU_VOLUME                      = (0);
        public const WORD WFS_SIU_UPS                         = (1);
        public const WORD WFS_SIU_REMOTE_STATUS_MONITOR       = (2);
        public const WORD WFS_SIU_AUDIBLE_ALARM               = (3);
        public const WORD WFS_SIU_ENHANCEDAUDIOCONTROL        = (4);
        public const WORD WFS_SIU_ENHANCEDMICROPHONECONTROL   = (5);
        public const WORD WFS_SIU_MICROPHONEVOLUME            = (6);
        #pragma warning restore format

    }

    public static class WDoorIndices
    {
        #pragma warning disable format
          /* Indices of WFSSIUSTATUS.fwDoors [...]
                     WFSSIUCAPS.fwDoors [...]
                     WFSSIUENABLE.fwDoors [...]
                     WFSSIUSETPORTS.fwDoors [...]
                     WFSSIUSETDOOR.wDoor
                     WFSSIUPORTEVENT.wPortIndex
                     WFSSIUPORTERROR.wPortIndex */

        public const WORD WFS_SIU_CABINET                     = (0);
        public const WORD WFS_SIU_SAFE                        = (1);
        public const WORD WFS_SIU_VANDALSHIELD                = (2);
        public const WORD WFS_SIU_CABINET_FRONT               = (3);
        public const WORD WFS_SIU_CABINET_REAR                = (4);
        public const WORD WFS_SIU_CABINET_LEFT                = (5);
        public const WORD WFS_SIU_CABINET_RIGHT               = (6);
        #pragma warning restore format

        public static AuxiliariesCapabilities.DoorType ToDoorType(WORD val) => val switch
        {
            WFS_SIU_CABINET => AuxiliariesCapabilities.DoorType.FrontCabinet,
            WFS_SIU_SAFE => AuxiliariesCapabilities.DoorType.Safe,
            //WFS_SIU_VANDALSHIELD => AuxiliariesCapabilities.DoorType,
            WFS_SIU_CABINET_FRONT => AuxiliariesCapabilities.DoorType.FrontCabinet,
            WFS_SIU_CABINET_REAR => AuxiliariesCapabilities.DoorType.RearCabinet,
            WFS_SIU_CABINET_LEFT => AuxiliariesCapabilities.DoorType.LeftCabinet,
            WFS_SIU_CABINET_RIGHT => AuxiliariesCapabilities.DoorType.RightCabinet,
            _ => throw new UnknowConstException(val, typeof(WDoorIndices))
        };

        public static WORD FromDoorType(AuxiliariesCapabilities.DoorType val) => val switch
        {
            AuxiliariesCapabilities.DoorType.Safe => WFS_SIU_SAFE,
            AuxiliariesCapabilities.DoorType.FrontCabinet => WFS_SIU_CABINET_FRONT,
            AuxiliariesCapabilities.DoorType.RearCabinet => WFS_SIU_CABINET_REAR,
            AuxiliariesCapabilities.DoorType.LeftCabinet => WFS_SIU_CABINET_LEFT,
            AuxiliariesCapabilities.DoorType.RightCabinet => WFS_SIU_CABINET_RIGHT,
            _ => throw new UnknowEnumException(val, typeof(WDoorIndices))
        };

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

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSSIUENABLE
    {
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

        public static CommandData BuildCommandData()
        {
            WFSSIUENABLE dataCommand = new()
            {
                fwSensors = new WORD[DEF.WFS_SIU_SENSORS_SIZE],
                fwDoors = new WORD[DEF.WFS_SIU_DOORS_SIZE],
                fwIndicators = new WORD[DEF.WFS_SIU_INDICATORS_SIZE],
                fwAuxiliaries = new WORD[DEF.WFS_SIU_AUXILIARIES_SIZE],
                fwGuidLights = new WORD[DEF.WFS_SIU_GUIDLIGHTS_SIZE],
                lpszExtra = IntPtr.Zero,
            };

            Array.Fill(dataCommand.fwSensors, DEF.WFS_SIU_ENABLE_EVENT);
            Array.Fill(dataCommand.fwDoors, DEF.WFS_SIU_ENABLE_EVENT);
            Array.Fill(dataCommand.fwIndicators, DEF.WFS_SIU_DISABLE_EVENT);
            Array.Fill(dataCommand.fwAuxiliaries, DEF.WFS_SIU_ENABLE_EVENT);
            Array.Fill(dataCommand.fwGuidLights, DEF.WFS_SIU_DISABLE_EVENT);

            return CommandData.FromStructureNoIntPtr(ref dataCommand);
        }

        public static CommandData BuildCommandData(ref WFSSIUCAPS300 caps)
        {
            WFSSIUENABLE dataCommand = new()
            {
                fwSensors = new WORD[DEF.WFS_SIU_SENSORS_SIZE],
                fwDoors = new WORD[DEF.WFS_SIU_DOORS_SIZE],
                fwIndicators = new WORD[DEF.WFS_SIU_INDICATORS_SIZE],
                fwAuxiliaries = new WORD[DEF.WFS_SIU_AUXILIARIES_SIZE],
                fwGuidLights = new WORD[DEF.WFS_SIU_GUIDLIGHTS_SIZE],
                lpszExtra = IntPtr.Zero,
            };

            //Array.Fill(dataCommand.fwSensors, DEF.WFS_SIU_ENABLE_EVENT);
            SetEnable(ref dataCommand.fwSensors, ref caps.fwSensors);
            //Array.Fill(dataCommand.fwDoors, DEF.WFS_SIU_ENABLE_EVENT);
            SetEnable(ref dataCommand.fwDoors, ref caps.fwDoors);
            Array.Fill(dataCommand.fwIndicators, DEF.WFS_SIU_NO_CHANGE);
            //Array.Fill(dataCommand.fwAuxiliaries, DEF.WFS_SIU_ENABLE_EVENT);
            SetEnable(ref dataCommand.fwAuxiliaries, ref caps.fwAuxiliaries);
            Array.Fill(dataCommand.fwGuidLights, DEF.WFS_SIU_NO_CHANGE);

            return CommandData.FromStructureNoIntPtr(ref dataCommand);
        }

        private static void SetEnable(ref WORD[] arr, ref WORD[] caps)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (caps[i] == DEF.WFS_SIU_NOT_AVAILABLE)
                {
                    arr[i] = DEF.WFS_SIU_NO_CHANGE;
                }
                else
                {
                    arr[i] = DEF.WFS_SIU_ENABLE_EVENT;
                }
            }
        }
    }

}