using System.Runtime.InteropServices;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Printer;
using BOOL = System.Int32;
using DWORD = System.UInt32;
using Form = XFS4IoTFramework.Printer.Form;
using LPBYTE = System.IntPtr;
using LPDWORD = System.IntPtr;
using LPSTR = System.IntPtr;
using LPUSHORT = System.IntPtr;
using LPWFSPTRRETRACTBINS = System.IntPtr;
using LPWSTR = System.IntPtr;
using ULONG = System.UInt32;
using USHORT = System.UInt16;
using WORD = System.UInt16;

namespace XFS3xAPI.PTR
{
    public static class PTRExtension
    {
        public static WFSFRMHEADER ReadFrmHeader(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSFRMHEADER>(wfsResult.lpBuffer);
        public static WFSFRMMEDIA ReadMedia(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSFRMMEDIA>(wfsResult.lpBuffer);
        public static List<WFSFRMFIELD> ReadFormFields(this WFSRESULT wfsResult) => ResultData.ReadArrayStructure<WFSFRMFIELD>(wfsResult.lpBuffer);
        public static WFSPTRRAWDATAIN ReadRawDataIn(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSPTRRAWDATAIN>(wfsResult.lpBuffer);
        public static WFSPTRSTATUS_300 ReadPTRStatus300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSPTRSTATUS_300>(wfsResult.lpBuffer);
        public static WFSPTRSTATUS ReadPTRStatus(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSPTRSTATUS>(wfsResult.lpBuffer);
        public static WFSPTRCAPS300 ReadPTRCaps300(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSPTRCAPS300>(wfsResult.lpBuffer);
        public static WFSPTRCAPS ReadPTRCaps(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSPTRCAPS>(wfsResult.lpBuffer);
    }
    internal static class CLASS
    {
        #pragma warning disable format
        /* value of WFSPTRCAPS.wClass */
        public const DWORD WFS_SERVICE_CLASS_PTR            = (1);
        public const DWORD WFS_SERVICE_CLASS_VERSION_PTR    = (0x2803); /* Version 3.40 */
        public const string WFS_SERVICE_CLASS_NAME_PTR      = "PTR";

        public const DWORD PTR_SERVICE_OFFSET = (WFS_SERVICE_CLASS_PTR * 100);
        #pragma warning restore format
    }

    public static class DEF
    {
        #pragma warning disable format
        public const int WFS_PTR_SUPPLYSIZE         = (16);
        public const int WFS_PTR_SUPPLYMAX          = (WFS_PTR_SUPPLYSIZE - 1);
        public const int WFS_PTR_GUIDLIGHTS_SIZE    = (32);
        public const int WFS_PTR_GUIDLIGHTS_MAX     = (WFS_PTR_GUIDLIGHTS_SIZE - 1);
        #pragma warning restore format
    }

    public static class CMD
    {
        #pragma warning disable format
        public const DWORD PTR_SERVICE_OFFSET = CLASS.PTR_SERVICE_OFFSET;
        /* PTR Info Commands */

        public const DWORD WFS_INF_PTR_STATUS                = (PTR_SERVICE_OFFSET + 1);
        public const DWORD WFS_INF_PTR_CAPABILITIES          = (PTR_SERVICE_OFFSET + 2);
        public const DWORD WFS_INF_PTR_FORM_LIST             = (PTR_SERVICE_OFFSET + 3);
        public const DWORD WFS_INF_PTR_MEDIA_LIST            = (PTR_SERVICE_OFFSET + 4);
        public const DWORD WFS_INF_PTR_QUERY_FORM            = (PTR_SERVICE_OFFSET + 5);
        public const DWORD WFS_INF_PTR_QUERY_MEDIA           = (PTR_SERVICE_OFFSET + 6);
        public const DWORD WFS_INF_PTR_QUERY_FIELD           = (PTR_SERVICE_OFFSET + 7);
        public const DWORD WFS_INF_PTR_CODELINE_MAPPING      = (PTR_SERVICE_OFFSET + 8);

        /* PTR Execute Commands */

        public const DWORD WFS_CMD_PTR_CONTROL_MEDIA         = (PTR_SERVICE_OFFSET + 1);
        public const DWORD WFS_CMD_PTR_PRINT_FORM            = (PTR_SERVICE_OFFSET + 2);
        public const DWORD WFS_CMD_PTR_READ_FORM             = (PTR_SERVICE_OFFSET + 3);
        public const DWORD WFS_CMD_PTR_RAW_DATA              = (PTR_SERVICE_OFFSET + 4);
        public const DWORD WFS_CMD_PTR_MEDIA_EXTENTS         = (PTR_SERVICE_OFFSET + 5);
        public const DWORD WFS_CMD_PTR_RESET_COUNT           = (PTR_SERVICE_OFFSET + 6);
        public const DWORD WFS_CMD_PTR_READ_IMAGE            = (PTR_SERVICE_OFFSET + 7);
        public const DWORD WFS_CMD_PTR_RESET                 = (PTR_SERVICE_OFFSET + 8);
        public const DWORD WFS_CMD_PTR_RETRACT_MEDIA         = (PTR_SERVICE_OFFSET + 9);
        public const DWORD WFS_CMD_PTR_DISPENSE_PAPER        = (PTR_SERVICE_OFFSET + 10);
        public const DWORD WFS_CMD_PTR_SET_GUIDANCE_LIGHT    = (PTR_SERVICE_OFFSET + 11);
        public const DWORD WFS_CMD_PTR_PRINT_RAW_FILE        = (PTR_SERVICE_OFFSET + 12);
        public const DWORD WFS_CMD_PTR_LOAD_DEFINITION       = (PTR_SERVICE_OFFSET + 13);
        public const DWORD WFS_CMD_PTR_SUPPLY_REPLENISH      = (PTR_SERVICE_OFFSET + 14);
        public const DWORD WFS_CMD_PTR_POWER_SAVE_CONTROL    = (PTR_SERVICE_OFFSET + 15);
        public const DWORD WFS_CMD_PTR_CONTROL_PASSBOOK      = (PTR_SERVICE_OFFSET + 16);
        public const DWORD WFS_CMD_PTR_SET_BLACK_MARK_MODE   = (PTR_SERVICE_OFFSET + 17);
        public const DWORD WFS_CMD_PTR_SYNCHRONIZE_COMMAND   = (PTR_SERVICE_OFFSET + 18);

        #pragma warning restore format
    }

    public static class EVENT
    {
        #pragma warning disable format
        public const DWORD PTR_SERVICE_OFFSET = CLASS.PTR_SERVICE_OFFSET;
        /* PTR Messages */

        public const DWORD WFS_EXEE_PTR_NOMEDIA              = (PTR_SERVICE_OFFSET + 1);
        public const DWORD WFS_EXEE_PTR_MEDIAINSERTED        = (PTR_SERVICE_OFFSET + 2);
        public const DWORD WFS_EXEE_PTR_FIELDERROR           = (PTR_SERVICE_OFFSET + 3);
        public const DWORD WFS_EXEE_PTR_FIELDWARNING         = (PTR_SERVICE_OFFSET + 4);
        public const DWORD WFS_USRE_PTR_RETRACTBINTHRESHOLD  = (PTR_SERVICE_OFFSET + 5);
        public const DWORD WFS_SRVE_PTR_MEDIATAKEN           = (PTR_SERVICE_OFFSET + 6);
        public const DWORD WFS_USRE_PTR_PAPERTHRESHOLD       = (PTR_SERVICE_OFFSET + 7);
        public const DWORD WFS_USRE_PTR_TONERTHRESHOLD       = (PTR_SERVICE_OFFSET + 8);
        public const DWORD WFS_SRVE_PTR_MEDIAINSERTED        = (PTR_SERVICE_OFFSET + 9);
        public const DWORD WFS_USRE_PTR_LAMPTHRESHOLD        = (PTR_SERVICE_OFFSET + 10);
        public const DWORD WFS_USRE_PTR_INKTHRESHOLD         = (PTR_SERVICE_OFFSET + 11);
        public const DWORD WFS_SRVE_PTR_MEDIADETECTED        = (PTR_SERVICE_OFFSET + 12);
        public const DWORD WFS_SRVE_PTR_RETRACTBINSTATUS     = (PTR_SERVICE_OFFSET + 13);
        public const DWORD WFS_EXEE_PTR_MEDIAPRESENTED       = (PTR_SERVICE_OFFSET + 14);
        public const DWORD WFS_SRVE_PTR_DEFINITIONLOADED     = (PTR_SERVICE_OFFSET + 15);
        public const DWORD WFS_EXEE_PTR_MEDIAREJECTED        = (PTR_SERVICE_OFFSET + 16);
        public const DWORD WFS_SRVE_PTR_MEDIAPRESENTED       = (PTR_SERVICE_OFFSET + 17);
        public const DWORD WFS_SRVE_PTR_MEDIAAUTORETRACTED   = (PTR_SERVICE_OFFSET + 18);
        public const DWORD WFS_SRVE_PTR_DEVICEPOSITION       = (PTR_SERVICE_OFFSET + 19);
        public const DWORD WFS_SRVE_PTR_POWER_SAVE_CHANGE    = (PTR_SERVICE_OFFSET + 20);
        #pragma warning restore format
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSPTRSTATUS
    {
        WORD fwDevice;
        WORD fwMedia;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_SUPPLYSIZE)]
        WORD[] fwPaper;
        WORD fwToner;
        WORD fwInk;
        WORD fwLamp;
        LPWFSPTRRETRACTBINS lppRetractBins;
        USHORT usMediaOnStacker;
        LPSTR lpszExtra;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_GUIDLIGHTS_SIZE)]
        DWORD[] dwGuidLights;
        WORD wDevicePosition;
        USHORT usPowerSaveRecoveryTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_SUPPLYSIZE)]
        WORD[] wPaperType;
        WORD wAntiFraudModule;
        WORD wBlackMarkMode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSPTRSTATUS_300
    {
        public WORD fwDevice;
        public WORD fwMedia;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_SUPPLYSIZE)]
        public WORD[] fwPaper;
        public WORD fwToner;
        public WORD fwInk;
        public WORD fwLamp;
        LPWFSPTRRETRACTBINS lppRetractBins;
        public USHORT usMediaOnStacker;
        LPSTR lpszExtra;

