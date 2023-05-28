using XFS4IoT.Completions;
using DWORD = System.UInt32;
using HAPP = System.IntPtr;
using HRESULT = System.Int32;
using HSERVICE = System.UInt16;
using HWND = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using REQUESTID = System.UInt32;

namespace XFS3xAPI
{
    public class XfsService
    {
        // Properties for check 1 times init SDK only
        private static bool s_isSDKInitialized = false;
        private static readonly object s_initSDKLock = new();

        private static HWNDMessageHandle? s_hWNDMessageHandle;

        private static HWND s_hWND;

        /// <summary>
        /// NLog logger for this class
        /// </summary>
        public readonly NLog.Logger Logger;

        /// <summary>
        /// XFS SDK Manager version required
        /// </summary>
        public static DWORD SDKVersionsRequired { get; set; } = 0x00030003;

        /// <summary>
        /// XFS using version
        /// </summary>
        public static WFSVERSION SDKVersion = new();

        private delegate void XFSResultHandle(ref WFSRESULT xfsResult);
        private event XFSResultHandle? WFS_EXECUTE_COMPLETE;
        private event XFSResultHandle? WFS_SERVICE_EVENT;
        private event XFSResultHandle? WFS_EXECUTE_EVENT;

        private HRESULT _hCompleteResult;
        public readonly AutoResetEvent ExecuteCompleteEvent = new(false);
        public HRESULT LastCompleteResult => _hCompleteResult;
        public MessagePayload.CompletionCodeEnum LastCompletionCode => RESULT.ToCompletionCode(_hCompleteResult);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="timouet"></param>
        /// <returns></returns>
        public static async Task<bool> WaitOne(WaitHandle handle, int timouet = -1) => await Task.Run(() => { return handle.WaitOne(timouet); });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handles"></param>
        /// <param name="timouet"></param>
        /// <returns></returns>
        public static async Task<int> WaitAny(WaitHandle[] handles, int timouet = -1) => await Task.Run(() => { return WaitHandle.WaitAny(handles, timouet); });

        /// <summary>
        /// Hiden form to reciev all XFS WINDOWS message for all service 
        /// </summary>
        internal class HWNDMessageHandle : Form
        {
            private static readonly NLog.Logger s_logger = NLog.LogManager.GetLogger("WM_HANDLE");

            private static bool IsXFSMessage(int msg)
            {
                return (msg > MSG.WM_USER && msg < MSG.WFS_TIMER_EVENT);
            }
            /// <summary>
            /// Dictionary to map service handle (HSERVICE) with service object
            /// </summary>
            private Dictionary<HSERVICE, XfsService> _registeredService = new();

            public static void CreateInstance()
            {
                using (ManualResetEvent mre = new(false))
                {
                    Thread t = new((ThreadStart)delegate
                    {
                        s_hWNDMessageHandle = new HWNDMessageHandle();
                        s_hWND = s_hWNDMessageHandle.Handle.ToInt32();
                        mre.Set();
                        Application.Run();
                    })
                    {
                        IsBackground = true
                    };

                    // Starting thread
                    t.Start();

                    // Wait for thread started
                    mre.WaitOne();
                }

            }

            public void AddService(XfsService service)
            {
                _registeredService.Add(service._hService, service);
            }

            public void RemoveService(HSERVICE hService)
            {
                if (_registeredService.ContainsKey(hService))
                {
                    _registeredService.Remove(hService);
                }
                else
                {
                    s_logger.Error($"Can not file HSERVICE [{hService}] to remove");
                }
            }

