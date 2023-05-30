using XFS3xAPI;
using XFS3xAPI.SIU;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTServer;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;

namespace Lights.XFS3xLights
{

    public class SIUDevice : XfsService
    {
        public delegate void PortEventDelegate(ref WFSSIUPORTEVENT300 eventData);

        protected static CommonStatusClass SIUCommonStatus = new CommonStatusClass(Device: CommonStatusClass.DeviceEnum.NoDevice,
                                                 DevicePosition: CommonStatusClass.PositionStatusEnum.InPosition,
                                                 PowerSaveRecoveryTime: 0,
                                                 AntiFraudModule: CommonStatusClass.AntiFraudModuleEnum.NotSupported,
                                                 Exchange: CommonStatusClass.ExchangeEnum.NotSupported,
                                                 EndToEndSecurity: CommonStatusClass.EndToEndSecurityEnum.NotSupported);

        protected static LightsStatusClass SIULightsStatus = new();

        protected static LightsCapabilitiesClass? SIULightsCapabilities = null; 

        protected static AuxiliariesCapabilities? SIUAuxiliariesCapabilities = null;

        private AutoResetEvent _shareDeviceFlags = new(true);

        private bool _isShareDevice = true;

        
        public event PortEventDelegate? PortEvent;

        public SIUDevice(string logicalServiceName) : base(logicalServiceName)
        {
            if(_shareDeviceFlags.WaitOne(10))
            {
                //SIULightsCapabilities = GetCapabilities();
                GetCapabilities();
                
                _isShareDevice = false;
            }
        }

        public void GetCapabilities()
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
                    SIULightsCapabilities =  new LightsCapabilitiesClass(wfsCaps.Lights());
                    SIUAuxiliariesCapabilities = wfsCaps.GetAuxiliaries();
                    EnableAllEvent(ref wfsCaps);
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

        public void UpdateStatus()
        {
            if(_isShareDevice)
            {
                return;
            }
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
                        SIUCommonStatus.Device = FwDevice.ToEnum(wfsStatus.fwDevice);
                        //commonStatus.DevicePosition = WDevicePosition.ToEnum(wfsStatus.wDevicePosition);

                        SIULightsStatus.Status = wfsStatus.LightsStatus();
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
                SIUCommonStatus.Device = CommonStatusClass.DeviceEnum.PotentialFraud;
                throw;
            }
        }

        public void EnableAllEvent(ref WFSSIUCAPS300 caps)
        {
            using var commandData = WFSSIUENABLE.BuildCommandData(ref caps);
            AsyncExecute(CMD.WFS_CMD_SIU_ENABLE_EVENTS, commandData.DataPtr, 100000);
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
                case EVENT.WFS_SRVE_SIU_PORT_STATUS:
                    var portStatus = xfsResult.ReadSIUPortEvent300();
                    PortEvent?.Invoke(ref portStatus);
                    Console.WriteLine($"ERR: {portStatus.ToString()}");
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
                case EVENT.WFS_EXEE_SIU_PORT_ERROR:
                    var portError = xfsResult.ReadSIUPortError();
                    Console.WriteLine($"ERR: {portError.ToString()}");
                    break;
                default:
                    Logger.Error($"Unhandle Execute Event: [{(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Execute Event: [{(xfsResult.dwEventID)}]");
                    break;
            }
            base.HandleExecuteEvent(ref xfsResult);
        }
    }
}