        /*
         * 3.40
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_GUIDLIGHTS_SIZE)]
        public DWORD[] dwGuidLights;
        public WORD wDevicePosition;
        USHORT usPowerSaveRecoveryTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_SUPPLYSIZE)]
        WORD[] wPaperType;
        WORD wAntiFraudModule;
        WORD wBlackMarkMode;
        */

        public void ReadPaper(Dictionary<PrinterStatusClass.PaperSourceEnum, PrinterStatusClass.SupplyStatusClass> paper,
            Dictionary<string, PrinterStatusClass.SupplyStatusClass> custom)
        {
            for (int i = 0; i < DEF.WFS_PTR_SUPPLYSIZE; i++)
            {
                if (FwPaper.IsCustom(i))
                {
                    if (fwPaper[i] != FwPaper.WFS_PTR_PAPERNOTSUPP)
                    {
                        var customName = $"custom_{i}";
                        custom.Add(customName, new(FwPaper.ToEnum(fwPaper[i]), PrinterStatusClass.PaperTypeEnum.Unknown));
                    }
                }
                else
                {
                    paper.Add(FwPaper.ToSourceEnum(i),
                        new(FwPaper.ToEnum(fwPaper[i]), PrinterStatusClass.PaperTypeEnum.Unknown));
                }
            }
        }