            protected override void WndProc(ref Message m)
            {
                s_logger.Debug($"WndProc MESSAGE [{MSG.ToString(m.Msg)}]");

                if (IsXFSMessage(m.Msg))
                {
                    if (m.LParam != IntPtr.Zero)
                    {
                        using ResultData resultData = new(m.LParam);
                        WFSRESULT wfsResult = resultData.ReadResult();
                        if (_registeredService.ContainsKey(wfsResult.hService))
                        {
                            var service = _registeredService[wfsResult.hService];
                            switch (m.Msg)
                            {
                                case MSG.WFS_EXECUTE_COMPLETE:
                                    service.WFS_EXECUTE_COMPLETE?.Invoke(ref wfsResult);
                                    break;

                                case MSG.WFS_EXECUTE_EVENT:
                                    service.WFS_EXECUTE_EVENT?.Invoke(ref wfsResult);
                                    break;

                                case MSG.WFS_USER_EVENT:
                                    Console.WriteLine($"WFS_USER_EVENT");
                                    break;

                                case MSG.WFS_SYSTEM_EVENT:
                                    Console.WriteLine($"WFS_SYSTEM_EVENT");
                                    break;

                                case MSG.WFS_SERVICE_EVENT:
                                    service.WFS_SERVICE_EVENT?.Invoke(ref wfsResult);
                                    break;

                                default:
                                    s_logger.Warn($"UNHANDLE XFS Msg: {m.Msg}");
                                    break;
                            }
                        }
                        else
                        {
                            s_logger.Error($"Can not find [{nameof(XfsService)}] for hService[{wfsResult.hService}]");
                        }
                    }
                    else
                    {
                        throw new NullResultException();
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
        }

        protected virtual void HandleServiceEvent(ref WFSRESULT xfsResult)
        {
        }

        protected virtual void HandleExecuteEvent(ref WFSRESULT xfsResult)
        {
        }

        protected virtual void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            _hCompleteResult = xfsResult.hResult;
            ExecuteCompleteEvent.Set();
        }

        private bool SDKInit()
        {
            Logger.Info("Initialize 3.x XFS SDK");
            lock (s_initSDKLock)
            {
                if (!s_isSDKInitialized)
                {
                    var funcInfo = $"Call {nameof(API.WFSStartUp)}: Versions Required[{WFSVERSION.VersionRequireString(SDKVersionsRequired)}]";
                    Logger.Debug(funcInfo);
                    var result = API.WFSStartUp(SDKVersionsRequired, ref SDKVersion);
                    Logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
                    if (result == RESULT.WFS_SUCCESS)
                    {
                        HWNDMessageHandle.CreateInstance();
                        Logger.Info($"Init WFS SDK sucess, version: {SDKVersion.ToString()}");
                        s_isSDKInitialized = true;
                        return true;
                    }
                    else
                    {
                        Logger.Error($"Init WFS SDK ERROR: [{RESULT.ToString(result)}]");
                        return false;
                    }
                }
                else
                {
                    Logger.Info($"SDK was initialized");
                    return true;
                }
            }
        }

        public XfsService(string logicalServiceName)
        {
            WFS_EXECUTE_COMPLETE += HandleCompleteEvent;
            WFS_SERVICE_EVENT += HandleServiceEvent;
            WFS_EXECUTE_EVENT += HandleExecuteEvent;
            _logicalServiceName = logicalServiceName;

            Logger = NLog.LogManager.GetLogger(logicalServiceName);

            // Init SDK 3.x
            Logger.Info("Init XFS SDK 3.x");
            if (!SDKInit())
            {
                var erro_msg = $"Init SDK ERROR";
                Logger.Error(erro_msg);
                throw new Xfs3xException(RESULT.WFS_ERR_INTERNAL_ERROR, erro_msg);
            }
            else
            {
                // Create Application handle for service
                var result = API.WFSCreateAppHandle(ref _hApp);
                if (result == RESULT.WFS_SUCCESS)
                {
                    // Create Application handle success => Connect to SP service
                    Logger.Info($"Create App Handle success, hApp[{_hApp}]");
                    if (Connect())
                    {
                        Logger.Info($"Connect to SP success, hService[{_hService}]");
                    }
                    else
                    {
                        var erro_msg = $"Connect to SP ERROR";
                        Logger.Error(erro_msg);
                        throw new Xfs3xException(RESULT.WFS_ERR_INTERNAL_ERROR, erro_msg);
                    }
                }
                else
                {
                    var erro_msg = $"Create App Handle ERROR, code {result}";
                    Logger.Error(erro_msg);
                    throw new Xfs3xException(RESULT.WFS_ERR_INTERNAL_ERROR, erro_msg);
                }
            }
        }

        private bool Connect()
        {
            Logger.Info($"Call {nameof(API.WFSOpen)}: Logical Service[{_logicalServiceName}], Trace Level[{TraceLevel}]");
            var openResult = API.WFSOpen(_logicalServiceName, _hApp, nameof(XfsService), TraceLevel, OpenTimeOut, SrvcVersionsRequired, ref _spVersion, ref _spiVersion, ref _hService);
            Logger.Info($"Call {nameof(API.WFSOpen)}: => [{RESULT.ToString(openResult)}]");
            if (openResult == RESULT.WFS_SUCCESS)
            {
                Logger.Info($"Open service success, service handle is: {_hService} - ver: {_spiVersion.ToString()}/{_spVersion.ToString()}");
                Logger.Info($"Call {nameof(API.WFSRegister)}: Event classes[{EVENT.CLASSES.ToString(EventClass)}], HWND[{s_hWND}]");
                var registerResult = API.WFSRegister(_hService, EventClass, s_hWND);
                Logger.Info($"Call {nameof(API.WFSRegister)}: => [{RESULT.ToString(registerResult)}]");
                if (registerResult == RESULT.WFS_SUCCESS)
                {
                    s_hWNDMessageHandle?.AddService(this);
                    Logger.Info("Open XFS service success");
                }
                else
                {
                    Logger.Error("Reqister handle EVENT Error");
                    return false;
                }
            }
            else
            {
                Logger.Error("Open XFS service ERROR");
                return false;
            }
            return true;
        }

        public void GetInfo(DWORD dwCategory, LPVOID lpQueryDetails, ref LPWFSRESULT lppResult, DWORD dwTimeOut = 0)
        {
            var timeOut = dwTimeOut;
            if (timeOut == 0)
            {
                timeOut = GetInfoTimeOutDefault;
            }
            var funcInfo = $"Call {nameof(API.WFSGetInfo)}: Category[{dwCategory}], TimeOut[{timeOut}]";
            Logger.Debug(funcInfo);
            var result = API.WFSGetInfo(_hService, dwCategory, lpQueryDetails, timeOut, ref lppResult);
            Logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
            if (result != RESULT.WFS_SUCCESS)
            {
                if (lppResult != LPWFSRESULT.Zero)
                {
                    _ = API.WFSFreeResult(lppResult);
                    lppResult = LPWFSRESULT.Zero;
                }
                throw new Xfs3xException(result, "WFSGetInfo ERROR");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCommand"></param>
        /// <param name="lpCmdData"></param>
        /// <param name="dwTimeOut"></param>
        /// <exception cref="Xfs3xException"></exception>
        public void AsyncExecute(DWORD dwCommand, LPVOID lpCmdData, DWORD dwTimeOut = 0)
        {
            var timeOut = dwTimeOut;
            if (timeOut == 0)
            {
                timeOut = ExecuteTimeOutDefault;
            }
            var funcInfo = $"Call {nameof(API.WFSAsyncExecute)}: Command[{dwCommand}], CmdData [{lpCmdData}], TimeOut[{timeOut}]";
            Logger.Debug(funcInfo);
            var result = API.WFSAsyncExecute(_hService, dwCommand, lpCmdData, timeOut, s_hWND, ref _curRequestID);
            Logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
            if (result != RESULT.WFS_SUCCESS)
            {
                throw new Xfs3xException(result, "Async Execute ERROR");
            }
        }

        public void Execute(DWORD dwCommand, LPVOID lpCmdData, ref LPWFSRESULT lpWFSRESULT, DWORD dwTimeOut = 0)
        {
            var timeOut = dwTimeOut;
            if (timeOut == 0)
            {
                timeOut = ExecuteTimeOutDefault;
            }
            var funcInfo = $"Call {nameof(Execute)} CMD=[{dwCommand}], TimeOut=[{dwTimeOut}]";
            Logger.Debug(funcInfo);
            var result = API.WFSExecute(_hService, dwCommand, lpCmdData, timeOut, ref lpWFSRESULT);
            Logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
            if (result != RESULT.WFS_SUCCESS)
            {
                if (lpWFSRESULT != LPWFSRESULT.Zero)
                {
                    _ = API.WFSFreeResult(lpWFSRESULT);
                    lpWFSRESULT = LPWFSRESULT.Zero;
                }
                throw new Xfs3xException(result, funcInfo);
            }
        }

        private HAPP _hApp = HAPP.Zero;
        private WFSVERSION _spVersion;
        private WFSVERSION _spiVersion;
        private HSERVICE _hService;

        public DWORD TraceLevel { get; set; } = STRACE_LEVEL.All;
        public DWORD EventClass { get; set; } = EVENT.CLASSES.All;
        public DWORD OpenTimeOut { get; set; } = 10000;
        public DWORD ExecuteTimeOutDefault { get; set; } = 10000;
        public DWORD GetInfoTimeOutDefault { get; set; } = 10000;
        public DWORD SrvcVersionsRequired { get; set; } = 0x00030403;

        private REQUESTID _curRequestID;
        private string _logicalServiceName;

    }
}
