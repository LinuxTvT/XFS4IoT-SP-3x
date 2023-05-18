using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;

namespace XFS3xCardReader
{
    public partial class CardReaderDevice : IStorageDevice
    {
        private const string s_binPositionName = "unitBIN1";

        private const string s_binSerialNumber = "SN0919531558";

        private static readonly CardCapabilitiesClass s_binCardCapabilities = new(CardCapabilitiesClass.TypeEnum.Retain, false);

        public bool GetCardStorageConfiguration(out Dictionary<string, CardUnitStorageConfiguration> newCardUnits)
        {
            var binThreshold = GetBinThreshold();
            newCardUnits = new() {
                                    {
                                        s_binPositionName,
                                        new(s_binPositionName,
                                            binThreshold,
                                            s_binSerialNumber,
                                            s_binCardCapabilities,
                                            new CardConfigurationClass(binThreshold))
                                    }
                                };
            return true;
        }

        /// <summary>
        /// This method is call after card is moved to the storage. Move or Reset command.
        /// </summary>
        /// <returns>Return true if the device maintains hardware counters for the card unit_cardBinCouts</returns>
        public bool GetCardUnitCounts(out Dictionary<string, CardUnitCount> unitCounts)
        {
            unitCounts = new() { { s_binPositionName, new(0, GetBinCount()) } };
            return true;
        }

        /// <summary>
        /// Update card unit hardware status by device class. the maintaining status by the framework will be overwritten.
        /// The framework can't handle threshold event if the device class maintains hardware storage status on threshold value is not zero.
        /// </summary>
        /// <returns>Return true if the device maintains hardware card unit status</returns>
        public bool GetCardUnitStatus(out Dictionary<string, CardStatusClass.ReplenishmentStatusEnum> unitStatus)
        {
            unitStatus = new() { { s_binPositionName, CardStatusClass.ReplenishmentStatusEnum.Healthy } };
            return true;
        }

        /// <summary>
        /// Update card unit hardware storage status by device class.
        /// </summary>
        /// <returns>Return true if the device maintains hardware card storage status</returns>
        public bool GetCardStorageStatus(out Dictionary<string, CardUnitStorage.StatusEnum> storageStatus)
        {
            storageStatus = new() { { s_binPositionName, CardUnitStorage.StatusEnum.Good } };
            return true;
        }

        public Task<SetCardStorageResult> SetCardStorageAsync(SetCardStorageRequest request, CancellationToken cancellation)
        {
            var functionInfo = nameof(SetCardStorageAsync);
            Logger.Debug($"Call: {functionInfo}");
            if (request.CardStorageToSet.ContainsKey(s_binPositionName))
            {
                var initCount = request.CardStorageToSet[s_binPositionName].InitialCount;
                if (initCount != 0)
                {
                    var errorString = $"InitialCount # 0, XFS 3.x only support reset count";
                    Logger.Error($"{functionInfo}=> {errorString}");
                    return Task.FromResult(new SetCardStorageResult(MessagePayload.CompletionCodeEnum.UnsupportedData, errorString));
                }
                else
                {
                    ResetBinCount();
                    return Task.FromResult(new SetCardStorageResult(MessagePayload.CompletionCodeEnum.Success));
                }
            }
            else
            {
                var errorString = $"ID # [{s_binPositionName}]";
                Logger.Error($"{functionInfo}=> {errorString}");
                return Task.FromResult(new SetCardStorageResult(MessagePayload.CompletionCodeEnum.UnsupportedData, errorString));
            }

        }

        public bool GetCashStorageConfiguration(out Dictionary<string, CashUnitStorageConfiguration> newCardUnits)
        {
            throw new NotImplementedException();
        }

        public bool GetCashUnitCounts(out Dictionary<string, CashUnitCountClass> unitCounts)
        {
            throw new NotImplementedException();
        }

        public bool GetCashUnitInitialCounts(out Dictionary<string, StorageCashCountClass> initialCounts)
        {
            throw new NotImplementedException();
        }

        public bool GetCashStorageStatus(out Dictionary<string, CashUnitStorage.StatusEnum> storageStatus)
        {
            throw new NotImplementedException();
        }

        public bool GetCashUnitStatus(out Dictionary<string, CashStatusClass.ReplenishmentStatusEnum> unitStatus)
        {
            throw new NotImplementedException();
        }

        public void GetCashUnitAccuray(string storageId, out CashStatusClass.AccuracyEnum unitAccuracy)
        {
            throw new NotImplementedException();
        }

        public Task<SetCashStorageResult> SetCashStorageAsync(SetCashStorageRequest request, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<StartExchangeResult> StartExchangeAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<EndExchangeResult> EndExchangeAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

    }
}