        public void ReadRestactBins(List<PrinterStatusClass.RetractBinsClass> retractBins)
        {
            List<WFSPTRRETRACTBINS> listRetractBins = ResultData.ReadArrayStructure<WFSPTRRETRACTBINS>(lppRetractBins);
            foreach (var item in listRetractBins)
            {
                retractBins.Add(item.ToRetractBins());
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSPTRCAPS
    {
        WORD wClass;
        WORD fwType;
        BOOL bCompound;
        WORD wResolution;
        WORD fwReadForm;
        WORD fwWriteForm;
        WORD fwExtents;
        WORD fwControl;
        USHORT usMaxMediaOnStacker;
        BOOL bAcceptMedia;
        BOOL bMultiPage;
        WORD fwPaperSources;
        BOOL bMediaTaken;
        USHORT usRetractBins;
        LPUSHORT lpusMaxRetract;
        WORD fwImageType;
        WORD fwFrontImageColorFormat;
        WORD fwBackImageColorFormat;
        WORD fwCodelineFormat;
        WORD fwImageSource;
        WORD fwCharSupport;
        BOOL bDispensePaper;
        LPSTR lpszExtra;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_GUIDLIGHTS_SIZE)]
        DWORD[] dwGuidLights;
        LPSTR lpszWindowsPrinter;
        BOOL bMediaPresented;
        USHORT usAutoRetractPeriod;
        BOOL bRetractToTransport;
        BOOL bPowerSaveControl;
        WORD fwCoercivityType;
        WORD fwControlPassbook;
        WORD wPrintSides;
        BOOL bAntiFraudModule;
        DWORD dwControlEx;
        BOOL bBlackMarkModeSupported;
        LPDWORD lpdwSynchronizableCommands;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct WFSPTRCAPS300
    {
        public WORD wClass;
        public WORD fwType;
        public BOOL bCompound;
        public WORD wResolution;
        public WORD fwReadForm;
        public WORD fwWriteForm;
        public WORD fwExtents;
        public WORD fwControl;
        public USHORT usMaxMediaOnStacker;
        public BOOL bAcceptMedia;
        public BOOL bMultiPage;
        public WORD fwPaperSources;
        public BOOL bMediaTaken;
        public USHORT usRetractBins;
        LPUSHORT lpusMaxRetract;
        public WORD fwImageType;
        public WORD fwFrontImageColorFormat;
        public WORD fwBackImageColorFormat;
        public WORD fwCodelineFormat;
        public WORD fwImageSource;
        public WORD fwCharSupport;
        public BOOL bDispensePaper;
        LPSTR lpszExtra;
        /* 3.40
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = DEF.WFS_PTR_GUIDLIGHTS_SIZE)]
        DWORD[] dwGuidLights;
        LPSTR lpszWindowsPrinter;
        BOOL bMediaPresented;
        USHORT usAutoRetractPeriod;
        BOOL bRetractToTransport;
        BOOL bPowerSaveControl;
        WORD fwCoercivityType;
        WORD fwControlPassbook;
        WORD wPrintSides;
        BOOL bAntiFraudModule;
        DWORD dwControlEx;
        BOOL bBlackMarkModeSupported;
        LPDWORD lpdwSynchronizableCommands;
        */

        public List<int> ReadRetractBins()
        {
            List<USHORT> listUshort = ResultData.ReadArrayStructure<USHORT>(lpusMaxRetract);
            return listUshort.Cast<int>().ToList();
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRRETRACTBINS
    {
        [FieldOffset(0)] WORD wRetractBin;
        [FieldOffset(2)] USHORT usRetractCount;

        public PrinterStatusClass.RetractBinsClass ToRetractBins()
        {
            return new PrinterStatusClass.RetractBinsClass(WRetractBin.ToEnum(wRetractBin), usRetractCount);
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSFRMHEADER
    {
        [FieldOffset(0)] LPSTR lpszFormName;
        [FieldOffset(4)] WORD wBase;
        [FieldOffset(6)] WORD wUnitX;
        [FieldOffset(8)] WORD wUnitY;
        [FieldOffset(10)] WORD wWidth;
        [FieldOffset(12)] WORD wHeight;
        [FieldOffset(14)] WORD wAlignment;
        [FieldOffset(16)] WORD wOrientation;
        [FieldOffset(18)] WORD wOffsetX;
        [FieldOffset(20)] WORD wOffsetY;
        [FieldOffset(22)] WORD wVersionMajor;
        [FieldOffset(24)] WORD wVersionMinor;
        [FieldOffset(26)] LPSTR lpszUserPrompt;
        [FieldOffset(30)] WORD fwCharSupport;
        [FieldOffset(32)] LPSTR lpszFields;
        [FieldOffset(36)] WORD wLanguageID;

        private string? FormName => ResultData.ReadLPSTR(lpszFormName);
        private string? UserPrompt => ResultData.ReadLPSTR(lpszFormName);
        //private string? FormName => ResultData.ReadLPSTR(lpszFormName);

        public Form ToForm(IPrinterDevice Device)
        {
            return new Form(null, Device, FormName, WBase.ToFormEnum(wBase),
                wUnitX, wUnitY, wWidth, wHeight, wOffsetX, wOffsetY,
                wVersionMajor, wVersionMinor, "Data", "Auto", "Copyright", "Title", "Comment",
                UserPrompt, 0, WAlignment.ToEnum(wAlignment), WOrientation.ToEnum(wOrientation));
        }
    }
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSFRMMEDIA
    {
        [FieldOffset(0)] WORD fwMediaType;
        [FieldOffset(2)] WORD wBase;
        [FieldOffset(4)] WORD wUnitX;
        [FieldOffset(6)] WORD wUnitY;
        [FieldOffset(8)] WORD wSizeWidth;
        [FieldOffset(10)] WORD wSizeHeight;
        [FieldOffset(12)] WORD wPageCount;
        [FieldOffset(14)] WORD wLineCount;
        [FieldOffset(16)] WORD wPrintAreaX;
        [FieldOffset(18)] WORD wPrintAreaY;
        [FieldOffset(20)] WORD wPrintAreaWidth;
        [FieldOffset(22)] WORD wPrintAreaHeight;
        [FieldOffset(24)] WORD wRestrictedAreaX;
        [FieldOffset(26)] WORD wRestrictedAreaY;
        [FieldOffset(28)] WORD wRestrictedAreaWidth;
        [FieldOffset(30)] WORD wRestrictedAreaHeight;
        [FieldOffset(32)] WORD wStagger;
        [FieldOffset(34)] WORD wFoldType;
        [FieldOffset(36)] WORD wPaperSources;

        public Media ToMedia(string name, IPrinterDevice Device)
        {
            return new Media(null, Device, name, FwMediaType.ToEnum(fwMediaType), WPaperSources.ToEnum(wPaperSources), WBase.ToMediaEnum(wBase),
                wUnitX, wUnitY, wSizeWidth, wSizeHeight, wPrintAreaX, wPrintAreaY, wPrintAreaWidth, wPrintAreaHeight,
                wRestrictedAreaX, wRestrictedAreaY, wRestrictedAreaWidth, wRestrictedAreaHeight,
                WFoldType.ToEnum(wFoldType), wStagger, wPageCount, wLineCount);
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSFRMFIELD
    {
        [FieldOffset(0)] LPSTR lpszFieldName;
        [FieldOffset(4)] WORD wIndexCount;
        [FieldOffset(6)] WORD fwType;
        [FieldOffset(8)] WORD fwClass;
        [FieldOffset(10)] WORD fwAccess;
        [FieldOffset(12)] WORD fwOverflow;
        [FieldOffset(14)] LPSTR lpszInitialValue;
        [FieldOffset(18)] LPWSTR lpszUNICODEInitialValue;
        [FieldOffset(22)] LPSTR lpszFormat;
        [FieldOffset(26)] LPWSTR lpszUNICODEFormat;
        [FieldOffset(30)] WORD wLanguageID;

        private string? FieldName => ResultData.ReadLPSTR(lpszFieldName);
        private string? Format
        {
            get
            {
                string? format = ResultData.ReadLPSTR(lpszFormat);
                if (format == null && fwType == FwFieldType.WFS_FRM_FIELDGRAPHIC)
                {
                    return "BMP";
                }
                else
                {
                    return format;
                }
            }
        }
        private string? InitialValue => ResultData.ReadLPSTR(lpszInitialValue);

        public FormField ToField()
        {
            return new FormField(FieldName, 0, 0, 0, 0, 0, 0, 0, 0, null, wIndexCount, 0, 0, 0, 0, null, -1, -1, -1,
                Format, InitialValue, FieldSideEnum.FRONT, FwFieldType.ToEnum(fwType), FwClass.ToEnum(fwClass),
                FwAccess.ToEnum(fwAccess), FwOverflow.ToEnum(fwOverflow),
                FieldStyleEnum.NORMAL, FormField.CaseEnum.NOCHANGE, FormField.HorizontalEnum.LEFT, FormField.VerticalEnum.CENTER,
                FieldColorEnum.BLACK, FieldScalingEnum.BESTFIT, FieldBarcodeEnum.NONE);
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRRAWDATAIN
    {
        [FieldOffset(0)] ULONG ulSize;
        [FieldOffset(4)] LPBYTE lpbData;

        public List<byte> Data => ResultData.ReadRawData(lpbData, (int)ulSize);
    }

    /* values of WFSPTRSTATUS.fwDevice */
    public static class FwDevice
    {
            #pragma warning disable format
            public const WORD WFS_PTR_DEVONLINE             = WFSDEVSTATUS.FwState.WFS_STAT_DEVONLINE;
            public const WORD WFS_PTR_DEVOFFLINE            = WFSDEVSTATUS.FwState.WFS_STAT_DEVOFFLINE;
            public const WORD WFS_PTR_DEVPOWEROFF           = WFSDEVSTATUS.FwState.WFS_STAT_DEVPOWEROFF;
            public const WORD WFS_PTR_DEVNODEVICE           = WFSDEVSTATUS.FwState.WFS_STAT_DEVNODEVICE;
            public const WORD WFS_PTR_DEVHWERROR            = WFSDEVSTATUS.FwState.WFS_STAT_DEVHWERROR;
            public const WORD WFS_PTR_DEVUSERERROR          = WFSDEVSTATUS.FwState.WFS_STAT_DEVUSERERROR;
            public const WORD WFS_PTR_DEVBUSY               = WFSDEVSTATUS.FwState.WFS_STAT_DEVBUSY;
            public const WORD WFS_PTR_DEVFRAUDATTEMPT       = WFSDEVSTATUS.FwState.WFS_STAT_DEVFRAUDATTEMPT;
            public const WORD WFS_PTR_DEVPOTENTIALFRAUD     = WFSDEVSTATUS.FwState.WFS_STAT_DEVPOTENTIALFRAUD;
            #pragma warning restore format
            public static string ToString(WORD state) => state switch
            {
                WFS_PTR_DEVONLINE => "WFS_PTR_DEVONLINE",
                WFS_PTR_DEVOFFLINE => "WFS_PTR_DEVOFFLINE",
                WFS_PTR_DEVPOWEROFF => "WFS_PTR_DEVPOWEROFF",
                WFS_PTR_DEVNODEVICE => "WFS_PTR_DEVNODEVICE",
                WFS_PTR_DEVHWERROR => "WFS_PTR_DEVHWERROR",
                WFS_PTR_DEVUSERERROR => "WFS_PTR_DEVUSERERROR",
                WFS_PTR_DEVBUSY => "WFS_PTR_DEVBUSY",
                WFS_PTR_DEVFRAUDATTEMPT => "WFS_PTR_DEVFRAUDATTEMPT",
                WFS_PTR_DEVPOTENTIALFRAUD => "WFS_PTR_DEVPOTENTIALFRAUD",
                _ => throw new UnknowConstException(state, typeof(FwDevice))
            };

        public static CommonStatusClass.DeviceEnum ToEnum(WORD state) => WFSDEVSTATUS.FwState.ToDeviceEnum(state);
    }

    /* values of WFSPTRSTATUS.wDevicePosition
             WFSPTRDEVICEPOSITION.wPosition */
    public static class WDevicePosition
    {
        #pragma warning disable format
        public const WORD WFS_PTR_DEVICEINPOSITION            = (0);
        public const WORD WFS_PTR_DEVICENOTINPOSITION         = (1);
        public const WORD WFS_PTR_DEVICEPOSUNKNOWN            = (2);
        public const WORD WFS_PTR_DEVICEPOSNOTSUPP            = (3);
        #pragma warning restore format

        public static CommonStatusClass.PositionStatusEnum ToEnum(WORD pos) => pos switch
        {
            WFS_PTR_DEVICEINPOSITION => CommonStatusClass.PositionStatusEnum.InPosition,
            WFS_PTR_DEVICENOTINPOSITION => CommonStatusClass.PositionStatusEnum.NotInPosition,
            WFS_PTR_DEVICEPOSUNKNOWN => CommonStatusClass.PositionStatusEnum.Unknown,
            WFS_PTR_DEVICEPOSNOTSUPP => CommonStatusClass.PositionStatusEnum.Unknown,
            _ => throw new UnknowConstException(pos, typeof(WDevicePosition))
        };
    }

    /* values of WFSPTRSTATUS.fwMedia and
             WFSPTRMEDIADETECTED.wPosition */
    public static class FwMedia
    {
        #pragma warning disable format
        public const WORD WFS_PTR_MEDIAPRESENT              = (0);
        public const WORD WFS_PTR_MEDIANOTPRESENT           = (1);
        public const WORD WFS_PTR_MEDIAJAMMED               = (2);
        public const WORD WFS_PTR_MEDIANOTSUPP              = (3);
        public const WORD WFS_PTR_MEDIAUNKNOWN              = (4);
        public const WORD WFS_PTR_MEDIAENTERING             = (5);
        public const WORD WFS_PTR_MEDIARETRACTED            = (6);
        #pragma warning restore format

        public static PrinterStatusClass.MediaEnum ToEnum(WORD pos) => pos switch
        {
            WFS_PTR_MEDIAPRESENT => PrinterStatusClass.MediaEnum.Present,
            WFS_PTR_MEDIANOTPRESENT => PrinterStatusClass.MediaEnum.NotPresent,
            WFS_PTR_MEDIAJAMMED => PrinterStatusClass.MediaEnum.Jammed,
            WFS_PTR_MEDIANOTSUPP => PrinterStatusClass.MediaEnum.NotSupported,
            WFS_PTR_MEDIAUNKNOWN => PrinterStatusClass.MediaEnum.Unknown,
            WFS_PTR_MEDIAENTERING => PrinterStatusClass.MediaEnum.Entering,
            WFS_PTR_MEDIARETRACTED => PrinterStatusClass.MediaEnum.Retracted,
            _ => throw new UnknowConstException(pos, typeof(WDevicePosition))
        };
    }

    /* values of WFSPTRSTATUS.fwPaper and
         WFSPTRPAPERTHRESHOLD.wPaperThreshold */
    public static class FwPaper
    {
        #pragma warning disable format
        /* Indices of WFSPTRSTATUS.fwPaper [...] */
        public const int WFS_PTR_SUPPLYUPPER        = (0);
        public const int WFS_PTR_SUPPLYLOWER        = (1);
        public const int WFS_PTR_SUPPLYEXTERNAL     = (2);
        public const int WFS_PTR_SUPPLYAUX          = (3);
        public const int WFS_PTR_SUPPLYAUX2         = (4);
        public const int WFS_PTR_SUPPLYPARK         = (5);

        public const WORD WFS_PTR_PAPERFULL     = (0);
        public const WORD WFS_PTR_PAPERLOW      = (1);
        public const WORD WFS_PTR_PAPEROUT      = (2);
        public const WORD WFS_PTR_PAPERNOTSUPP  = (3);
        public const WORD WFS_PTR_PAPERUNKNOWN  = (4);
        public const WORD WFS_PTR_PAPERJAMMED   = (5);
        #pragma warning restore format

        public static bool IsCustom(int idx)
        {
            return idx > WFS_PTR_SUPPLYPARK;
        }

        public static PrinterStatusClass.PaperSourceEnum ToSourceEnum(int idx) => idx switch
        {
            WFS_PTR_SUPPLYUPPER => PrinterStatusClass.PaperSourceEnum.Upper,
            WFS_PTR_SUPPLYLOWER => PrinterStatusClass.PaperSourceEnum.Lower,
            WFS_PTR_SUPPLYEXTERNAL => PrinterStatusClass.PaperSourceEnum.External,
            WFS_PTR_SUPPLYAUX => PrinterStatusClass.PaperSourceEnum.AUX,
            WFS_PTR_SUPPLYAUX2 => PrinterStatusClass.PaperSourceEnum.AUX2,
            WFS_PTR_SUPPLYPARK => PrinterStatusClass.PaperSourceEnum.Park,
            _ => throw new UnknowConstException((DWORD)idx, typeof(FwPaper))
        };

        public static PrinterStatusClass.PaperSupplyEnum ToEnum(WORD val) => val switch
        {
            WFS_PTR_PAPERFULL => PrinterStatusClass.PaperSupplyEnum.Full,
            WFS_PTR_PAPERLOW => PrinterStatusClass.PaperSupplyEnum.Low,
            WFS_PTR_PAPEROUT => PrinterStatusClass.PaperSupplyEnum.Out,
            WFS_PTR_PAPERNOTSUPP => PrinterStatusClass.PaperSupplyEnum.NotSupported,
            WFS_PTR_PAPERUNKNOWN => PrinterStatusClass.PaperSupplyEnum.Unknown,
            WFS_PTR_PAPERJAMMED => PrinterStatusClass.PaperSupplyEnum.Jammed,
            _ => throw new UnknowConstException(val, typeof(FwPaper))
        };
    }

    public static class FwToner
    {
        #pragma warning disable format
        /* values of WFSPTRSTATUS.fwToner */
        public const WORD WFS_PTR_TONERFULL                 = (0);
        public const WORD WFS_PTR_TONERLOW                  = (1);
        public const WORD WFS_PTR_TONEROUT                  = (2);
        public const WORD WFS_PTR_TONERNOTSUPP              = (3);
        public const WORD WFS_PTR_TONERUNKNOWN              = (4);
        #pragma warning restore format

        public static PrinterStatusClass.TonerEnum ToEnum(WORD val) => val switch
        {
            WFS_PTR_TONERFULL => PrinterStatusClass.TonerEnum.Full,
            WFS_PTR_TONERLOW => PrinterStatusClass.TonerEnum.Low,
            WFS_PTR_TONEROUT => PrinterStatusClass.TonerEnum.Out,
            WFS_PTR_TONERNOTSUPP => PrinterStatusClass.TonerEnum.NotSupported,
            WFS_PTR_TONERUNKNOWN => PrinterStatusClass.TonerEnum.Unknown,
            _ => throw new UnknowConstException(val, typeof(FwToner))
        };
    }

    public static class FwInk
    {
        #pragma warning disable format
        /* values of WFSPTRSTATUS.fwToner */
        public const WORD WFS_PTR_INKFULL                 = (0);
        public const WORD WFS_PTR_INKLOW                  = (1);
        public const WORD WFS_PTR_INKOUT                  = (2);
        public const WORD WFS_PTR_INKNOTSUPP              = (3);
        public const WORD WFS_PTR_INKUNKNOWN              = (4);
        #pragma warning restore format

        public static PrinterStatusClass.InkEnum ToEnum(WORD val) => val switch
        {
            WFS_PTR_INKFULL => PrinterStatusClass.InkEnum.Full,
            WFS_PTR_INKLOW => PrinterStatusClass.InkEnum.Low,
            WFS_PTR_INKOUT => PrinterStatusClass.InkEnum.Out,
            WFS_PTR_INKNOTSUPP => PrinterStatusClass.InkEnum.NotSupported,
            WFS_PTR_INKUNKNOWN => PrinterStatusClass.InkEnum.Unknown,
            _ => throw new UnknowConstException(val, typeof(FwInk))
        };
    }

    public static class FwLamp
    {
        #pragma warning disable format
        /* values of WFSPTRSTATUS.fwLamp */
        public const WORD WFS_PTR_LAMPOK                   = (0);
        public const WORD WFS_PTR_LAMPFADING               = (1);
        public const WORD WFS_PTR_LAMPINOP                 = (2);
        public const WORD WFS_PTR_LAMPNOTSUPP              = (3);
        public const WORD WFS_PTR_LAMPUNKNOWN              = (4);
        #pragma warning restore format

        public static PrinterStatusClass.LampEnum ToEnum(WORD val) => val switch
        {
            WFS_PTR_LAMPOK => PrinterStatusClass.LampEnum.Ok,
            WFS_PTR_LAMPFADING => PrinterStatusClass.LampEnum.Fading,
            WFS_PTR_LAMPINOP => PrinterStatusClass.LampEnum.Inop,
            WFS_PTR_LAMPNOTSUPP => PrinterStatusClass.LampEnum.NotSupported,
            WFS_PTR_LAMPUNKNOWN => PrinterStatusClass.LampEnum.Unknown,
            _ => throw new UnknowConstException(val, typeof(FwLamp))
        };
    }

    public static class WRetractBin
    {
        #pragma warning disable format
        /* values of WFSPTRRETRACTBINS.wRetractBin and
                        WFSPTRBINTHRESHOLD.wRetractBin */

       public const WORD WFS_PTR_RETRACTBINOK              = (0);
       public const WORD WFS_PTR_RETRACTBINFULL            = (1);
       public const WORD WFS_PTR_RETRACTNOTSUPP            = (2); /* Deprecated */
       public const WORD WFS_PTR_RETRACTUNKNOWN            = (3);
       public const WORD WFS_PTR_RETRACTBINHIGH            = (4);
        #pragma warning restore format

        public static PrinterStatusClass.RetractBinsClass.StateEnum ToEnum(WORD val) => val switch
        {
            WFS_PTR_RETRACTBINOK => PrinterStatusClass.RetractBinsClass.StateEnum.Ok,
            WFS_PTR_RETRACTBINFULL => PrinterStatusClass.RetractBinsClass.StateEnum.Full,
            // WFS_PTR_RETRACTNOTSUPP => PrinterStatusClass.RetractBinsClass.StateEnum, /* Deprecated */
            WFS_PTR_RETRACTNOTSUPP => PrinterStatusClass.RetractBinsClass.StateEnum.Unknown,
            WFS_PTR_RETRACTUNKNOWN => PrinterStatusClass.RetractBinsClass.StateEnum.Unknown,
            WFS_PTR_RETRACTBINHIGH => PrinterStatusClass.RetractBinsClass.StateEnum.High,
            _ => throw new UnknowConstException(val, typeof(WRetractBin))
        };
    }

    public static class FwDeviceType
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwType */
        public const WORD WFS_PTR_TYPERECEIPT               = (0x0001);
        public const WORD WFS_PTR_TYPEPASSBOOK              = (0x0002);
        public const WORD WFS_PTR_TYPEJOURNAL               = (0x0004);
        public const WORD WFS_PTR_TYPEDOCUMENT              = (0x0008);
        public const WORD WFS_PTR_TYPESCANNER               = (0x0010);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.TypeEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.TypeEnum ret = 0;

            if ((val & WFS_PTR_TYPERECEIPT) == WFS_PTR_TYPERECEIPT) ret |= PrinterCapabilitiesClass.TypeEnum.Receipt;
            if ((val & WFS_PTR_TYPEPASSBOOK) == WFS_PTR_TYPEPASSBOOK) ret |= PrinterCapabilitiesClass.TypeEnum.Passbook;
            if ((val & WFS_PTR_TYPEJOURNAL) == WFS_PTR_TYPEJOURNAL) ret |= PrinterCapabilitiesClass.TypeEnum.Journal;
            if ((val & WFS_PTR_TYPEDOCUMENT) == WFS_PTR_TYPEDOCUMENT) ret |= PrinterCapabilitiesClass.TypeEnum.Document;
            if ((val & WFS_PTR_TYPESCANNER) == WFS_PTR_TYPESCANNER) ret |= PrinterCapabilitiesClass.TypeEnum.Scanner;

            return ret;
        }
    }

    public static class WResolution
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.wResolution,
                    WFSPTRPRINTFORM.wResolution */

        public const WORD WFS_PTR_RESLOW                    = (0x0001);
        public const WORD WFS_PTR_RESMED                    = (0x0002);
        public const WORD WFS_PTR_RESHIGH                   = (0x0004);
        public const WORD WFS_PTR_RESVERYHIGH               = (0x0008);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.ResolutionEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.ResolutionEnum ret = 0;

            if ((val & WFS_PTR_RESLOW) == WFS_PTR_RESLOW) ret |= PrinterCapabilitiesClass.ResolutionEnum.Low;
            if ((val & WFS_PTR_RESMED) == WFS_PTR_RESMED) ret |= PrinterCapabilitiesClass.ResolutionEnum.Medium;
            if ((val & WFS_PTR_RESHIGH) == WFS_PTR_RESHIGH) ret |= PrinterCapabilitiesClass.ResolutionEnum.High;
            if ((val & WFS_PTR_RESVERYHIGH) == WFS_PTR_RESVERYHIGH) ret |= PrinterCapabilitiesClass.ResolutionEnum.VeryHigh;

            return ret;

        }
    }

    public static class FwReadForm
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwReadForm */
        public const WORD WFS_PTR_READOCR                   = (0x0001);
        public const WORD WFS_PTR_READMICR                  = (0x0002);
        public const WORD WFS_PTR_READMSF                   = (0x0004);
        public const WORD WFS_PTR_READBARCODE               = (0x0008);
        public const WORD WFS_PTR_READPAGEMARK              = (0x0010);
        public const WORD WFS_PTR_READIMAGE                 = (0x0020);
        public const WORD WFS_PTR_READEMPTYLINE             = (0x0040);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.ReadFormEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.ReadFormEnum ret = 0;

            if ((val & WFS_PTR_READOCR) == WFS_PTR_READOCR) ret |= PrinterCapabilitiesClass.ReadFormEnum.OCR;
            if ((val & WFS_PTR_READMICR) == WFS_PTR_READMICR) ret |= PrinterCapabilitiesClass.ReadFormEnum.MICR;
            if ((val & WFS_PTR_READMSF) == WFS_PTR_READMSF) ret |= PrinterCapabilitiesClass.ReadFormEnum.MSF;
            if ((val & WFS_PTR_READBARCODE) == WFS_PTR_READBARCODE) ret |= PrinterCapabilitiesClass.ReadFormEnum.Barcode;
            if ((val & WFS_PTR_READPAGEMARK) == WFS_PTR_READPAGEMARK) ret |= PrinterCapabilitiesClass.ReadFormEnum.PageMark;
            if ((val & WFS_PTR_READIMAGE) == WFS_PTR_READIMAGE) ret |= PrinterCapabilitiesClass.ReadFormEnum.Image;
            if ((val & WFS_PTR_READEMPTYLINE) == WFS_PTR_READEMPTYLINE) ret |= PrinterCapabilitiesClass.ReadFormEnum.EmptyLine;

            return ret;
        }
    }

    public static class FwWriteForm
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwWriteForm */
        public const WORD WFS_PTR_WRITETEXT                 = (0x0001);
        public const WORD WFS_PTR_WRITEGRAPHICS             = (0x0002);
        public const WORD WFS_PTR_WRITEOCR                  = (0x0004);
        public const WORD WFS_PTR_WRITEMICR                 = (0x0008);
        public const WORD WFS_PTR_WRITEMSF                  = (0x0010);
        public const WORD WFS_PTR_WRITEBARCODE              = (0x0020);
        public const WORD WFS_PTR_WRITESTAMP                = (0x0040);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.WriteFormEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.WriteFormEnum ret = 0;

            if ((val & WFS_PTR_WRITETEXT) == WFS_PTR_WRITETEXT) ret |= PrinterCapabilitiesClass.WriteFormEnum.Text;
            if ((val & WFS_PTR_WRITEGRAPHICS) == WFS_PTR_WRITEGRAPHICS) ret |= PrinterCapabilitiesClass.WriteFormEnum.Graphics;
            if ((val & WFS_PTR_WRITEOCR) == WFS_PTR_WRITEOCR) ret |= PrinterCapabilitiesClass.WriteFormEnum.OCR;
            if ((val & WFS_PTR_WRITEMICR) == WFS_PTR_WRITEMICR) ret |= PrinterCapabilitiesClass.WriteFormEnum.MICR;
            if ((val & WFS_PTR_WRITEMSF) == WFS_PTR_WRITEMSF) ret |= PrinterCapabilitiesClass.WriteFormEnum.MSF;
            if ((val & WFS_PTR_WRITEBARCODE) == WFS_PTR_WRITEBARCODE) ret |= PrinterCapabilitiesClass.WriteFormEnum.Barcode;
            if ((val & WFS_PTR_WRITESTAMP) == WFS_PTR_WRITESTAMP) ret |= PrinterCapabilitiesClass.WriteFormEnum.Stamp;

            return ret;
        }
    }

    public static class FwExtents
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwExtents */
        public const WORD WFS_PTR_EXTHORIZONTAL           =  (0x0001);
        public const WORD WFS_PTR_EXTVERTICAL             =  (0x0002);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.ExtentEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.ExtentEnum ret = 0;

            if ((val & WFS_PTR_EXTHORIZONTAL) == WFS_PTR_EXTHORIZONTAL) ret |= PrinterCapabilitiesClass.ExtentEnum.Horizontal;
            if ((val & WFS_PTR_EXTVERTICAL) == WFS_PTR_EXTVERTICAL) ret |= PrinterCapabilitiesClass.ExtentEnum.Vertical;

            return ret;
        }
    }

