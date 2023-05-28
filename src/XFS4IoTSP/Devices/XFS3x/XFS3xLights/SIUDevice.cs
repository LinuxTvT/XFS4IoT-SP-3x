using XFS3xAPI;
using XFS3xAPI.SIU;
using XFS4IoT.Printer.Events;
using XFS4IoTFramework.Common;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;

namespace Lights.XFS3xLights
{
    public class SIUDevice : XfsService
    {
        private static LightsCapabilitiesClass.Light _light = new LightsCapabilitiesClass.Light((
                                                                                        LightsCapabilitiesClass.FlashRateEnum.Continuous |
                                                                                        LightsCapabilitiesClass.FlashRateEnum.Medium |
                             LightsCapabilitiesClass.FlashRateEnum.Quick |
                             LightsCapabilitiesClass.FlashRateEnum.Slow |
                             LightsCapabilitiesClass.FlashRateEnum.Off),
                            LightsCapabilitiesClass.ColorEnum.Default,
                            LightsCapabilitiesClass.DirectionEnum.NotSupported,
                            LightsCapabilitiesClass.LightPostionEnum.Center);
        public SIUDevice(string logicalServiceName) : base(logicalServiceName)
        {
        }

        public LightsCapabilitiesClass? GetCapabilities()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_SIU_CAPABILITIES, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                if (wfsResult.lpBuffer != LPVOID.Zero)
                {
                    WFSSIUCAPS300 wfsCaps = wfsResult.ReadSIUCaps300();
                    return new LightsCapabilitiesClass(wfsCaps.Lights());
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

        public void UpdateStatus(CommonStatusClass commonStatus, LightsStatusClass lightsStatus)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_SIU_STATUS, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    using ResultData resultData = new(lpResult);
                    WFSRESULT wfsResult = resultData.ReadResult();
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSSIUSTATUS300 wfsStatus = wfsResult.ReadSIUStatus300();

                        // Common status
                        commonStatus.Device = FwDevice.ToEnum(wfsStatus.fwDevice);
                        //commonStatus.DevicePosition = WDevicePosition.ToEnum(wfsStatus.wDevicePosition);

                        lightsStatus.Status = wfsStatus.LightsStatus();
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

        public void SetLight(LightsCapabilitiesClass.DeviceEnum device, LightsStatusClass.LightOperation operation)
        {
            using var commandData = WFSSIUSETGUIDLIGHT.BuildCommandData(device, operation);
            AsyncExecute(CMD.WFS_CMD_SIU_SET_GUIDLIGHT, commandData.DataPtr, 100000);
        }

        protected override void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"HandleCompleteEvent: [{xfsResult.dwCommandCode}]=[{RESULT.ToString(xfsResult.hResult)}]");
            switch (xfsResult.dwCommandCode)
            {
                case CMD.WFS_CMD_SIU_SET_GUIDLIGHT:
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
