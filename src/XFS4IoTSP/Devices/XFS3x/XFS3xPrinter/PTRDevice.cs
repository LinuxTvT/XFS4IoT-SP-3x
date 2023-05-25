using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS3xAPI;
using XFS3xAPI.PTR;
using XFS4IoTFramework.Printer;
using XFS4IoTServer;

using HRESULT = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using ULONG = System.UInt32;

namespace Printer.XFS3xPrinter
{
    public class PTRDevice : XfsService
    {
        public PTRDevice(string logicalName) : base(logicalName) { }

        public List<string> GetFormNameList()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PTR_FORM_LIST, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                return wfsResult.ReadLPSZ(2);
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
                return wfsResult.ReadLPSZ(2);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public Form QueryForm(string name, IPrinterDevice Device)
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

        public Field QueryField(string formName, string fieldName)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            using CommandData commandData = WFSPTRQUERYFIELD.BuildCommandData(formName,fieldName);

            GetInfo(CMD.WFS_INF_PTR_QUERY_FIELD, commandData.DataPtr, ref lpResult);
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
    }
}