    public static class FwControl
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwControl,
             WFSPTRCAPS.dwControlEx, dwMediaControl */

        public const WORD WFS_PTR_CTRLEJECT                 = (0x0001);
        public const WORD WFS_PTR_CTRLPERFORATE             = (0x0002);
        public const WORD WFS_PTR_CTRLCUT                   = (0x0004);
        public const WORD WFS_PTR_CTRLSKIP                  = (0x0008);
        public const WORD WFS_PTR_CTRLFLUSH                 = (0x0010);
        public const WORD WFS_PTR_CTRLRETRACT               = (0x0020);
        public const WORD WFS_PTR_CTRLSTACK                 = (0x0040);
        public const WORD WFS_PTR_CTRLPARTIALCUT            = (0x0080);
        public const WORD WFS_PTR_CTRLALARM                 = (0x0100);
        public const WORD WFS_PTR_CTRLATPFORWARD            = (0x0200);
        public const WORD WFS_PTR_CTRLATPBACKWARD           = (0x0400);
        public const WORD WFS_PTR_CTRLTURNMEDIA             = (0x0800);
        public const WORD WFS_PTR_CTRLSTAMP                 = (0x1000);
        public const WORD WFS_PTR_CTRLPARK                  = (0x2000);
        public const WORD WFS_PTR_CTRLEXPEL                 = (0x4000);
        public const WORD WFS_PTR_CTRLEJECTTOTRANSPORT      = (0x8000);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.ControlEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.ControlEnum ret = 0;

