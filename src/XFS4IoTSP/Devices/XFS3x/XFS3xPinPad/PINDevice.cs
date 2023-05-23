using System.Collections.Concurrent;
using XFS3xAPI;
using XFS3xAPI.PIN;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.PinPad;
using XFS4IoTServer;
using EVENT = XFS3xAPI.PIN.EVENT;
using HRESULT = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using ULONG = System.UInt32;
using WORD = System.UInt16;

namespace XFS3xPinPad
{
    public class PINDevice : XfsService
    {
        private WORD _wResetOut;

        private HRESULT _hCompleteResult;

        public DeviceResult LastResetDeviceResult => new(RESULT.ToCompletionCode(_hCompleteResult));

        public PINDevice(string logicalName) : base(logicalName) { }

        public bool Init()
        {
            Logger.Info($"Init XFS Device 3.x service");
            try
            {
                Connect();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"Init XFS Device 3.x service ERROR: [{e.Message}]");
                return false;
            }
        }

        protected override void HandleServiceEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_SRVE_PIN_ILLEGAL_KEY_ACCESS:
                    WFSPINACCESS wFSPINACCESS = ShareMemory.ReadStructure<WFSPINACCESS>(xfsResult.lpBuffer);
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
                        WFSPINKEY pinKey = ShareMemory.ReadStructure<WFSPINKEY>(xfsResult.lpBuffer);
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
                            WFSPINDATA pinData = ShareMemory.ReadStructure<WFSPINDATA>(xfsResult.lpBuffer);
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
                            WFSPINENTRY pinData = ShareMemory.ReadStructure<WFSPINENTRY>(xfsResult.lpBuffer);
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
                    WFSRESULT wfsResult = ShareMemory.ReadResult(lpResult);
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPINSTATUS_310 wfsPINStatus = ShareMemory.ReadStructure<WFSPINSTATUS_310>(wfsResult.lpBuffer);

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
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

        public void GetData(DataEntryRequest request, BlockingCollection<WFSPINKEY> bcKeyInput)
        {
            _bcKeyInput = bcKeyInput;
            DataEntryResult = null;

            UlKeyMask.ParseActiveKeys(request.ActiveKeys, out ULONG activeFDKs, out ULONG activeKeys, out ULONG terminateFDKs, out ULONG terminateKeys);

            WFSPINGETDATA wfsPinGetData = new()
            {
                bAutoEnd = request.AutoEnd,
                usMaxLen = (ushort)request.MaxLen,
                ulActiveFDKs = activeFDKs,
                ulActiveKeys = activeKeys,
                ulTerminateFDKs = terminateFDKs,
                ulTerminateKeys = terminateKeys
            };

            LPVOID lpCommandData = ShareMemory.WriteStructure<WFSPINGETDATA>(ref wfsPinGetData);

            try
            {
                AsyncExecute(CMD.WFS_CMD_PIN_GET_DATA, lpCommandData, 100000);
            }
            finally
            {
                WFSPINGETDATA.Free(lpCommandData);
            }
        }

        public void GetPIN(PinEntryRequest request, BlockingCollection<WFSPINKEY> bcKeyInput)
        {
            _bcKeyInput = bcKeyInput;
            DataEntryResult = null;
            UlKeyMask.ParseActiveKeys(request.ActiveKeys, out ULONG activeFDKs, out ULONG activeKeys, out ULONG terminateFDKs, out ULONG terminateKeys);

            WFSPINGETPIN wfsPinGetPIN = new()
            {
                usMinLen = (ushort)request.MinLen,
                usMaxLen = (ushort)request.MaxLen,
                bAutoEnd = request.AutoEnd,
                cEcho = Convert.ToByte(request.Echo.First()),
                ulActiveFDKs = activeFDKs,
                ulActiveKeys = activeKeys,
                ulTerminateFDKs = terminateFDKs,
                ulTerminateKeys = terminateKeys
            };

            LPVOID lpCommandData = ShareMemory.WriteStructure<WFSPINGETPIN>(ref wfsPinGetPIN);

            try
            {
                AsyncExecute(CMD.WFS_CMD_PIN_GET_PIN, lpCommandData, 100000);
            }
            finally
            {
                WFSPINGETPIN.Free(lpCommandData);
            }
        }

