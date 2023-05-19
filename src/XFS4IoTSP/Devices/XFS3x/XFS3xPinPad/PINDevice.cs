using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using XFS3xAPI;
using XFS3xAPI.PIN;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.PinPad;
using XFS4IoTServer;
using static XFS4IoTFramework.KeyManagement.KeyDetail;
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

        public DeviceResult LastResetDeviceResult => new(RESULT.ToXfs4IotCompletionCode(_hCompleteResult));

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
            throw new NotImplementedException();
        }

        protected override void HandleExecuteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_EXEE_PIN_KEY:
                    WFSPINKEY pinKey = Marshal.PtrToStructure<WFSPINKEY>(xfsResult.lpBuffer);
                    _bcKeyInput?.Add(pinKey);
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
                    int ptrSize = Marshal.SizeOf(typeof(IntPtr));
                    int idx = 0;
                    if (xfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPINDATA pinData = Marshal.PtrToStructure<WFSPINDATA>(xfsResult.lpBuffer);
                        DataEntryResult = new(RESULT.ToXfs4IotCompletionCode(_hCompleteResult),
                                                pinData.usKeys,
                                                new(),
                                                WCompletion.ToEnum(pinData.wCompletion));
                        for (; ; )
                        {
                            IntPtr intPtr = Marshal.ReadIntPtr(pinData.lpPinKeys, idx * ptrSize);
                            if (intPtr == IntPtr.Zero)
                            {
                                break;
                            }
                            else
                            {
                                WFSPINKEY pinKey = Marshal.PtrToStructure<WFSPINKEY>(intPtr);
                                Console.WriteLine($"KEY: [{pinKey.ulDigit}]");
                                DataEntryResult.EnteredKeys.Add(new DataEntryResult.EnteredKey(
                                                                        UlKeyMask.ToString(pinKey.ulDigit),
                                                                        WCompletion.ToEnum(pinKey.wCompletion)));
                            }
                            idx++;
                        }
                    }
                    _bcKeyInput?.CompleteAdding();
                    break;

                case CMD.WFS_CMD_PIN_GET_PIN:

                    if (xfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPINENTRY pinData = Marshal.PtrToStructure<WFSPINENTRY>(xfsResult.lpBuffer);
                        PinEntryResult = new(RESULT.ToXfs4IotCompletionCode(_hCompleteResult),
                                                pinData.usDigits,
                                                WCompletion.ToEnum(pinData.wCompletion));
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
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSPINSTATUS_310 wfsPINStatus = Marshal.PtrToStructure<WFSPINSTATUS_310>(wfsResult.lpBuffer);

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
                XfsService.FreeWFSResult(lpResult);
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

            LPVOID lpCommandData = Marshal.AllocHGlobal(Marshal.SizeOf(wfsPinGetData));

            Marshal.StructureToPtr(wfsPinGetData, lpCommandData, false);

            try
            {
                AsyncExecute(CMD.WFS_CMD_PIN_GET_DATA, lpCommandData, 100000);
            }
            finally
            {
                Marshal.FreeHGlobal(lpCommandData);
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

            LPVOID lpCommandData = Marshal.AllocHGlobal(Marshal.SizeOf(wfsPinGetPIN));

            Marshal.StructureToPtr(wfsPinGetPIN, lpCommandData, false);

            try
            {
                AsyncExecute(CMD.WFS_CMD_PIN_GET_PIN, lpCommandData, 100000);
            }
            finally
            {
                Marshal.FreeHGlobal(lpCommandData);
            }
        }

        public List<byte> GetPINBlock(PINBlockRequest request)
        {
            WFSPINBLOCK wfsBINBLock = new()
            {
                lpsCustomerData = Marshal.StringToHGlobalAnsi(request.CustomerData),
                lpsXORData = Marshal.StringToHGlobalAnsi(request.XorData),
                bPadding = request.Padding,
                wFormat = FwPinFormats.FromEnum(request.Format),
                lpsKey = Marshal.StringToHGlobalAnsi(request.KeyName),
                lpsKeyEncKey = Marshal.StringToHGlobalAnsi(request.SecondEncKeyName)
            };

            LPWFSRESULT lpResult = LPVOID.Zero;

            LPVOID lpCommandData = Marshal.AllocHGlobal(Marshal.SizeOf(wfsBINBLock));
            Marshal.StructureToPtr(wfsBINBLock, lpCommandData, false);

            try
            {
                Execute(CMD.WFS_CMD_PIN_GET_PINBLOCK, lpCommandData, ref lpResult, 100000);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = WFSRESULT.FromPtr(lpResult);
                    WFSXDATA wfsXData = wfsResult.Data<WFSXDATA>();
                    return wfsXData.Data();
                }
                else
                {
                    throw new NullResultException();
                }
            }
            finally
            {
                wfsBINBLock.Free();
                Marshal.FreeHGlobal(lpCommandData);
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
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    int ptrSize = Marshal.SizeOf(typeof(IntPtr));
                    int idx = 0;
                    var keyDetailList = new Dictionary<string, KeyDetail>();
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {

                        for (; ; )
                        {
                            IntPtr intPtr = Marshal.ReadIntPtr(wfsResult.lpBuffer, idx * ptrSize);
                            if (intPtr == IntPtr.Zero)
                            {
                                break;
                            }
                            else
                            {
                                WFSPINKEYDETAIL wFSIDCCARDDATA = Marshal.PtrToStructure<WFSPINKEYDETAIL>(intPtr);
                                string keyName = (wFSIDCCARDDATA.lpsKeyName == "PINKey") ? "PinKey" : wFSIDCCARDDATA.lpsKeyName;
                                var key = new KeyDetail(
                                             // string keyName
                                             keyName,
                                             //int KeySlot,
                                             0,
                                             //string KeyUsage,
                                             FwUse.ToKeyUsage(wFSIDCCARDDATA.fwUse),
                                             //string Algorithm,
                                             "T",
                                             //string ModeOfUse,
                                             "B",
                                             //int KeyLength,
                                             32,
                                             //KeyStatusEnum KeyStatus,
                                             KeyStatusEnum.Loaded,
                                             //bool Preloaded,
                                             false,
                                             //string RestrictedKeyUsage,
                                             "C0",
                                             //string KeyVersionNumber,
                                             "V1",
                                             //string Exportability,
                                             "N",
                                             //List<byte> OptionalKeyBlockHeader,
                                             new List<byte>());
                                //ReadData.Add(WDataSource.ToEnum(wFSIDCCARDDATA.wDataSource), new ReadCardResult.CardData(wFSIDCCARDDATA.DataStatus, wFSIDCCARDDATA.DataAsList()));
                                Console.WriteLine($"Key: [{wFSIDCCARDDATA.lpsKeyName}]");
                                keyDetailList.Add(keyName, key);
                            }
                            idx++;
                        }
                    }
                    return keyDetailList;

                }
                else
                {
                    throw new NullResultException();
                }
            }
            catch { throw; }
            finally
            {
                XfsService.FreeWFSResult(lpResult);
            }
        }

        public readonly Dictionary<EntryModeEnum, List<FrameClass>> KeyLayouts = new()
        {
            {
                EntryModeEnum.Data,
                new List<FrameClass>()
                {
                     {
                        new FrameClass(0, 0, 0, 0, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(282, 204, 80, 80, "one", null) },
                            { new FrameClass.FunctionKeyClass(282, 294, 80, 80, "four", null) },
                            { new FrameClass.FunctionKeyClass(282, 384, 80, 80, "seven", null) },
                            { new FrameClass.FunctionKeyClass(372, 204, 80, 80, "two", null) },
                            { new FrameClass.FunctionKeyClass(372, 294, 80, 80, "five", null) },
                            { new FrameClass.FunctionKeyClass(372, 384, 80, 80, "eight", null) },
                            { new FrameClass.FunctionKeyClass(372, 474, 80, 80, "zero", null) },
                            { new FrameClass.FunctionKeyClass(462, 204, 80, 80, "three", null) },
                            { new FrameClass.FunctionKeyClass(462, 294, 80, 80, "six", null) },
                            { new FrameClass.FunctionKeyClass(462, 384, 80, 80, "nine", null) },
                            { new FrameClass.FunctionKeyClass(572, 209, 160, 80, "enter", null) },
                            { new FrameClass.FunctionKeyClass(572, 299, 160, 80, "clear", null) },
                            { new FrameClass.FunctionKeyClass(572, 389, 160, 80, "cancel", null) },
                            { new FrameClass.FunctionKeyClass(572, 479, 160, 80,  "backspace", null) }
                        })
                    },
                    {
                        new FrameClass(0, 0, 0, 768, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(0, 0, 0, 187, "fdk01", null) },
                            { new FrameClass.FunctionKeyClass(0, 187, 0, 187, "fdk02", null) },
                            { new FrameClass.FunctionKeyClass(0, 374, 0, 187, "fdk03", null) },
                            { new FrameClass.FunctionKeyClass(0, 561, 0, 187, "fdk04", null) }
                        })
                    },
                    {
                        new FrameClass(1024, 0, 0, 768, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(0, 0, 0, 187, "fdk05", null) },
                            { new FrameClass.FunctionKeyClass(0, 187, 0, 187, "fdk06", null) },
                            { new FrameClass.FunctionKeyClass(0, 374, 0, 187, "fdk07", null) },
                            { new FrameClass.FunctionKeyClass(0, 561, 0, 187, "fdk08", null) },
                        })
                    }
                }
            },
            {
                EntryModeEnum.Pin,
                new List<FrameClass>()
                {
                    {
                        new FrameClass(0, 0, 0, 0, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(287, 209, 80, 80, "one", null) },
                            { new FrameClass.FunctionKeyClass(287, 299, 80, 80, "four", null) },
                            { new FrameClass.FunctionKeyClass(287, 389, 80, 80, "seven", null) },
                            { new FrameClass.FunctionKeyClass(377, 209, 80, 80, "two", null) },
                            { new FrameClass.FunctionKeyClass(377, 299, 80, 80, "five", null) },
                            { new FrameClass.FunctionKeyClass(377, 389, 80, 80, "eight", null) },
                            { new FrameClass.FunctionKeyClass(377, 479, 80, 80, "zero", null) },
                            { new FrameClass.FunctionKeyClass(467, 209, 80, 80, "three", null) },
                            { new FrameClass.FunctionKeyClass(467, 299, 80, 80, "six", null) },
                            { new FrameClass.FunctionKeyClass(467, 389, 80, 80, "nine", null) },
                            { new FrameClass.FunctionKeyClass(577, 209, 160, 80, "enter", null) },
                            { new FrameClass.FunctionKeyClass(577, 299, 160, 80, "clear", null) },
                            { new FrameClass.FunctionKeyClass(577, 389, 160, 80, "cancel", null) },
                            { new FrameClass.FunctionKeyClass(577, 479, 160, 80, "backspace", null) }
                        })
                    }
                }
            },
            {
                EntryModeEnum.Secure,
                new List<FrameClass>()
                {
                    {
                        new FrameClass(0, 0, 0, 0, FrameClass.FloatEnum.NotSupported,
                        new List<FrameClass.FunctionKeyClass>()
                        {
                            { new FrameClass.FunctionKeyClass(287, 209, 80, 80, "one", "a") },
                            { new FrameClass.FunctionKeyClass(287, 299, 80, 80, "four", "d") },
                            { new FrameClass.FunctionKeyClass(287, 389, 80, 80, "seven", null) },
                            { new FrameClass.FunctionKeyClass(287, 479, 80, 80, "shift", null) },
                            { new FrameClass.FunctionKeyClass(377, 209, 80, 80, "two", "b") },
                            { new FrameClass.FunctionKeyClass(377, 299, 80, 80, "five", "e") },
                            { new FrameClass.FunctionKeyClass(377, 389, 80, 80, "eight", null) },
                            { new FrameClass.FunctionKeyClass(377, 479, 80, 80, "zero", null) },
                            { new FrameClass.FunctionKeyClass(467, 209, 80, 80, "three", "c") },
                            { new FrameClass.FunctionKeyClass(467, 299, 80, 80, "six", "f") },
                            { new FrameClass.FunctionKeyClass(467, 389, 80, 80, "nine", null) },
                            { new FrameClass.FunctionKeyClass(577, 209, 160, 80, "enter", null) },
                            { new FrameClass.FunctionKeyClass(577, 299, 160, 80, "clear", null) },
                            { new FrameClass.FunctionKeyClass(577, 389, 160, 80, "cancel", null) }
                        })
                    }
                }
            }
        };
    }
}