            if ((val & WFS_PTR_CTRLEJECT) == WFS_PTR_CTRLEJECT) ret |= PrinterCapabilitiesClass.ControlEnum.Eject;
            if ((val & WFS_PTR_CTRLPERFORATE) == WFS_PTR_CTRLPERFORATE) ret |= PrinterCapabilitiesClass.ControlEnum.Perforate;
            if ((val & WFS_PTR_CTRLCUT) == WFS_PTR_CTRLCUT) ret |= PrinterCapabilitiesClass.ControlEnum.Cut;
            if ((val & WFS_PTR_CTRLSKIP) == WFS_PTR_CTRLSKIP) ret |= PrinterCapabilitiesClass.ControlEnum.Skip;
            if ((val & WFS_PTR_CTRLFLUSH) == WFS_PTR_CTRLFLUSH) ret |= PrinterCapabilitiesClass.ControlEnum.Flush;
            if ((val & WFS_PTR_CTRLRETRACT) == WFS_PTR_CTRLRETRACT) ret |= PrinterCapabilitiesClass.ControlEnum.Retract;
            if ((val & WFS_PTR_CTRLSTACK) == WFS_PTR_CTRLSTACK) ret |= PrinterCapabilitiesClass.ControlEnum.Stack;
            if ((val & WFS_PTR_CTRLPARTIALCUT) == WFS_PTR_CTRLPARTIALCUT) ret |= PrinterCapabilitiesClass.ControlEnum.PartialCut;
            if ((val & WFS_PTR_CTRLALARM) == WFS_PTR_CTRLALARM) ret |= PrinterCapabilitiesClass.ControlEnum.Alarm;
            if ((val & WFS_PTR_CTRLATPFORWARD) == WFS_PTR_CTRLATPFORWARD) ret |= PrinterCapabilitiesClass.ControlEnum.Forward;
            if ((val & WFS_PTR_CTRLATPBACKWARD) == WFS_PTR_CTRLATPBACKWARD) ret |= PrinterCapabilitiesClass.ControlEnum.Backward;
            if ((val & WFS_PTR_CTRLTURNMEDIA) == WFS_PTR_CTRLTURNMEDIA) ret |= PrinterCapabilitiesClass.ControlEnum.TurnMedia;
            if ((val & WFS_PTR_CTRLSTAMP) == WFS_PTR_CTRLSTAMP) ret |= PrinterCapabilitiesClass.ControlEnum.Stamp;
            if ((val & WFS_PTR_CTRLPARK) == WFS_PTR_CTRLPARK) ret |= PrinterCapabilitiesClass.ControlEnum.Park;
            if ((val & WFS_PTR_CTRLEXPEL) == WFS_PTR_CTRLEXPEL) ret |= PrinterCapabilitiesClass.ControlEnum.Expel;
            if ((val & WFS_PTR_CTRLEJECTTOTRANSPORT) == WFS_PTR_CTRLEJECTTOTRANSPORT) ret |= PrinterCapabilitiesClass.ControlEnum.EjectToTransport;

