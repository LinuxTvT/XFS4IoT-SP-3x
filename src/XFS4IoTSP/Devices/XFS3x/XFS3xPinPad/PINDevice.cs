using System.Collections.Concurrent;
using XFS3xAPI;
using XFS3xAPI.PIN;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.PinPad;
using XFS4IoTServer;
using HRESULT = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using ULONG = System.UInt32;

namespace XFS3xPinPad
{
    public class PINDevice : XfsService
    {
        private HRESULT _hCompleteResult;

        public DeviceResult LastResetDeviceResult => new(RESULT.ToCompletionCode(_hCompleteResult));

        public PINDevice(string logicalName) : base(logicalName) { }

        protected override void HandleServiceEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_SRVE_PIN_ILLEGAL_KEY_ACCESS:
                    WFSPINACCESS wFSPINACCESS = xfsResult.ReadPINAccess();
                    Console.WriteLine($"WFSPINACCESS ERROR Event: [{wFSPINACCESS.lErrorCode}]");
                    break;
                default:
                    Logger.Error($"Unhandle Service Event: [{(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Service Event: [{(xfsResult.dwEventID)}]");
                    break;
            }
        }

        protected override void HandleExecuteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_EXEE_PIN_KEY:
                    if (xfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPINKEY pinKey = xfsResult.ReadPINKey();
                        _bcKeyInput?.Add(pinKey);
                    }
                    else
                    {
                        Logger.Error($"lpBuffer is NULL in {nameof(WFSRESULT)}[{xfsResult}]");
                    }
                    break;
                default:
                    Logger.Error($"Unhandle Execute Event: [{(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Execute Event: [{(xfsResult.dwEventID)}]");
                    break;
            }
        }

        protected override void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            _hCompleteResult = xfsResult.hResult;
            Logger.Debug($"HandleCompleteEvent: [{xfsResult.dwCommandCode}]=[{RESULT.ToString(_hCompleteResult)}]");
            switch (xfsResult.dwCommandCode)
            {
                case CMD.WFS_CMD_PIN_GET_DATA:
                    if (_hCompleteResult == RESULT.WFS_SUCCESS)
                    {
                        if (xfsResult.lpBuffer != LPVOID.Zero)
                        {
                            WFSPINDATA pinData = xfsResult.ReadPinData();
                            DataEntryResult = new(RESULT.ToCompletionCode(_hCompleteResult),
                                                    pinData.usKeys,
                                                    pinData.ReadEnteredKeys(),
                                                    WCompletion.ToEnum(pinData.wCompletion));
                        }
                        else
                        {
                            Logger.Error($"lpBuffer is NULL in {nameof(WFSRESULT)}[{xfsResult}]");
                        }
                    }
                    else
                    {
                        Logger.Error($"Command {nameof(CMD.WFS_CMD_PIN_GET_DATA)} completion error [{RESULT.ToString(_hCompleteResult)}]");
                    }
                    _bcKeyInput?.CompleteAdding();
                    break;

                case CMD.WFS_CMD_PIN_GET_PIN:
                    if (_hCompleteResult == RESULT.WFS_SUCCESS)
                    {
                        if (xfsResult.lpBuffer != LPVOID.Zero)
                        {
                            WFSPINENTRY pinData = xfsResult.ReadPinEntry();
                            PinEntryResult = new(RESULT.ToCompletionCode(_hCompleteResult),
                                                    pinData.usDigits,
                                                    WCompletion.ToEnum(pinData.wCompletion));
                        }
                    }
                    else
                    {
                        Logger.Error($"Command {nameof(CMD.WFS_CMD_PIN_GET_PIN)} completion error [{RESULT.ToString(_hCompleteResult)}]");
                    }
                    _bcKeyInput?.CompleteAdding();

                    break;
                case CMD.WFS_CMD_PIN_SECUREKEY_ENTRY:
                    if (_hCompleteResult == RESULT.WFS_SUCCESS)
                    {
                        if (xfsResult.lpBuffer != LPVOID.Zero)
                        {
                            var dataResult = WFSPINSECUREKEYENTRYOUT.ReadStruct(xfsResult.lpBuffer);

                            SecureKeyEntryResult = new(RESULT.ToCompletionCode(_hCompleteResult),
                                                    dataResult.usDigits,
                                                    dataResult.Completion,
                                                    dataResult.KeyCheckValue);
                        }
                    }
                    else
                    {
                        var error = $"Command {nameof(CMD.WFS_CMD_PIN_GET_PIN)} completion error [{RESULT.ToString(_hCompleteResult)}]";
                        Logger.Error(error);
                        SecureKeyEntryResult = new(RESULT.ToCompletionCode(_hCompleteResult), error);
                    }
                    _bcKeyInput?.CompleteAdding();

                    break;
                case CMD.WFS_CMD_PIN_RESET:
                    ExecuteCompleteEvent.Set();
                    break;
                default:
                    Logger.Error($"Unhanlde Complete CMD [{(xfsResult.dwCommandCode)}]");
                    Console.WriteLine($"Unhanlde Complete CMD [{(xfsResult.dwCommandCode)}]");
                    break;
            }
        }

        private BlockingCollection<WFSPINKEY>? _bcKeyInput;
        public DataEntryResult? DataEntryResult { get; private set; } = null;
        public PinEntryResult? PinEntryResult { get; private set; } = null;
        public SecureKeyEntryResult? SecureKeyEntryResult { get; private set; } = null;

        public void Reset()
        {
            AsyncExecute(CMD.WFS_CMD_PIN_RESET, LPVOID.Zero);
        }

        public void UpdateStatus(CommonStatusClass status, KeyManagementStatusClass keyManagementStatus, KeyboardStatusClass keyboardStatus)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_PIN_STATUS, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    using ResultData resultData = new(lpResult);
                    WFSRESULT wfsResult = resultData.ReadResult();
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPINSTATUS_310 wfsPINStatus = wfsResult.ReadPinStatus310();

                        status.Device = FwDevice.ToEnum(wfsPINStatus.fwDevice);
                        keyManagementStatus.EncryptionState = FwEncStat.ToEnum(wfsPINStatus.fwEncStat);
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
            catch
            {
                status.Device = CommonStatusClass.DeviceEnum.PotentialFraud;
                throw;
            }
        }

        public void GetData(DataEntryRequest request, BlockingCollection<WFSPINKEY> bcKeyInput)
        {
            _bcKeyInput = bcKeyInput;
            DataEntryResult = null;

            using var commandData = WFSPINGETDATA.BuildCommandData(request);
            AsyncExecute(CMD.WFS_CMD_PIN_GET_DATA, commandData.DataPtr, 100000);
        }

        public void GetPIN(PinEntryRequest request, BlockingCollection<WFSPINKEY> bcKeyInput)
        {
            _bcKeyInput = bcKeyInput;
            DataEntryResult = null;

            using var commandData = WFSPINGETPIN.BuildCommandData(request);
            AsyncExecute(CMD.WFS_CMD_PIN_GET_PIN, commandData.DataPtr, 100000);
        }

        public List<byte> GetPINBlock(PINBlockRequest request)
        {
            using var commandData = WFSPINBLOCK.BuildCommandData(request);

            LPWFSRESULT lpResult = LPVOID.Zero;

            Execute(CMD.WFS_CMD_PIN_GET_PINBLOCK, commandData.DataPtr, ref lpResult, 100000);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                WFSXDATA wfsXData = wfsResult.Data<WFSXDATA>(out bool isNull);
                return wfsXData.ReadData();
            }
            else
            {
                throw new NullResultException();
            }
        }

        public Dictionary<string, KeyDetail> GetKeyDetail()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PIN_KEY_DETAIL, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                return WFSPINKEYDETAIL.ReadDetails(ref wfsResult);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public void GetFunctionKeys(ref List<FrameClass.FunctionKeyClass> fKeys,
                                    ref List<FrameClass.FunctionKeyClass> leftFDKs,
                                    ref List<FrameClass.FunctionKeyClass> rightFDKs)
        {
            ULONG lpulFDKMask = 0xFFFFFFFF;
            using var commandData = new CommandData(lpulFDKMask);
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PIN_FUNCKEY_DETAIL, commandData.DataPtr, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                WFSPINFUNCKEYDETAIL.ReadFuncKeyDetail(ref wfsResult, ref fKeys, ref leftFDKs, ref rightFDKs);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public void GetSecureKeys()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;

            GetInfo(CMD.WFS_INF_PIN_SECUREKEY_DETAIL, LPVOID.Zero, ref lpResult);
            if (lpResult != LPWFSRESULT.Zero)
            {
                using ResultData resultData = new(lpResult);
                WFSRESULT wfsResult = resultData.ReadResult();
                WFSPINSECUREKEYDETAIL.ReadSecureKeyDetail(ref wfsResult);
            }
            else
            {
                throw new NullResultException();
            }
        }

        public void SecureKeyEntry(SecureKeyEntryRequest request, BlockingCollection<WFSPINKEY> bcKeyInput)
        {
            _bcKeyInput = bcKeyInput;
            DataEntryResult = null;

            using var commandData = WFSPINSECUREKEYENTRY.BuildCommandData(request);
            AsyncExecute(CMD.WFS_CMD_PIN_SECUREKEY_ENTRY, commandData.DataPtr, 100000);
        }
    }
}
