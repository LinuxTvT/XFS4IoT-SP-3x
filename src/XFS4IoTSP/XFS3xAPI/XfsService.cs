﻿using System.Runtime.InteropServices;
using XFS3xAPI.IDC;
using DWORD = System.UInt32;
using HAPP = System.IntPtr;
using HSERVICE = System.UInt16;
using HWND = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using REQUESTID = System.UInt32;
using WORD = System.UInt16;

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
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// XFS SDK Manager version required
        /// </summary>
        public static DWORD SDKVersionsRequired { get; set; } = 0x00030003;

        /// <summary>
        /// XFS using version
        /// </summary>
        public static WFSVERSION SDKVersion = new();

        public delegate void XFSResultHandle(ref WFSRESULT xfsResult);
        public event XFSResultHandle? WFS_EXECUTE_COMPLETE;
        public event XFSResultHandle? WFS_SERVICE_EVENT;
        public event XFSResultHandle? WFS_EXECUTE_EVENT;

        /// <summary>
        /// Hiden form to reciev all XFS WINDOWS message for all service 
        /// </summary>
        internal class HWNDMessageHandle : Form
        {
            private static readonly NLog.Logger s_logger = NLog.LogManager.GetLogger("WM_HANDLE");
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
                if(_registeredService.ContainsKey(hService)) {
                    _registeredService.Remove(hService);
                } else
                {
                    s_logger.Error($"Can not file HSERVICE [{hService}] to remove");
                }
            }

            protected override void WndProc(ref Message m)
            {
                s_logger.Debug($"WndProc MESSAGE [{MSG.ToString(m.Msg)}]");
                try
                {
                    switch (m.Msg)
                    {
                        case MSG.WFS_EXECUTE_COMPLETE:
                            WFSRESULT completeResult = Marshal.PtrToStructure<WFSRESULT>(m.LParam);
                            s_logger.Debug($"WFS_EXECUTE_COMPLETE, RESULT =[{completeResult}]");
                            if (_registeredService.ContainsKey(completeResult.hService))
                            {
                                var service = _registeredService[completeResult.hService];
                                service.WFS_EXECUTE_COMPLETE?.Invoke(ref completeResult);
                            }
                            else
                            {
                                s_logger.Error($"Can not find [{nameof(XfsService)}] for hService[{completeResult.hService}]");
                            }
                            break;

                        case MSG.WFS_EXECUTE_EVENT:
                            WFSRESULT executeEventResult = Marshal.PtrToStructure<WFSRESULT>(m.LParam);
                            s_logger.Debug($"WFS_SERVICE_EVENT, RESULT =[{executeEventResult}]");
                            if (_registeredService.ContainsKey(executeEventResult.hService))
                            {
                                var service = _registeredService[executeEventResult.hService];
                                service.WFS_EXECUTE_EVENT?.Invoke(ref executeEventResult);
                            }
                            else
                            {
                                s_logger.Error($"Can not find [{nameof(XfsService)}] for hService[{executeEventResult.hService}]");
                            }
                            break;

                        case MSG.WFS_USER_EVENT:
                            Console.WriteLine($"WFS_USER_EVENT");
                            break;

                        case MSG.WFS_SYSTEM_EVENT:
                            Console.WriteLine($"WFS_SYSTEM_EVENT");
                            break;

                        case MSG.WFS_SERVICE_EVENT:
                            WFSRESULT serviceEventResult = Marshal.PtrToStructure<WFSRESULT>(m.LParam);
                            s_logger.Debug($"WFS_SERVICE_EVENT, RESULT =[{serviceEventResult}]");
                            if (_registeredService.ContainsKey(serviceEventResult.hService))
                            {
                                var service = _registeredService[serviceEventResult.hService];
                                service.WFS_SERVICE_EVENT?.Invoke(ref serviceEventResult);
                            }
                            else
                            {
                                s_logger.Error($"Can not find [{nameof(XfsService)}] for hService[{serviceEventResult.hService}]");
                            }
                            break;

                        case MSG.WFS_TIMER_EVENT:
                            Console.WriteLine($"WFS_TIMER_EVENT");
                            break;

                        case MSG.WM_CREATE:
                            s_logger.Info($"Windows created, handle [{Handle}]");
                            base.WndProc(ref m);
                            break;

                        default:
                            if (m.Msg > MSG.WM_USER && m.Msg < MSG.WM_USER_MAX)
                            {
                                s_logger.Warn($"UNHANDLE Msg: {m.Msg}");
                            }
                            else
                            {
                                //s_logger.Debug($"Call default {nameof(WndProc)}");
                                base.WndProc(ref m);
                            }
                            break;
                    }
                }
                finally
                {
                    switch (m.Msg)
                    {
                        case MSG.WFS_EXECUTE_COMPLETE:
                        case MSG.WFS_EXECUTE_EVENT:
                        case MSG.WFS_SERVICE_EVENT:
                            FreeWFSResult(m.LParam);
                            break;
                        default:
                            break;
                    }

                }
            }

        }

        private bool SDKInit()
        {
            s_logger.Info("Initialize 3.x XFS SDK");
            lock (s_initSDKLock)
            {
                if (!s_isSDKInitialized)
                {
                    var funcInfo = $"Call {nameof(API.WFSStartUp)}: Versions Required[{API.VersionString((WORD)(SDKVersionsRequired >> 16))}-{API.VersionString((WORD)(SDKVersionsRequired & 0x0000FFFF))}]";
                    s_logger.Debug(funcInfo);
                    var result = API.WFSStartUp(SDKVersionsRequired, ref SDKVersion);
                    s_logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
                    if (result == RESULT.WFS_SUCCESS)
                    {
                        HWNDMessageHandle.CreateInstance();
                        s_logger.Info($"Init WFS SDK sucess, version: {SDKVersion.VersionString}");
                        s_isSDKInitialized = true;
                        return true;
                    }
                    else
                    {
                        s_logger.Error($"Init WFS SDK ERROR: [{RESULT.ToString(result)}]");
                        return false;
                    }
                }
                else
                {
                    s_logger.Info($"SDK was initialized");
                    return true;
                }
            }
        }

        public XfsService(string logicalServiceName)
        {
            _logicalServiceName = logicalServiceName;

            // Init SDK
            if (!SDKInit())
            {
                var erro_msg = $"Init SDK ERROR";
                s_logger.Error(erro_msg);
                throw new Xfs3xException(RESULT.WFS_ERR_INTERNAL_ERROR, erro_msg);
            }
            else
            {
                var result = API.WFSCreateAppHandle(ref _hApp);
                if (result == RESULT.WFS_SUCCESS)
                {
                    s_logger.Info($"Create App Handle success: {_hApp}");
                }
                else
                {
                    var erro_msg = $"Create App Handle ERROR, code {result}";
                    s_logger.Error(erro_msg);
                    throw new Xfs3xException(RESULT.WFS_ERR_INTERNAL_ERROR, erro_msg);
                }
            }
        }

        public void Connect()
        {
            s_logger.Info($"Call {nameof(API.WFSOpen)}: Logical Service[{_logicalServiceName}], Trace Level[{TraceLevel}]");
            var openResult = API.WFSOpen(_logicalServiceName, _hApp, nameof(XfsService), TraceLevel, OpenTimeOut, SrvcVersionsRequired, ref _spVersion, ref _spiVersion, ref _hService);
            s_logger.Info($"Call {nameof(API.WFSOpen)}: => [{RESULT.ToString(openResult)}]");
            if (openResult == RESULT.WFS_SUCCESS)
            {
                s_logger.Info($"Open service success, service handle is: {_hService} - ver: {_spiVersion.VersionString}/{_spVersion.VersionString}");
                s_logger.Info($"Call {nameof(API.WFSRegister)}: Event classes[{EVENT.CLASSES.ToString(EventClass)}], HWND[{s_hWND}]");
                var registerResult = API.WFSRegister(_hService, EventClass, s_hWND);
                s_logger.Info($"Call {nameof(API.WFSRegister)}: => [{RESULT.ToString(registerResult)}]");
                if (registerResult == RESULT.WFS_SUCCESS)
                {
                    s_hWNDMessageHandle?.AddService(this);
                    s_logger.Info("Open XFS service success");
                }
                else
                {
                    string error = "Reqister handle EVENT Error";
                    s_logger.Error(error);
                    throw new Xfs3xException(registerResult, error);
                }
            }
            else
            {
                string error = "Open XFS service ERROR";
                s_logger.Error(error);
                throw new Xfs3xException(openResult, error);
            }
        }

        public void GetInfo(DWORD dwCategory, LPVOID lpQueryDetails, ref LPWFSRESULT lppResult, DWORD dwTimeOut = 0)
        {
            var timeOut = dwTimeOut;
            if (timeOut == 0)
            {
                timeOut = GetInfoTimeOutDefault;
            }
            var funcInfo = $"Call {nameof(API.WFSGetInfo)}: Category[{CMD.ToInfoCommandString(dwCategory)}], TimeOut[{timeOut}]";
            s_logger.Debug(funcInfo);
            var result = API.WFSGetInfo(_hService, dwCategory, lpQueryDetails, timeOut, ref lppResult);
            s_logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
            if (result != RESULT.WFS_SUCCESS)
            {
                throw new Xfs3xException(result, "Async Execute ERROR");
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
            var funcInfo = $"Call {nameof(API.WFSAsyncExecute)}: Command[{CMD.ToExecuteCommandString(dwCommand)}], TimeOut[{timeOut}]";
            s_logger.Debug(funcInfo);
            var result = API.WFSAsyncExecute(_hService, dwCommand, lpCmdData, timeOut, s_hWND, ref _curRequestID);
            s_logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
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
            s_logger.Debug(funcInfo);
            var result = API.WFSExecute(_hService, dwCommand, lpCmdData, timeOut, ref lpWFSRESULT);
            s_logger.Debug($"{funcInfo}  => [{RESULT.ToString(result)}]");
            if (result != RESULT.WFS_SUCCESS)
            {
                throw new Xfs3xException(result, funcInfo);
            }
        }

        public static void FreeWFSResult(LPWFSRESULT lpWfsResult)
        {
            if (lpWfsResult != LPWFSRESULT.Zero)
            {
                s_logger.Debug($"Free WFSRESULT: [{lpWfsResult}]");
                var result = API.WFSFreeResult(lpWfsResult);
                if (result != RESULT.WFS_SUCCESS)
                {
                    throw new Xfs3xException(result, "Free WFSRESULT");
                }
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
        public DWORD SrvcVersionsRequired { get; set; } = 0x04030403;

        private REQUESTID _curRequestID;
        private string _logicalServiceName;

    }
}