            return ret;
        }

        public static WORD FromMediaControlEnum(ResetDeviceRequest.MediaControlEnum val)
        {
            WORD ret = 0x0000;
            if (val.HasFlag(ResetDeviceRequest.MediaControlEnum.Retract)) ret |= WFS_PTR_CTRLRETRACT;
            if (val.HasFlag(ResetDeviceRequest.MediaControlEnum.Eject)) ret |= WFS_PTR_CTRLEJECT;
            if (val.HasFlag(ResetDeviceRequest.MediaControlEnum.Expel)) ret |= WFS_PTR_CTRLEXPEL;
            return ret;
        }
    }

    public static class FwImageType
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwImageType,
                  WFSPTRIMAGEREQUEST.wFrontImageType and
                  WFSPTRIMAGEREQUEST.wBackImageType */

        public const WORD WFS_PTR_IMAGETIF                  = (0x0001);
        public const WORD WFS_PTR_IMAGEWMF                  = (0x0002);
        public const WORD WFS_PTR_IMAGEBMP                  = (0x0004);
        public const WORD WFS_PTR_IMAGEJPG                  = (0x0008);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.ImageTypeEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.ImageTypeEnum ret = 0;

            if ((val & WFS_PTR_IMAGETIF) == WFS_PTR_IMAGETIF) ret |= PrinterCapabilitiesClass.ImageTypeEnum.TIF;
            if ((val & WFS_PTR_IMAGEWMF) == WFS_PTR_IMAGEWMF) ret |= PrinterCapabilitiesClass.ImageTypeEnum.WMF;
            if ((val & WFS_PTR_IMAGEBMP) == WFS_PTR_IMAGEBMP) ret |= PrinterCapabilitiesClass.ImageTypeEnum.BMP;
            if ((val & WFS_PTR_IMAGEJPG) == WFS_PTR_IMAGEJPG) ret |= PrinterCapabilitiesClass.ImageTypeEnum.JPG;

            return ret;
        }
    }

    public static class FwImageColorFormat
    {
        #pragma warning disable format
                                       /* values of WFSPTRCAPS.fwFrontImageColorFormat,
                                                    WFSPTRCAPS.fwBackImageColorFormat,
                                                    WFSPTRIMAGEREQUEST.wFrontImageColorFormat and
                                                    WFSPTRIMAGEREQUEST.wBackImageColorFormat */

        public const WORD WFS_PTR_IMAGECOLORBINARY          = (0x0001);
        public const WORD WFS_PTR_IMAGECOLORGRAYSCALE       = (0x0002);
        public const WORD WFS_PTR_IMAGECOLORFULL            = (0x0004);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.FrontImageColorFormatEnum ToFrontEnum(WORD val)
        {
            PrinterCapabilitiesClass.FrontImageColorFormatEnum ret = 0;

            if ((val & WFS_PTR_IMAGECOLORBINARY) == WFS_PTR_IMAGECOLORBINARY) ret |= PrinterCapabilitiesClass.FrontImageColorFormatEnum.Binary;
            if ((val & WFS_PTR_IMAGECOLORGRAYSCALE) == WFS_PTR_IMAGECOLORGRAYSCALE) ret |= PrinterCapabilitiesClass.FrontImageColorFormatEnum.GrayScale;
            if ((val & WFS_PTR_IMAGECOLORFULL) == WFS_PTR_IMAGECOLORFULL) ret |= PrinterCapabilitiesClass.FrontImageColorFormatEnum.Full;

            return ret;
        }

        public static PrinterCapabilitiesClass.BackImageColorFormatEnum ToBackEnum(WORD val)
        {
            PrinterCapabilitiesClass.BackImageColorFormatEnum ret = 0;

            if ((val & WFS_PTR_IMAGECOLORBINARY) == WFS_PTR_IMAGECOLORBINARY) ret |= PrinterCapabilitiesClass.BackImageColorFormatEnum.Binary;
            if ((val & WFS_PTR_IMAGECOLORGRAYSCALE) == WFS_PTR_IMAGECOLORGRAYSCALE) ret |= PrinterCapabilitiesClass.BackImageColorFormatEnum.GrayScale;
            if ((val & WFS_PTR_IMAGECOLORFULL) == WFS_PTR_IMAGECOLORFULL) ret |= PrinterCapabilitiesClass.BackImageColorFormatEnum.Full;

            return ret;
        }
    }

    public static class FwCodelineFormat
    {
        #pragma warning disable format
        /* values of WFSPTRCAPS.fwCodelineFormat and
                    WFSPTRIMAGEREQUEST.wCodelineFormat */

        public const WORD WFS_PTR_CODELINECMC7              = (0x0001);
        public const WORD WFS_PTR_CODELINEE13B              = (0x0002);
        public const WORD WFS_PTR_CODELINEOCR               = (0x0004);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.CodelineFormatEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.CodelineFormatEnum ret = 0;

            if ((val & WFS_PTR_CODELINECMC7) == WFS_PTR_CODELINECMC7) ret |= PrinterCapabilitiesClass.CodelineFormatEnum.CMC7;
            if ((val & WFS_PTR_CODELINEE13B) == WFS_PTR_CODELINEE13B) ret |= PrinterCapabilitiesClass.CodelineFormatEnum.E13B;
            if ((val & WFS_PTR_CODELINEOCR) == WFS_PTR_CODELINEOCR) ret |= PrinterCapabilitiesClass.CodelineFormatEnum.OCR;

            return ret;
        }
    }

    public static class FwImageSource
    {
        #pragma warning disable format
         /* values of WFSPTRCAPS.fwImageSource,
                        WFSPTRIMAGEREQUEST.fwImageSource and
                        WFSPTRIMAGE.wImageSource */

        public const WORD WFS_PTR_IMAGEFRONT                = (0x0001);
        public const WORD WFS_PTR_IMAGEBACK                 = (0x0002);
        public const WORD WFS_PTR_CODELINE                  = (0x0004);
        #pragma warning restore format

        public static PrinterCapabilitiesClass.ImageSourceTypeEnum ToEnum(WORD val)
        {
            PrinterCapabilitiesClass.ImageSourceTypeEnum ret = 0;

            if ((val & WFS_PTR_IMAGEFRONT) == WFS_PTR_IMAGEFRONT) ret |= PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageFront;
            if ((val & WFS_PTR_IMAGEBACK) == WFS_PTR_IMAGEBACK) ret |= PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageBack;
            if ((val & WFS_PTR_CODELINE) == WFS_PTR_CODELINE) ret |= PrinterCapabilitiesClass.ImageSourceTypeEnum.CodeLine;

            return ret;
        }
    }

    /* values of WFSFRMHEADER.wBase,
             WFSFRMMEDIA.wBase,
             WFSPTRMEDIAUNIT.wBase */
    public static class WBase
    {
        #pragma warning disable format
        public const WORD WFS_FRM_INCH      = (0);
        public const WORD WFS_FRM_MM        = (1);
        public const WORD WFS_FRM_ROWCOLUMN = (2);
        #pragma warning restore format

        public static Form.BaseEnum ToFormEnum(WORD val) => val switch
        {
            WFS_FRM_INCH => Form.BaseEnum.INCH,
            WFS_FRM_MM => Form.BaseEnum.MM,
            WFS_FRM_ROWCOLUMN => Form.BaseEnum.ROWCOLUMN,
            _ => throw new UnknowConstException(val, typeof(WBase))
        };

        public static Media.BaseEnum ToMediaEnum(WORD val) => val switch
        {
            WFS_FRM_INCH => Media.BaseEnum.INCH,
            WFS_FRM_MM => Media.BaseEnum.MM,
            WFS_FRM_ROWCOLUMN => Media.BaseEnum.ROWCOLUMN,
            _ => throw new UnknowConstException(val, typeof(WBase))
        };
    }

    /* values of WFSPTRPRINTFORM.wAlignment */
    public static class WAlignment
    {
        #pragma warning disable format
        public const WORD WFS_PTR_ALNUSEFORMDEFN    = (0);
        public const WORD WFS_PTR_ALNTOPLEFT        = (1);
        public const WORD WFS_PTR_ALNTOPRIGHT       = (2);
        public const WORD WFS_PTR_ALNBOTTOMLEFT     = (3);
        public const WORD WFS_PTR_ALNBOTTOMRIGHT    = (4);
        #pragma warning restore format

        public static Form.AlignmentEnum ToEnum(WORD val) => val switch
        {
            WFS_PTR_ALNUSEFORMDEFN => Form.AlignmentEnum.TOPLEFT,
            WFS_PTR_ALNTOPLEFT => Form.AlignmentEnum.TOPLEFT,
            WFS_PTR_ALNTOPRIGHT => Form.AlignmentEnum.TOPRIGHT,
            WFS_PTR_ALNBOTTOMLEFT => Form.AlignmentEnum.BOTTOMLEFT,
            WFS_PTR_ALNBOTTOMRIGHT => Form.AlignmentEnum.BOTTOMRIGHT,
            _ => throw new UnknowConstException(val, typeof(WAlignment))
        };
    }

    /* values of WFSFRMHEADER.wOrientation */
    public static class WOrientation
    {
        #pragma warning disable format
        public const WORD WFS_FRM_PORTRAIT  = (0);
        public const WORD WFS_FRM_LANDSCAPE = (1);
        #pragma warning restore format

        public static FormOrientationEnum ToEnum(WORD val) => val switch
        {
            //WFS_PTR_ALNUSEFORMDEFN => AlignmentEnum.,
            WFS_FRM_PORTRAIT => FormOrientationEnum.PORTRAIT,
            WFS_FRM_LANDSCAPE => FormOrientationEnum.LANDSCAPE,
            _ => throw new UnknowConstException(val, typeof(WOrientation))
        };
    }

    /* values of WFSFRMMEDIA.fwMediaType */
    /// <summary>
    /// /* values of WFSFRMMEDIA.fwMediaType */
    /// </summary>
    public static class FwMediaType
    {
        #pragma warning disable format
        public const WORD WFS_FRM_MEDIAGENERIC      = (0);
        public const WORD WFS_FRM_MEDIAPASSBOOK     = (1);
        public const WORD WFS_FRM_MEDIAMULTIPART    = (2);
        #pragma warning restore format

        public static Media.TypeEnum ToEnum(WORD val) => val switch
        {
            WFS_FRM_MEDIAGENERIC => Media.TypeEnum.GENERIC,
            WFS_FRM_MEDIAPASSBOOK => Media.TypeEnum.PASSBOOK,
            WFS_FRM_MEDIAMULTIPART => Media.TypeEnum.MULTIPART,
            _ => throw new UnknowConstException(val, typeof(FwMediaType))
        };
    }

    /* values of WFSPTRCAPS.fwPaperSources,
             WFSFRMMEDIA.wPaperSources,
             WFSPTRPRINTFORM.wPaperSource and
             WFSPTRPAPERTHRESHOLD.wPaperSource */
    public static class WPaperSources
    {
        #pragma warning disable format
        public const WORD WFS_PTR_PAPERANY                  = (0x0001);
        public const WORD WFS_PTR_PAPERUPPER                = (0x0002);
        public const WORD WFS_PTR_PAPERLOWER                = (0x0004);
        public const WORD WFS_PTR_PAPEREXTERNAL             = (0x0008);
        public const WORD WFS_PTR_PAPERAUX                  = (0x0010);
        public const WORD WFS_PTR_PAPERAUX2                 = (0x0020);
        public const WORD WFS_PTR_PAPERPARK                 = (0x0040);
        #pragma warning restore format

        public static Media.SourceEnum ToEnum(WORD val)
        {
            Media.SourceEnum ret = Media.SourceEnum.ANY;
            if ((val & WFS_PTR_PAPERUPPER) == WFS_PTR_PAPERUPPER)
            {
                ret |= Media.SourceEnum.UPPER;
            }
            if ((val & WFS_PTR_PAPERLOWER) == WFS_PTR_PAPERLOWER)
            {
                ret |= Media.SourceEnum.LOWER;
            }
            if ((val & WFS_PTR_PAPEREXTERNAL) == WFS_PTR_PAPEREXTERNAL)
            {
                ret |= Media.SourceEnum.EXTERNAL;
            }
            if ((val & WFS_PTR_PAPERAUX) == WFS_PTR_PAPERAUX)
            {
                ret |= Media.SourceEnum.AUX;
            }
            if ((val & WFS_PTR_PAPERAUX2) == WFS_PTR_PAPERAUX2)
            {
                ret |= Media.SourceEnum.AUX2;
            }
            if ((val & WFS_PTR_PAPERPARK) == WFS_PTR_PAPERPARK)
            {
                ret |= Media.SourceEnum.PARK;
            }

            return ret;
        }

        public static PrinterCapabilitiesClass.PaperSourceEnum ToPaperSourceEnum(WORD val)
        {
            PrinterCapabilitiesClass.PaperSourceEnum ret = 0;
            if ((val & WFS_PTR_PAPERUPPER) == WFS_PTR_PAPERUPPER)
            {
                ret |= PrinterCapabilitiesClass.PaperSourceEnum.Upper;
            }
            if ((val & WFS_PTR_PAPERLOWER) == WFS_PTR_PAPERLOWER)
            {
                ret |= PrinterCapabilitiesClass.PaperSourceEnum.Lower;
            }
            if ((val & WFS_PTR_PAPEREXTERNAL) == WFS_PTR_PAPEREXTERNAL)
            {
                ret |= PrinterCapabilitiesClass.PaperSourceEnum.External;
            }
            if ((val & WFS_PTR_PAPERAUX) == WFS_PTR_PAPERAUX)
            {
                ret |= PrinterCapabilitiesClass.PaperSourceEnum.AUX;
            }
            if ((val & WFS_PTR_PAPERAUX2) == WFS_PTR_PAPERAUX2)
            {
                ret |= PrinterCapabilitiesClass.PaperSourceEnum.AUX2;
            }
            if ((val & WFS_PTR_PAPERPARK) == WFS_PTR_PAPERPARK)
            {
                ret |= PrinterCapabilitiesClass.PaperSourceEnum.Park;
            }

            return ret;
        }
    }

    public static class WFoldType
    {
        #pragma warning disable format
        public const WORD WFS_FRM_FOLDNONE                  = (0);
        public const WORD WFS_FRM_FOLDHORIZONTAL            = (1);
        public const WORD WFS_FRM_FOLDVERTICAL              = (2);
#pragma warning restore format

        public static Media.FoldEnum ToEnum(WORD val) => val switch
        {
            WFS_FRM_FOLDNONE or WFS_FRM_FOLDHORIZONTAL or WFS_FRM_FOLDVERTICAL => Media.FoldEnum.NONE,
            //WFS_FRM_FOLDHORIZONTAL => Media.FoldEnum.HORIZONTAL,
            //WFS_FRM_FOLDVERTICAL => Media.FoldEnum.VERTICAL,
            _ => throw new UnknowConstException(val, typeof(WFoldType))
        };
    }

    public static class FwFieldType
    {
        #pragma warning disable format
        public const WORD WFS_FRM_FIELDTEXT                 = (0);
        public const WORD WFS_FRM_FIELDMICR                 = (1);
        public const WORD WFS_FRM_FIELDOCR                  = (2);
        public const WORD WFS_FRM_FIELDMSF                  = (3);
        public const WORD WFS_FRM_FIELDBARCODE              = (4);
        public const WORD WFS_FRM_FIELDGRAPHIC              = (5);
        public const WORD WFS_FRM_FIELDPAGEMARK             = (6);       
        #pragma warning restore format

        public static FieldTypeEnum ToEnum(WORD val) => val switch
        {
            WFS_FRM_FIELDTEXT => FieldTypeEnum.TEXT,
            WFS_FRM_FIELDMICR => FieldTypeEnum.MICR,
            WFS_FRM_FIELDOCR => FieldTypeEnum.OCR,
            WFS_FRM_FIELDMSF => FieldTypeEnum.MSF,
            WFS_FRM_FIELDBARCODE => FieldTypeEnum.BARCODE,
            WFS_FRM_FIELDGRAPHIC => FieldTypeEnum.GRAPHIC,
            WFS_FRM_FIELDPAGEMARK => FieldTypeEnum.PAGEMARK,
            _ => throw new UnknowConstException(val, typeof(FwFieldType))
        };
    }

    /* values of WFSFRMFIELD.fwClass */
    public static class FwClass
    {
        #pragma warning disable format
        public const WORD WFS_FRM_CLASSSTATIC   = (0);
        public const WORD WFS_FRM_CLASSOPTIONAL = (1);
        public const WORD WFS_FRM_CLASSREQUIRED = (2);
        #pragma warning restore format

        public static FormField.ClassEnum ToEnum(WORD val) => val switch
        {
            WFS_FRM_CLASSSTATIC => FormField.ClassEnum.STATIC,
            WFS_FRM_CLASSOPTIONAL => FormField.ClassEnum.OPTIONAL,
            WFS_FRM_CLASSREQUIRED => FormField.ClassEnum.REQUIRED,
            _ => throw new UnknowConstException(val, typeof(FwClass))
        };
    }

    /* values of WFSFRMFIELD.fwAccess */
    public static class FwAccess
    {
        #pragma warning disable format
        public const WORD WFS_FRM_ACCESSREAD = (0x0001);
        public const WORD WFS_FRM_ACCESSWRITE = (0x0002);
        public const WORD READ_WRITE = WFS_FRM_ACCESSREAD | WFS_FRM_ACCESSWRITE;
        #pragma warning restore format

        public static FieldAccessEnum ToEnum(WORD val) => val switch
        {

            WFS_FRM_ACCESSREAD => FieldAccessEnum.READ,
            WFS_FRM_ACCESSWRITE => FieldAccessEnum.WRITE,
            READ_WRITE => FieldAccessEnum.READWRITE,
            _ => throw new UnknowConstException(val, typeof(FwAccess))
        };
    }

    /* values of WFSFRMFIELD.fwOverflow */
    public static class FwOverflow
    {
        #pragma warning disable format
        public const WORD WFS_FRM_OVFTERMINATE              = (0);
        public const WORD WFS_FRM_OVFTRUNCATE               = (1);
        public const WORD WFS_FRM_OVFBESTFIT                = (2);
        public const WORD WFS_FRM_OVFOVERWRITE              = (3);
        public const WORD WFS_FRM_OVFWORDWRAP               = (4);
        #pragma warning restore format

        public static FormField.OverflowEnum ToEnum(WORD val) => val switch
        {

            WFS_FRM_OVFTERMINATE => FormField.OverflowEnum.TERMINATE,
            WFS_FRM_OVFTRUNCATE => FormField.OverflowEnum.TRUNCATE,
            WFS_FRM_OVFBESTFIT => FormField.OverflowEnum.BESTFIT,
            WFS_FRM_OVFOVERWRITE => FormField.OverflowEnum.OVERWRITE,
            WFS_FRM_OVFWORDWRAP => FormField.OverflowEnum.WORDWRAP,
            _ => throw new UnknowConstException(val, typeof(FwAccess))
        };
    }

    /* values of WFSPTRCAPS.fwControl,
                 WFSPTRCAPS.dwControlEx, dwMediaControl */
    public static class DwMediaControl
    {
        #pragma warning disable format
        public const DWORD WFS_PTR_CTRLEJECT                 = (0x0001);
        public const DWORD WFS_PTR_CTRLPERFORATE             = (0x0002);
        public const DWORD WFS_PTR_CTRLCUT                   = (0x0004);
        public const DWORD WFS_PTR_CTRLSKIP                  = (0x0008);
        public const DWORD WFS_PTR_CTRLFLUSH                 = (0x0010);
        public const DWORD WFS_PTR_CTRLRETRACT               = (0x0020);
        public const DWORD WFS_PTR_CTRLSTACK                 = (0x0040);
        public const DWORD WFS_PTR_CTRLPARTIALCUT            = (0x0080);
        public const DWORD WFS_PTR_CTRLALARM                 = (0x0100);
        public const DWORD WFS_PTR_CTRLATPFORWARD            = (0x0200);
        public const DWORD WFS_PTR_CTRLATPBACKWARD           = (0x0400);
        public const DWORD WFS_PTR_CTRLTURNMEDIA             = (0x0800);
        public const DWORD WFS_PTR_CTRLSTAMP                 = (0x1000);
        public const DWORD WFS_PTR_CTRLPARK                  = (0x2000);
        public const DWORD WFS_PTR_CTRLEXPEL                 = (0x4000);
        public const DWORD WFS_PTR_CTRLEJECTTOTRANSPORT      = (0x8000);

             /* values of WFSPTRCAPS.dwControlEx, dwMediaControl */

        public const DWORD WFS_PTR_CTRLROTATE180             = (0x00010000);
        public const DWORD WFS_PTR_CTRLCLEARBUFFER           = (0x00020000);
        #pragma warning restore format

        public static DWORD FromEnum(PrinterCapabilitiesClass.ControlEnum val)
        {
            DWORD ret = 0x00000000;
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Eject))
            {
                ret |= WFS_PTR_CTRLEJECT;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Rotate180))
            {
                ret |= WFS_PTR_CTRLPERFORATE;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Cut))
            {
                ret |= WFS_PTR_CTRLCUT;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Skip))
            {
                ret |= WFS_PTR_CTRLSKIP;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Flush))
            {
                ret |= WFS_PTR_CTRLFLUSH;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract))
            {
                ret |= WFS_PTR_CTRLRETRACT;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stack))
            {
                ret |= WFS_PTR_CTRLSTACK;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.PartialCut))
            {
                ret |= WFS_PTR_CTRLPARTIALCUT;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Alarm))
            {
                ret |= WFS_PTR_CTRLALARM;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Forward))
            {
                ret |= WFS_PTR_CTRLATPFORWARD;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Backward))
            {
                ret |= WFS_PTR_CTRLATPBACKWARD;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.TurnMedia))
            {
                ret |= WFS_PTR_CTRLTURNMEDIA;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stamp))
            {
                ret |= WFS_PTR_CTRLSTAMP;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Park))
            {
                ret |= WFS_PTR_CTRLPARK;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.Expel))
            {
                ret |= WFS_PTR_CTRLEXPEL;
            }
            if (val.HasFlag(PrinterCapabilitiesClass.ControlEnum.EjectToTransport))
            {
                ret |= WFS_PTR_CTRLEJECTTOTRANSPORT;
            }
            return ret;
        }
    }

    /* values of WFSPTRRAWDATA.wInputData */
    public static class WInputData
    {
        public const WORD WFS_PTR_NOINPUTDATA = (0);
        public const WORD WFS_PTR_INPUTDATA = (1);

        public static WORD FormBool(bool isInputData) => isInputData ? WFS_PTR_INPUTDATA : WFS_PTR_NOINPUTDATA;

    }

    #region Command Data
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRQUERYFIELD
    {
        [FieldOffset(0)] LPSTR lpszFormName;
        [FieldOffset(4)] LPSTR lpszFieldName;

        public static CommandData BuildCommandData(string formName, string? fieldName = null)
        {
            WFSPTRQUERYFIELD dataCommand = new()
            {
                lpszFormName = LPSTR.Zero,
                lpszFieldName = LPSTR.Zero
            };

            var commandData = CommandData.FromStructureNoIntPtr(ref dataCommand);
            commandData.AddLPSTRField<WFSPTRQUERYFIELD>(nameof(lpszFormName), formName);
            if (fieldName != null)
            {
                commandData.AddLPSTRField<WFSPTRQUERYFIELD>(nameof(lpszFieldName), fieldName);
            }
            return commandData;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRPRINTFORM
    {
        [FieldOffset(0)] LPSTR lpszFormName;
        [FieldOffset(4)] LPSTR lpszMediaName;
        [FieldOffset(8)] WORD wAlignment;
        [FieldOffset(10)] WORD wOffsetX;
        [FieldOffset(12)] WORD wOffsetY;
        [FieldOffset(14)] WORD wResolution;
        [FieldOffset(16)] DWORD dwMediaControl;
        [FieldOffset(20)] LPSTR lpszFields;
        [FieldOffset(24)] LPWSTR lpszUNICODEFields;
        [FieldOffset(28)] WORD wPaperSource;

        public static CommandData BuildCommandData(DirectFormPrintRequest request)
        {
            WFSPTRPRINTFORM dataCommand = new()
            {
                lpszFormName = LPSTR.Zero,
                lpszMediaName = LPSTR.Zero,
                wAlignment = WAlignment.WFS_PTR_ALNUSEFORMDEFN,
                wOffsetX = 0,
                wOffsetY = 0,
                wResolution = 1,
                dwMediaControl = 0x00000000,
                lpszFields = LPSTR.Zero,
                lpszUNICODEFields = LPWSTR.Zero,
                wPaperSource = WPaperSources.WFS_PTR_PAPERANY
            };

            var commandData = CommandData.FromStructureNoIntPtr(ref dataCommand);
            commandData.AddLPSTRField<WFSPTRPRINTFORM>(nameof(lpszFormName), request.FormName);
            commandData.AddLPSTRField<WFSPTRPRINTFORM>(nameof(lpszMediaName), request.MediaName);

            List<string> fieldList = request.Fields.Select(dict => $"{dict.Key}={dict.Value}").ToList();

            commandData.AddLPSZField<WFSPTRPRINTFORM>(nameof(lpszFields), fieldList);

            return commandData;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRRAWDATA
    {
        [FieldOffset(0)] WORD wInputData;
        [FieldOffset(2)] ULONG ulSize;
        [FieldOffset(6)] LPBYTE lpbData;

        public static CommandData BuildCommandData(RawPrintRequest request)
        {
            WFSPTRRAWDATA dataCommand = new()
            {
                wInputData = WInputData.FormBool(request.Input),
                ulSize = (ULONG)request.PrintData.Count(),
                lpbData = LPSTR.Zero
            };

            var commandData = CommandData.FromStructureNoIntPtr(ref dataCommand);
            commandData.AddLPBYTEField<WFSPTRRAWDATA>(nameof(lpbData), request.PrintData);
            return commandData;
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRRESET
    {
        [FieldOffset(0)] DWORD dwMediaControl;
        [FieldOffset(4)] USHORT usRetractBinNumber;

        public static CommandData BuildCommandData(ResetDeviceRequest request)
        {
            WFSPTRRESET dataCommand = new()
            {
                dwMediaControl = FwControl.FromMediaControlEnum(request.MediaControl),
                usRetractBinNumber = (USHORT)((request.RetractBin == -1) ? 0 : request.RetractBin),
            };

            return CommandData.FromStructureNoIntPtr(ref dataCommand);
        }
    }

    #endregion
}