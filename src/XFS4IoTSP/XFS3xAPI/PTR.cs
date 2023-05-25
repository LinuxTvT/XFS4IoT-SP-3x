using System.Drawing.Printing;
using System.Runtime.InteropServices;
using XFS3xAPI.PIN;
using XFS4IoTFramework.Printer;
using DWORD = System.UInt32;
using Form = XFS4IoTFramework.Printer.Form;
using LPSTR = System.IntPtr;
using LPWSTR = System.IntPtr;
using WORD = System.UInt16;

namespace XFS3xAPI.PTR
{
    public static class PTRExtension
    {
        public static WFSFRMHEADER ReadFrmHeader(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSFRMHEADER>(wfsResult.lpBuffer);
        public static WFSFRMMEDIA ReadMedia(this WFSRESULT wfsResult) => ResultData.ReadStructure<WFSFRMMEDIA>(wfsResult.lpBuffer);
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

        #pragma warning restore format
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
        private string? Format => ResultData.ReadLPSTR(lpszFormat);
        private string? InitialValue => ResultData.ReadLPSTR(lpszInitialValue);

        public FormField ToField()
        {
            return new FormField(FieldName,0,0,0,0,0,0,0,0,null, wIndexCount,0,0,0,0,null,0,0,0, 
                Format, InitialValue, FieldSideEnum.FRONT,FwType.ToEnum(fwType), FwClass.ToEnum(fwClass),
                FwAccess.ToEnum(fwAccess), Device, name, FwMediaType.ToEnum(fwMediaType), WPaperSources.ToEnum(wPaperSources), WBase.ToMediaEnum(wBase),
                wUnitX, wUnitY, wSizeWidth, wSizeHeight, wPrintAreaX, wPrintAreaY, wPrintAreaWidth, wPrintAreaHeight,
                wRestrictedAreaX, wRestrictedAreaY, wRestrictedAreaWidth, wRestrictedAreaHeight,
                WFoldType.ToEnum(wFoldType), wStagger, wPageCount, wLineCount);
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
            WFS_FRM_FOLDNONE => Media.FoldEnum.NONE,
            WFS_FRM_FOLDHORIZONTAL => Media.FoldEnum.HORIZONTAL,
            WFS_FRM_FOLDVERTICAL => Media.FoldEnum.VERTICAL,
            _ => throw new UnknowConstException(val, typeof(WFoldType))
        };
    }

    public static class FwType
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
            _ => throw new UnknowConstException(val, typeof(FwType))
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

    

    #region Command Data
    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct WFSPTRQUERYFIELD
    {
        [FieldOffset(0)] LPSTR lpszFormName;
        [FieldOffset(4)] LPSTR lpszFieldName;

        public static CommandData BuildCommandData(string formName, string fieldName)
        {
            WFSPTRQUERYFIELD dataCommand = new()
            {
                lpszFormName = LPSTR.Zero,
                lpszFieldName = LPSTR.Zero
            };

            var commandData = CommandData.FromStructureNoIntPtr(ref dataCommand);
            commandData.AddLPSTRField<WFSPTRQUERYFIELD>(nameof(lpszFormName), formName);
            commandData.AddLPSTRField<WFSPTRQUERYFIELD>(nameof(lpszFieldName), formName);
            return commandData;
        }
    }

    #endregion
}