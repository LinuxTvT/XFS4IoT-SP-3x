using XFS4IoTFramework.Common;
using XFS4IoTServer;

namespace XFS3xCardReader
{
    public partial class CardReaderDevice : ICommonDevice
    {

        #region Common Device

        Task<DeviceResult> ICommonDevice.ClearCommandNonce()
        {
            throw new NotImplementedException();
        }

        Task<GetCommandNonceResult> ICommonDevice.GetCommandNonce()
        {
            throw new NotImplementedException();
        }

        Task<GetTransactionStateResult> ICommonDevice.GetTransactionState()
        {
            throw new NotImplementedException();
        }

        Task<DeviceResult> ICommonDevice.PowerSaveControl(int MaxPowerSaveRecoveryTime, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }



        Task<DeviceResult> ICommonDevice.SetTransactionState(SetTransactionStateRequest request)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Stores Common Capabilities
        /// </summary>
        public CommonCapabilitiesClass CommonCapabilities { get; set; } = new CommonCapabilitiesClass(
                CommonInterface: new CommonCapabilitiesClass.CommonInterfaceClass
                (
                    Commands: new()
                    {
                        CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.Capabilities,
                        CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.Status
                    }
                ),
                CardReaderInterface: new CommonCapabilitiesClass.CardReaderInterfaceClass
                (
                    Commands: new()
                    {
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.ReadRawData,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.Reset,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.WriteRawData,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.ChipIO,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.ChipPower,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessConfigure,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessIssuerUpdate,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessPerformTransaction,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessQueryApplications,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.QueryIFMIdentifier,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.SetKey,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.Move,
                    },
                    Events: new()
                    {
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.InsertCardEvent,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.MediaDetectedEvent,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.MediaInsertedEvent,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.MediaRemovedEvent,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.InvalidMediaEvent,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.EMVClessReadStatusEvent,
                        CommonCapabilitiesClass.CardReaderInterfaceClass.EventEnum.CardActionEvent,
                    }
                ),
                StorageInterface: new CommonCapabilitiesClass.StorageInterfaceClass
                (
                    Commands: new()
                    {
                        CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum.GetStorage,
                        CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum.SetStorage,
                    },
                    Events: new()
                    {
                        CommonCapabilitiesClass.StorageInterfaceClass.EventEnum.StorageThresholdEvent,
                        CommonCapabilitiesClass.StorageInterfaceClass.EventEnum.StorageChangedEvent,
                        CommonCapabilitiesClass.StorageInterfaceClass.EventEnum.StorageErrorEvent,
                    }
                ),
                DeviceInformation: new List<CommonCapabilitiesClass.DeviceInformationClass>()
                {
                    new CommonCapabilitiesClass.DeviceInformationClass(
                            ModelName: "Simulator",
                            SerialNumber: "123456-78900001",
                            RevisionNumber: "1.0",
                            ModelDescription: "KAL simualtor",
                            Firmware: new List<CommonCapabilitiesClass.FirmwareClass>()
                            {
                                new CommonCapabilitiesClass.FirmwareClass(
                                        FirmwareName: "XFS4 SP",
                                        FirmwareVersion: "1.0",
                                        HardwareRevision: "1.0")
                            },
                            Software: new List<CommonCapabilitiesClass.SoftwareClass>()
                            {
                                new CommonCapabilitiesClass.SoftwareClass(
                                        SoftwareName: "XFS4 SP",
                                        SoftwareVersion: "1.0")
                            })
                },
                PowerSaveControl: false,
                AntiFraudModule: false);
        /// <summary>
        /// Stores Commons status
        /// </summary>
        public CommonStatusClass CommonStatus { get; set; }

        /// <summary>
        /// Version of C# Assembly
        /// </summary>
        private readonly CommonCapabilitiesClass.SoftwareClass AssemblyVersion;

    }
}