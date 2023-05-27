using XFS3xAPI;
using XFS3xAPI.PTR;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Printer;
using EVENT = XFS3xAPI.PTR.EVENT;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;

namespace Printer.XFS3xPrinter
{
    public class PTRDevice : XfsService
    {
        public readonly AutoResetEvent MediaTakenEvent = new(false);

        public PTRDevice(string logicalName) : base(logicalName) { }

        public void UpdateStatus(CommonStatusClass commonStatus, PrinterStatusClass printerStatus)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_PTR_STATUS, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    using ResultData resultData = new(lpResult);
                    WFSRESULT wfsResult = resultData.ReadResult();
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPTRSTATUS_300 wfsStatus = wfsResult.ReadPTRStatus300();

                        // Common status
                        commonStatus.Device = FwDevice.ToEnum(wfsStatus.fwDevice);
                        //commonStatus.DevicePosition = WDevicePosition.ToEnum(wfsStatus.wDevicePosition);

                        // Printer status
                        printerStatus.Media = FwMedia.ToEnum(wfsStatus.fwMedia);
                        // Printer paper status
                        if (printerStatus.Paper == null)
                        {
                            printerStatus.Paper = new Dictionary<PrinterStatusClass.PaperSourceEnum, PrinterStatusClass.SupplyStatusClass>();
                        }
                        else
                        {
                            printerStatus.Paper.Clear();
                        }

                        if (printerStatus.CustomPaper == null)
                        {
                            printerStatus.CustomPaper = new Dictionary<string, PrinterStatusClass.SupplyStatusClass>();
                        }
                        else
                        {
                            printerStatus.CustomPaper.Clear();
                        }
                        wfsStatus.ReadPaper(printerStatus.Paper, printerStatus.CustomPaper);

                        printerStatus.Toner = FwToner.ToEnum(wfsStatus.fwToner);
                        printerStatus.Ink = FwInk.ToEnum(wfsStatus.fwInk);
                        printerStatus.Lamp = FwLamp.ToEnum(wfsStatus.fwLamp);