        public List<byte> GetPINBlock(PINBlockRequest request)
        {
            WFSPINBLOCK wfsBINBLock = new()
            {
                lpsCustomerData = ShareMemory.WriteLPTRString(request.CustomerData),
                lpsXORData = ShareMemory.WriteLPTRString(request.XorData),
                bPadding = request.Padding,
                wFormat = FwPinFormats.FromEnum(request.Format),
                lpsKey = ShareMemory.WriteLPTRString(request.KeyName),
                lpsKeyEncKey = ShareMemory.WriteLPTRString(request.SecondEncKeyName)
            };

            LPVOID lpCommandData = ShareMemory.WriteStructure<WFSPINBLOCK>(ref wfsBINBLock);

            LPWFSRESULT lpResult = LPVOID.Zero;
            try
            {
                Execute(CMD.WFS_CMD_PIN_GET_PINBLOCK, lpCommandData, ref lpResult, 100000);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = ShareMemory.ReadResult(lpResult);
                    WFSXDATA wfsXData = wfsResult.Data<WFSXDATA>();
                    return wfsXData.ReadData();
                }
                else
                {
                    throw new NullResultException();
                }
            }
            finally
            {
                WFSPINBLOCK.Free(ref lpCommandData);
                ShareMemory.FreeResult(lpResult);
            }
        }

        public Dictionary<string, KeyDetail> GetKeyDetail()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_PIN_KEY_DETAIL, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = ShareMemory.ReadResult(lpResult);
                    return WFSPINKEYDETAIL.ReadDetails(ref wfsResult);
                }
                else
                {
                    throw new NullResultException();
                }
            }
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

        public void GetFunctionKeys(ref List<FrameClass.FunctionKeyClass> fKeys,
                                    ref List<FrameClass.FunctionKeyClass> leftFDKs,
                                    ref List<FrameClass.FunctionKeyClass> rightFDKs)
        {
            ULONG lpulFDKMask = 0xFFFFFFFF;
            LPVOID lpCommandData = ShareMemory.WriteStructure<ULONG>(ref lpulFDKMask);
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_PIN_FUNCKEY_DETAIL, lpCommandData, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = ShareMemory.ReadResult(lpResult);
                    WFSPINFUNCKEYDETAIL.ReadFuncKeyDetail(ref wfsResult, ref fKeys, ref leftFDKs, ref rightFDKs);
                }
                else
                {
                    throw new NullResultException();
                }
            }
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

        public void GetSecureKeys()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_PIN_SECUREKEY_DETAIL, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = ShareMemory.ReadResult(lpResult);
                    WFSPINSECUREKEYDETAIL.ReadSecureKeyDetail(ref wfsResult);
                }
                else
                {
                    throw new NullResultException();
                }
            }
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

        public void SecureKeyEntry(SecureKeyEntryRequest request, BlockingCollection<WFSPINKEY> bcKeyInput)
        {
            _bcKeyInput = bcKeyInput;
            DataEntryResult = null;

            UlKeyMask.ParseActiveKeys(request.ActiveKeys, out ULONG activeFDKs, out ULONG activeKeys, out ULONG terminateFDKs, out ULONG terminateKeys);

            WFSPINSECUREKEYENTRY commandData = new()
            {
                usKeyLen = (ushort)request.KeyLen,
                bAutoEnd = request.AutoEnd,
                ulActiveFDKs = activeFDKs,
                ulActiveKeys = activeKeys,
                ulTerminateFDKs = terminateFDKs,
                ulTerminateKeys = terminateKeys,
                wVerificationType = WVerificationType.FromEnum(request.VerificationType)
            };

            LPVOID lpCommandData = ShareMemory.WriteStructure<WFSPINSECUREKEYENTRY>(ref commandData);

            try
            {
                AsyncExecute(CMD.WFS_CMD_PIN_SECUREKEY_ENTRY, lpCommandData, 100000);
            }
            finally
            {
                WFSPINGETDATA.Free(lpCommandData);
            }
        }
    }
}
