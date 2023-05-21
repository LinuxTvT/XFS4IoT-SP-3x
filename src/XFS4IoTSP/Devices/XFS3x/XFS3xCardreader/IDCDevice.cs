using System.Runtime.InteropServices;
using XFS3xAPI;
using XFS3xAPI.IDC;
using XFS4IoT.Completions;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using DWORD = System.UInt32;
using EVENT = XFS3xAPI.IDC.EVENT;
using HRESULT = System.Int32;
using LPVOID = System.IntPtr;
using LPWFSRESULT = System.IntPtr;
using WORD = System.UInt16;

namespace XFS3xCardReader
{
    public class IDCDevice : XfsService
    {
        private WORD _wResetOut;

        private HRESULT _hCompleteResult;

        public readonly AutoResetEvent MediaInsertEvent = new(false);
        public readonly AutoResetEvent MediaDetectedEvent = new(false);
        public readonly AutoResetEvent MediaRemovedEvent = new(false);

        public MovePosition.MovePositionEnum ResetOut => WAction.ToEnum(_wResetOut);

        public AcceptCardResult LastAcceptCardResult => new(RESULT.ToCompletionCode(_hCompleteResult), RESULT.ToString(_hCompleteResult));

        public ResetDeviceResult LastResetDeviceResult => new(RESULT.ToCompletionCode(_hCompleteResult), RESULT.ToString(_hCompleteResult));

        public MoveCardResult LastMoveCardResult
        {
            get
            {
                if (RESULT.IsGenericError(_hCompleteResult))
                {
                    return new MoveCardResult(RESULT.ToCompletionCode(_hCompleteResult), RESULT.ToString(_hCompleteResult));
                }
                else
                {
                    return new MoveCardResult(MessagePayload.CompletionCodeEnum.CommandErrorCode, ERROR.ToString(_hCompleteResult), ERROR.ToMoveCompletionErrorCode(_hCompleteResult));
                }
            }
        }

        public Dictionary<ReadCardRequest.CardDataTypesEnum, ReadCardResult.CardData> ReadData { get; } = new();

        public IDCDevice(string logicalName) : base(logicalName)
        {
            Logger.Info($"Create [{nameof(IDCDevice)}] object, logical name [{logicalName}]");
        }

        #region Handle XFS WM Event functions
        protected override void HandleCompleteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"HandleCompleteEvent: [{CMD.ToExecuteCommandString(xfsResult.dwCommandCode)}]");
            switch (xfsResult.dwCommandCode)
            {
                case CMD.WFS_CMD_IDC_READ_RAW_DATA:
                    ReadData.Clear();
                    int ptrSize = Marshal.SizeOf(typeof(IntPtr));
                    int idx = 0;
                    if (xfsResult.lpBuffer != LPVOID.Zero)
                    {

                        for (; ; )
                        {
                            IntPtr intPtr = Marshal.ReadIntPtr(xfsResult.lpBuffer, idx * ptrSize);
                            if (intPtr == IntPtr.Zero)
                            {
                                break;
                            }
                            else
                            {
                                WFSIDCCARDDATA wFSIDCCARDDATA = Marshal.PtrToStructure<WFSIDCCARDDATA>(intPtr);
                                ReadData.Add(WDataSource.ToEnum(wFSIDCCARDDATA.wDataSource), new ReadCardResult.CardData(wFSIDCCARDDATA.DataStatus, wFSIDCCARDDATA.DataAsList()));

                            }
                            idx++;
                        }
                    }
                    break;
                case CMD.WFS_CMD_IDC_RESET:
                    break;

                case CMD.WFS_CMD_IDC_EJECT_CARD:
                    break;

                case CMD.WFS_CMD_IDC_RETAIN_CARD:
                    WFSIDCRETAINCARD wFSIDCRETAINCARD = Marshal.PtrToStructure<WFSIDCRETAINCARD>(xfsResult.lpBuffer);
                    Console.WriteLine($"Count [{wFSIDCRETAINCARD.usCount}]");

                    break;

                default:
                    Logger.Error($"Unhanlde Complete CMD [{CMD.ToExecuteCommandString(xfsResult.dwCommandCode)}]");
                    Console.WriteLine($"Unhanlde Complete CMD [{CMD.ToExecuteCommandString(xfsResult.dwCommandCode)}]");

                    break;
            }

