using XFS4IoT.Completions;
using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;

namespace XFS3xCardReader
{
    public partial class CardReaderDevice : ICardReaderDevice
    {
        private IDCDevice Device { get; init; }
        public CardReaderDevice(string xfs3xLogicalName)
        {

            CommonStatus = new CommonStatusClass(CommonStatusClass.DeviceEnum.Offline,
                                                 CommonStatusClass.PositionStatusEnum.InPosition,
                                                 0,
                                                 CommonStatusClass.AntiFraudModuleEnum.NotSupported,
                                                 CommonStatusClass.ExchangeEnum.NotSupported,
                                                 CommonStatusClass.EndToEndSecurityEnum.NotSupported);


            CardReaderStatus = new CardReaderStatusClass(CardReaderStatusClass.MediaEnum.NotPresent,
                                                         CardReaderStatusClass.SecurityEnum.NotSupported,
                                                         CardReaderStatusClass.ChipPowerEnum.NoCard,
                                                         CardReaderStatusClass.ChipModuleEnum.Ok,
                                                         CardReaderStatusClass.MagWriteModuleEnum.Ok,
                                                         CardReaderStatusClass.FrontImageModuleEnum.NotSupported,
                                                         CardReaderStatusClass.BackImageModuleEnum.NotSupported);
            AssemblyVersion = new(typeof(CardReaderDevice).Assembly.GetName().Name, typeof(CardReaderDevice).Assembly.GetName().Version?.ToString());

            Device = new IDCDevice(xfs3xLogicalName);
            if (Device.Init())
            {
                s_logger.Info("Init IDC Device success");

                var caps = Device.GetCapabilities();
                if (caps != null)
                {
                    CardReaderCapabilities = caps;
                }
                else
                {
                    throw new Exception("Get Capabilities Exception");
                }

            }
            else
            {
                s_logger.Error("Init SDK ERROR");
            }

        }
        public CardReaderStatusClass CardReaderStatus { get; set; }
        public CardReaderCapabilitiesClass CardReaderCapabilities { get; set; } = new(CardReaderCapabilitiesClass.DeviceTypeEnum.Dip,
                                                                                      CardReaderCapabilitiesClass.ReadableDataTypesEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.WritableDataTypesEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.ChipProtocolsEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.SecurityTypeEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.PowerOptionEnum.NoAction,
                                                                                      CardReaderCapabilitiesClass.PowerOptionEnum.NoAction,
                                                                                      FluxSensorProgrammable: false,
                                                                                      ReadWriteAccessFollowingExit: false,
                                                                                      CardReaderCapabilitiesClass.WriteMethodsEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.ChipPowerOptionsEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.NotSupported,
                                                                                      CardReaderCapabilitiesClass.PositionsEnum.NotSupported,
                                                                                      false);

        public async Task<AcceptCardResult> AcceptCardAsync(CommonCardCommandEvents events, AcceptCardRequest acceptCardInfo, CancellationToken cancellation)
        {
            s_logger.Debug($"Call [{nameof(AcceptCardAsync)}]");
            await Task.Delay(1000, cancellation);
            Device.AcceptCard(acceptCardInfo);
            await events.InsertCardEvent();
            int idxEvent = await WaitAny(new[] { MediaInsertEvent, ExecuteCompleteEvent });
            if (idxEvent == 0)
            {
                await events.MediaInsertedEvent();
                await WaitOne(ExecuteCompleteEvent);
            }

            return Device.LastAcceptCardResult;
        }

        public Task<ChipIOResult> ChipIOAsync(ChipIORequest dataToSend, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<ChipPowerResult> ChipPowerAsync(ChipPowerRequest action, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<EMVContactlessConfigureResult> EMVContactlessConfigureAsync(EMVContactlessConfigureRequest terminalConfig, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<EMVContactlessIssuerUpdateResult> EMVContactlessIssuerUpdateAsync(EMVClessCommandEvents events, EMVContactlessIssuerUpdateRequest transactionData, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<EMVContactlessPerformTransactionResult> EMVContactlessPerformTransactionAsync(EMVClessCommandEvents events, EMVContactlessPerformTransactionRequest transactionData, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public QueryEMVApplicationResult EMVContactlessQueryApplications()
        {
            throw new NotImplementedException();
        }

        public async Task<MoveCardResult> MoveCardAsync(MoveCardRequest moveCardInfo, CancellationToken cancellation)
        {
            s_logger.Debug($"Call [{nameof(MoveCardAsync)}]");
            Device.MoveCard(moveCardInfo);
            s_logger.Debug($"Wait for [{nameof(ExecuteCompleteEvent)}]");
            await WaitOne(ExecuteCompleteEvent);
            s_logger.Debug($"Have [{nameof(ExecuteCompleteEvent)}]");
            MoveCardResult moveCardResult = Device.LastMoveCardResult;
            if (moveCardInfo.To.Position == MovePosition.MovePositionEnum.Storage && moveCardResult.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                return new MoveCardResult(moveCardResult.CompletionCode, moveCardInfo.To.StorageId, 1);
            }
            else
            {
                return moveCardResult;
            }
        }

        public QueryIFMIdentifierResult QueryIFMIdentifier()
        {
            throw new NotImplementedException();
        }

        public async Task<ReadCardResult> ReadCardAsync(ReadCardCommandEvents events, ReadCardRequest dataToRead, CancellationToken cancellation)
        {
            s_logger.Debug($"Call [{nameof(ReadCardAsync)}]");
            await Task.Delay(1000, cancellation);

            MessagePayload.CompletionCodeEnum completionCode = MessagePayload.CompletionCodeEnum.Success;

            List<ReadCardResult.CardData> chipATR = new();
            return new ReadCardResult(completionCode, Device.ReadData, chipATR);
        }

        public async Task<ResetDeviceResult> ResetDeviceAsync(ResetCommandEvents events, ResetDeviceRequest cardAction, CancellationToken cancellation)
        {
            s_logger.Debug($"Call [{nameof(ResetDeviceAsync)}]");

            // Clear all event signal
            MediaDetectedEvent.Reset();
            ExecuteCompleteEvent.Reset();

            // Call Device API
            Device.Reset(cardAction.MoveTo);

            // Wait for command completed or Media detect EVENT
            int idxEvent = await WaitAny(new[] { MediaDetectedEvent, ExecuteCompleteEvent });
            if (idxEvent == 0)
            {
                // Media detected
                await events.MediaDetectedEvent(new MovePosition(Device.ResetOut, s_binPositionName));
                // Wait for reset command complated
                await WaitOne(ExecuteCompleteEvent);
            }
            return Device.LastResetDeviceResult;
        }


        public Task<SetCIM86KeyResult> SetCIM86KeyAsync(SetCIM86KeyRequest keyInfo, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<WriteCardResult> WriteCardAsync(CommonCardCommandEvents events, WriteCardRequest dataToWrite, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}