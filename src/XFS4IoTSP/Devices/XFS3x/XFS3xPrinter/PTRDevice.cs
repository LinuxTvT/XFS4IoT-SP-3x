using XFS3xAPI;
using XFS3xAPI.PTR;
using XFS4IoTFramework.Printer;
using static XFS3xAPI.PTR.WFSPTRPRINTFORM;
using EVENT = XFS3xAPI.PTR.EVENT;
using HRESULT = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;

namespace Printer.XFS3xPrinter
{
    public class PTRDevice : XfsService
    {
        public readonly AutoResetEvent MediaTakenEvent = new(false);

        public PTRDevice(string logicalName) : base(logicalName) { }

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

        protected override void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"HandleCompleteEvent: [{xfsResult.dwCommandCode}]=[{RESULT.ToString(xfsResult.hResult)}]");
            switch (xfsResult.dwCommandCode)
            {
                case CMD.WFS_CMD_PTR_RAW_DATA:
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
            base.HandleExecuteEvent(ref  xfsResult);
        }
    }
}