            _hCompleteResult = xfsResult.hResult;
            ExecuteCompleteEvent.Set();
        }

        protected override void HandleExecuteEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleExecuteEvent)}: [{EVENT.ToString(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_EXEE_IDC_MEDIAINSERTED:
                    MediaInsertEvent.Set();
                    break;
                default:
                    Logger.Error($"Unhandle Execute Event: [{EVENT.ToString(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Execute Event: [{EVENT.ToString(xfsResult.dwEventID)}]");
                    break;
            }

        }

        protected override void HandleServiceEvent(ref WFSRESULT xfsResult)
        {
            Logger.Debug($"{nameof(HandleServiceEvent)}: [{EVENT.ToString(xfsResult.dwEventID)}]");
            switch (xfsResult.dwEventID)
            {
                case EVENT.WFS_SRVE_IDC_MEDIADETECTED:
                    _wResetOut = (WORD)Marshal.ReadInt16(xfsResult.lpBuffer);
                    MediaDetectedEvent.Set();
                    break;
                case EVENT.WFS_SRVE_IDC_MEDIAREMOVED:
                    MediaRemovedEvent.Set();
                    break;
                default:
                    Logger.Error($"Unhandle Service Event: [{EVENT.ToString(xfsResult.dwEventID)}]");
                    Console.WriteLine($"Unhandle Service Event: [{EVENT.ToString(xfsResult.dwEventID)}]");
                    break;
            }

        }
        #endregion

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        public void Reset(ResetDeviceRequest.ToEnum to)
        {
            Logger.Info($"Reset Device, to [{to}]");
            IntPtr lpwResetIn = Marshal.AllocHGlobal(sizeof(WORD));
            Marshal.WriteInt16(lpwResetIn, (Int16)(FwPowerOffOption.FromEnum(to)));
            try
            {
                AsyncExecute(CMD.WFS_CMD_IDC_RESET, lpwResetIn);
            }
            finally
            {
                Marshal.FreeHGlobal(lpwResetIn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AcceptCard(AcceptCardRequest acceptCardRequest)
        {
            LPVOID hglobal = Marshal.AllocHGlobal(sizeof(WORD));
            try
            {
                Marshal.WriteInt16(hglobal, (Int16)(FwReadTracks.FromEnum(acceptCardRequest.DataToRead)));
                AsyncExecute(CMD.WFS_CMD_IDC_READ_RAW_DATA, hglobal, (DWORD)acceptCardRequest.Timeout);
            }
            finally
            {
                Marshal.FreeHGlobal(hglobal);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void MoveCard(MoveCardRequest moveCardInfo)
        {
            AsyncExecute(CMD.FromMoveCardRequest(moveCardInfo), LPVOID.Zero);
        }

        public int GetBinCount()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_IDC_STATUS, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSIDCSTATUS_300 wfsIDCStatus = Marshal.PtrToStructure<WFSIDCSTATUS_300>(wfsResult.lpBuffer);
                        return wfsIDCStatus.usCards;
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
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }

        }

        public int GetBinThreshold()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_IDC_CAPABILITIES, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSIDCCAPS_300 wfsIDCCaps = Marshal.PtrToStructure<WFSIDCCAPS_300>(wfsResult.lpBuffer);
                        return wfsIDCCaps.usCards;
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
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

        public void ResetBinCount()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                Execute(CMD.WFS_CMD_IDC_RESET_COUNT, LPVOID.Zero, ref lpResult);
            }
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

        public void UpdateStatus(CommonStatusClass status, CardReaderStatusClass cardStatus)
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_IDC_STATUS, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSIDCSTATUS_300 wfsIDCStatus = Marshal.PtrToStructure<WFSIDCSTATUS_300>(wfsResult.lpBuffer);

                        status.Device = FwDevice.ToEnum(wfsIDCStatus.fwDevice);
                        cardStatus.Media = FwMedia.ToEnum(wfsIDCStatus.fwMedia);
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

        public CardReaderCapabilitiesClass? GetCapabilities()
        {
            LPWFSRESULT lpResult = LPWFSRESULT.Zero;
            try
            {
                GetInfo(CMD.WFS_INF_IDC_CAPABILITIES, LPVOID.Zero, ref lpResult);
                if (lpResult != LPWFSRESULT.Zero)
                {
                    WFSRESULT wfsResult = Marshal.PtrToStructure<WFSRESULT>(lpResult);
                    if (wfsResult.lpBuffer != LPVOID.Zero)
                    {
                        WFSIDCCAPS_300 wfsIDCCaps = Marshal.PtrToStructure<WFSIDCCAPS_300>(wfsResult.lpBuffer);
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
            catch { throw; }
            finally
            {
                ShareMemory.FreeResult(lpResult);
            }
        }

    }
}