                        if (printerStatus.RetractBins == null)
                        {
                            printerStatus.RetractBins = new List<PrinterStatusClass.RetractBinsClass>();
                        }
                        else
                        {
                            printerStatus.RetractBins.Clear();
                        }
                        wfsStatus.ReadRestactBins(printerStatus.RetractBins);
                        printerStatus.MediaOnStacker = wfsStatus.usMediaOnStacker;
                    }
                    else
                    {
                        throw new NullBufferException();
                    }
                }
                else
                {
                    throw new NullResultException();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                commonStatus.Device = CommonStatusClass.DeviceEnum.PotentialFraud;
                throw;
            }
        }

        public PrinterCapabilitiesClass? GetCapabilities()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PTR_CAPABILITIES, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                if (wfsResult.lpBuffer != LPVOID.Zero)
                {
                    WFSPTRCAPS300 wfsCaps = wfsResult.ReadPTRCaps300();

                    return new PrinterCapabilitiesClass(FwDeviceType.ToEnum(wfsCaps.fwType),
                        WResolution.ToEnum(wfsCaps.wResolution),
                        FwReadForm.ToEnum(wfsCaps.fwReadForm),
                        FwWriteForm.ToEnum(wfsCaps.fwWriteForm),
                        FwExtents.ToEnum(wfsCaps.fwExtents),
                        FwControl.ToEnum(wfsCaps.fwControl),
                        wfsCaps.usMaxMediaOnStacker,
                        wfsCaps.bAcceptMedia == 0 ? false : true,
                        wfsCaps.bMultiPage == 0 ? false : true,
                        WPaperSources.ToPaperSourceEnum(wfsCaps.fwPaperSources),
                        wfsCaps.bMediaTaken == 0 ? false : true,
                        wfsCaps.usRetractBins,
                        wfsCaps.ReadRetractBins(),
                        FwImageType.ToEnum(wfsCaps.fwImageType),
                        FwImageColorFormat.ToFrontEnum(wfsCaps.fwFrontImageColorFormat),
                        FwImageColorFormat.ToBackEnum(wfsCaps.fwBackImageColorFormat),
                        FwCodelineFormat.ToEnum(wfsCaps.fwCodelineFormat),
                        FwImageSource.ToEnum(wfsCaps.fwImageSource),
                        DispensePaper: false,
                                                OSPrinter: null,
                                                MediaPresented: false,
                                                AutoRetractPeriod: 0,
                                                RetractToTransport: false,
                                                CoercivityTypes: PrinterCapabilitiesClass.CoercivityTypeEnum.NotSupported,
                                                ControlPassbook: PrinterCapabilitiesClass.ControlPassbookEnum.NotSupported,
                                                PrintSides: PrinterCapabilitiesClass.PrintSidesEnum.NotSupported)

                        ;

                    /*
                     * PrinterCapabilitiesClass(TypeEnum Types,
                                        ResolutionEnum Resolutions,
                                        ReadFormEnum ReadForms,
                                        WriteFormEnum WriteForms,
                                        ExtentEnum Extents,
                                        ControlEnum Controls,
                                        int MaxMediaOnStacker,
                                        bool AcceptMedia,
                                        bool MultiPage,
                                        PaperSourceEnum PaperSources,
                                        bool MediaTaken,
                                        int RetractBins,
                                        List<int> MaxRetract,
                                        ImageTypeEnum ImageTypes,
                                        FrontImageColorFormatEnum FrontImageColorFormats,
                                        BackImageColorFormatEnum BackImageColorFormats,
                                        CodelineFormatEnum CodelineFormats,
                                        ImageSourceTypeEnum ImageSourceTypes,
                                        bool DispensePaper,
                                        string OSPrinter,
                                        bool MediaPresented,
                                        int AutoRetractPeriod,
                                        bool RetractToTransport,
                                        CoercivityTypeEnum CoercivityTypes,
                                        ControlPassbookEnum ControlPassbook,
                                        PrintSidesEnum PrintSides,
                                        Dictionary<string, bool> CustomPaperSources = null)
        {
                    WFSPTRCAPS300 wfsCaps = wfsResult.ReadIDCCaps300();// Marshal.PtrToStructure<WFSIDCCAPS_300>(wfsResult.lpBuffer);
                    return new(FwType.ToEnum(wfsIDCCaps.fwType),
                        FwReadTracks.ToEnum(wfsIDCCaps.fwReadTracks),
                        FwWriteTracks.ToEnum(wfsIDCCaps.fwWriteTracks),
                        FwChipProtocols.ToEnum(wfsIDCCaps.fwChipProtocols),
                        FwSecType.ToEnum(wfsIDCCaps.fwSecType),
                        FwPowerOnOption.ToEnum(wfsIDCCaps.fwPowerOnOption),
                        FwPowerOffOption.ToEnum(wfsIDCCaps.fwPowerOffOption),
                        wfsIDCCaps.bFluxSensorProgrammable,
                        wfsIDCCaps.bReadWriteAccessFollowingEject,
                        FwWriteMode.ToEnum(wfsIDCCaps.fwWriteMode),
                        FwChipPower.ToEnum(wfsIDCCaps.fwChipPower),
                        CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.NotSupported,
                        CardReaderCapabilitiesClass.PositionsEnum.Exit | CardReaderCapabilitiesClass.PositionsEnum.Transport,
                        true);
                    */
                }
                else
                {
                    throw new NullBufferException();
                }
            }
            else
            {
                throw new NullResultException();
            }

        }

        public List<string> GetFormNameList()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PTR_FORM_LIST, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                return wfsResult.ReadLPSZ(15);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public List<string> GetMediaNameList()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PTR_MEDIA_LIST, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                return wfsResult.ReadLPSZ(4);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public XFS4IoTFramework.Printer.Form QueryForm(string name, IPrinterDevice Device)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            using CommandData commandData = new(name);

            GetInfo(CMD.WFS_INF_PTR_QUERY_FORM, commandData.DataPtr, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                WFSFRMHEADER frmHeader = wfsResult.ReadFrmHeader();

                return frmHeader.ToForm(Device);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public Media QueryMedia(string name, IPrinterDevice Device)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            Logger.Info($"Call {nameof(QueryMedia)}(formName=[{name}])");

            using CommandData commandData = new(name);

            GetInfo(CMD.WFS_INF_PTR_QUERY_MEDIA, commandData.DataPtr, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                var wfsResult = resultData.ReadResult();
                var media = wfsResult.ReadMedia();

                return media.ToMedia(name, Device);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public void QueryField(ref XFS4IoTFramework.Printer.Form form)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            Logger.Info($"Call {nameof(QueryField)}(formName=[{form.Name}])");

            using CommandData commandData = WFSPTRQUERYFIELD.BuildCommandData(form.Name);

            GetInfo(CMD.WFS_INF_PTR_QUERY_FIELD, commandData.DataPtr, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                var wfsResult = resultData.ReadResult();
                var formFields = wfsResult.ReadFormFields();
                foreach (var item in formFields)
                {
                    var formField = item.ToField();
                    form.Fields.Add(formField.Name, formField);
                }
            }
            else
            {
                throw new NullResultException();
            }
        }

        public void PrintForm(DirectFormPrintRequest request)
        {
            using var commandData = WFSPTRPRINTFORM.BuildCommandData(request);
            AsyncExecute(CMD.WFS_CMD_PTR_PRINT_FORM, commandData.DataPtr, 100000);
        }

        public void PrintRaw(RawPrintRequest request)
        {
            using var commandData = WFSPTRRAWDATA.BuildCommandData(request);
            AsyncExecute(CMD.WFS_CMD_PTR_RAW_DATA, commandData.DataPtr, 100000);
        }

        public void ControlMedia(ControlMediaRequest request)
        {
            using var commandData = new CommandData(DwMediaControl.FromEnum(request.Controls));
            AsyncExecute(CMD.WFS_CMD_PTR_CONTROL_MEDIA, commandData.DataPtr, 100000);
        }

        public List<byte>? RawDataIn = null;
        protected override void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"HandleCompleteEvent: [{xfsResult.dwCommandCode}]=[{RESULT.ToString(xfsResult.hResult)}]");
            switch (xfsResult.dwCommandCode)
            {
                case CMD.WFS_CMD_PTR_RAW_DATA:
                    if (xfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPTRRAWDATAIN rawDataIn = xfsResult.ReadRawDataIn();
                        RawDataIn = rawDataIn.Data;
                    }
                    break;
                case CMD.WFS_CMD_PTR_PRINT_FORM:
                case CMD.WFS_CMD_PTR_CONTROL_MEDIA:
                    break;
                default:
                    Logger.Error($"Unhanlde Complete CMD [{(xfsResult.dwCommandCode)}]");
                    Console.WriteLine($"Unhanlde Complete CMD [{(xfsResult.dwCommandCode)}]");
                    break;
            }
            base.HandleCompleteEvent(ref xfsResult);
        }

        protected override void HandleServiceEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_SRVE_PTR_MEDIATAKEN:
                    MediaTakenEvent.Set();
                    break;

                default:
                    Logger.Error($"Unhandle Service Event: [{(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Service Event: [{(xfsResult.dwEventID)}]");
                    break;
            }
            base.HandleServiceEvent(ref xfsResult);
        }

        protected override void HandleExecuteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                default:
                    Logger.Error($"Unhandle Execute Event: [{(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Execute Event: [{(xfsResult.dwEventID)}]");
                    break;
            }
            base.HandleExecuteEvent(ref xfsResult);
        }
    }
}
