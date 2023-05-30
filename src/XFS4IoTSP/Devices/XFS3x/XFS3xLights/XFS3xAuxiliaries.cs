using Lights.XFS3xLights;
using XFS3xAPI.SIU;
using XFS4IoT;
using XFS4IoTFramework.Auxiliaries;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Lights;
using XFS4IoTServer;

namespace Auxiliaries.XFS3xAuxiliaries
{
    public class XFS3xAuxiliaries : SIUDevice, IAuxiliariesDevice, ICommonDevice, ILightsDevice
    {
        public XFS3xAuxiliaries(string logicalServiceName) : base(logicalServiceName) {
            PortEvent += PortEventHandle;
        }

        #region Common Interface

        public CommonStatusClass CommonStatus { get => SIUCommonStatus; set { SIUCommonStatus = value; } }

        public CommonCapabilitiesClass CommonCapabilities { get; set; } = new CommonCapabilitiesClass(
                CommonInterface: new CommonCapabilitiesClass.CommonInterfaceClass
                (
                    Commands: new()
                    {
                        CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.Capabilities,
                        CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.Status
                    }
                ),
                AuxiliariesInterface: new CommonCapabilitiesClass.AuxiliariesInterfaceClass
                (
                    Commands: new()
                    {
                        CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.SetAutoStartUpTime,
                        CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.ClearAutoStartUpTime,
                        CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.GetAutoStartUpTime,
                        CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.SetAuxiliaries,
                        CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.Register,
                    },
                    Events: new()
                    {
                        CommonCapabilitiesClass.AuxiliariesInterfaceClass.EventEnum.AuxiliaryStatusEvent,
                    }
                ),
                LightsInterface: new CommonCapabilitiesClass.LightsInterfaceClass
                (
                    Commands: new()
                    {
                        CommonCapabilitiesClass.LightsInterfaceClass.CommandEnum.SetLight,
                    }
                ),
                DeviceInformation: new List<CommonCapabilitiesClass.DeviceInformationClass>()
                {
                    new CommonCapabilitiesClass.DeviceInformationClass(
                            ModelName: "ModelName",
                            SerialNumber: "SerialNumber",
                            RevisionNumber: "RevisionNumber",
                            ModelDescription: "ModelDescription",
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

        public Task<DeviceResult> PowerSaveControl(int MaxPowerSaveRecoveryTime, CancellationToken cancel) => throw new NotImplementedException();
        public Task<DeviceResult> SetTransactionState(SetTransactionStateRequest request) => throw new NotImplementedException();
        public Task<GetTransactionStateResult> GetTransactionState() => throw new NotImplementedException();
        public Task<GetCommandNonceResult> GetCommandNonce() => throw new NotImplementedException();
        public Task<DeviceResult> ClearCommandNonce() => throw new NotImplementedException();

        #endregion

        #region Lights Interface

        /// <summary>
        /// This command is used to set the status of a light.
        /// For guidelights, the slow and medium flash rates must not be greater than 2.0 Hz. 
        /// It should be noted that in order to comply with American Disabilities Act guidelines only a slow or medium flash rate must be used.
        /// </summary>
        public async Task<SetLightResult> SetLightAsync(SetLightRequest request, CancellationToken cancellation)
        {
            foreach (var item in request.StdLights)
            {
                SetLight(item.Key, item.Value);
                await WaitOne(ExecuteCompleteEvent);
            }
            return new SetLightResult(LastCompletionCode);
        }

        /// <summary>
        /// Lights Capabilities
        /// </summary>
        public LightsCapabilitiesClass? LightsCapabilities { get => SIULightsCapabilities; set { SIULightsCapabilities = value; } }

        /// <summary>
        /// Stores light status
        /// </summary>
        public LightsStatusClass LightsStatus { get => SIULightsStatus; set { SIULightsStatus = value; } }

        #endregion

        public XFS4IoTServer.IServiceProvider? SetServiceProvider { get; set; }

        public AuxiliariesCapabilities? AuxiliariesCapabilities { get => SIUAuxiliariesCapabilities; set { SIUAuxiliariesCapabilities = value; } }

        public AuxiliariesStatus AuxiliariesStatus { get; set; } = new AuxiliariesStatus(HandsetSensor: AuxiliariesStatus.HandsetSensorStatusEnum.OffTheHook, Heating: AuxiliariesStatus.SensorEnum.Off);

        AutoStartupTimeModeEnum AutoStartupTimeModeEnum { get; set; } = AutoStartupTimeModeEnum.Clear;
        StartupTime AutoStartupTime { get; set; } = null;

        public Task<DeviceResult> ClearAutoStartupTime(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<GetAutostartupTimeResult> GetAutoStartupTime(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public async Task RunAsync(CancellationToken Token)
        {
            for (; ; )
            {
                UpdateStatus();
                await Task.Delay(10000, Token);
            }
        }

        public async Task<DeviceResult> SetAutostartupTime(SetAutostartupTimeRequest autoStartupInfo, CancellationToken cancellation)
        {
            await Task.Delay(100000);
            throw new NotImplementedException();
        }

        public async Task<DeviceResult> SetAuxiliaries(SetAuxiliariesRequest request, CancellationToken cancellation)
        {
            await Task.Delay(100000);
            throw new NotImplementedException();
        }

        public void PortEventHandle(ref WFSSIUPORTEVENT300 eventData)
        {
            AuxiliariesServiceProvider? auxiliariesServiceProvider = SetServiceProvider as AuxiliariesServiceProvider;
            if(auxiliariesServiceProvider == null)
            {
                return;
            }

            switch (eventData.wPortType)
            {
                case FwType.WFS_SIU_SENSORS:
                    switch(eventData.wPortIndex)
                    {
                        case WSensorIndices.WFS_SIU_OPERATORSWITCH:
                            auxiliariesServiceProvider.OperatorSwitchStateChanged(FwConst.ToOperatorSwitchStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_TAMPER:
                            auxiliariesServiceProvider.TamperSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_INTTAMPER:
                            auxiliariesServiceProvider.InternalTamperSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_SEISMIC:
                            auxiliariesServiceProvider.SeismicSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_HEAT:
                            auxiliariesServiceProvider.HeatSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_PROXIMITY:
                            auxiliariesServiceProvider.ProximitySensorStateChanged(FwConst.ToPresenceSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_AMBLIGHT:
                            auxiliariesServiceProvider.AmbientLightSensorStateChanged(FwConst.ToAmbientLightSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_ENHANCEDAUDIO:
                            auxiliariesServiceProvider.EnchancedAudioSensorStateChanged(FwConst.ToPresenceSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_BOOT_SWITCH:
                            auxiliariesServiceProvider.BootSwitchSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_CONSUMER_DISPLAY:
                            auxiliariesServiceProvider.DisplaySensorStateChanged(FwConst.ToDisplaySensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_OPERATOR_CALL_BUTTON:
                            auxiliariesServiceProvider.OperatorCallButtonSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_HANDSETSENSOR:
                            auxiliariesServiceProvider.HandsetSensorStateChanged(FwConst.ToHandsetSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_HEADSETMICROPHONE:
                            auxiliariesServiceProvider.HeadsetMicrophoneSensorStateChanged(FwConst.ToPresenceSensorStatus(eventData.wPortStatus));
                            break;
                        case WSensorIndices.WFS_SIU_FASCIAMICROPHONE:
                            auxiliariesServiceProvider.FasciaMicrophoneSensorStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        default:
                            throw new NotImplementedException($"Cant not find event for WFS_SIU_SENSORS port {eventData.wPortIndex}");
                    }
                    break;

                case FwType.WFS_SIU_DOORS:
                    switch(eventData.wPortIndex)
                    {
                        case WDoorIndices.WFS_SIU_VANDALSHIELD:
                            auxiliariesServiceProvider.VandalShieldSensorStateChanged(FwConst.ToVandalShieldStatus(eventData.wPortStatus));
                            break;

                        default:
                            auxiliariesServiceProvider.DoorSensorStateChanged(WDoorIndices.ToDoorType(eventData.wPortIndex), FwConst.ToDoorStatus(eventData.wPortStatus));
                            break;
                    }
                    break;
                case FwType.WFS_SIU_INDICATORS:
                    switch (eventData.wPortIndex)
                    {
                        case WIndicatorIndices.WFS_SIU_OPENCLOSE:
                            auxiliariesServiceProvider.OpenClosedIndicatorStateChanged(FwConst.ToOpenClosedIndicatorStatus(eventData.wPortStatus));
                            break;
                        case WIndicatorIndices.WFS_SIU_AUDIO:
                            (var newRate, var newSignal) = FwConst.ToAudioStatus(eventData.wPortStatus);
                            auxiliariesServiceProvider.AudioStateChanged(newRate, newSignal);
                            break;
                        case WIndicatorIndices.WFS_SIU_HEATING:
                            auxiliariesServiceProvider.HeatingStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WIndicatorIndices.WFS_SIU_CONSUMER_DISPLAY_BACKLIGHT:
                            auxiliariesServiceProvider.ConsumerDisplayBacklightStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WIndicatorIndices.WFS_SIU_SIGNAGEDISPLAY:
                            auxiliariesServiceProvider.SignageDisplayStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        default:
                            throw new NotImplementedException($"Cant not find event for WFS_SIU_INDICATORS port {eventData.wPortIndex}");
                    }
                    break;
                case FwType.WFS_SIU_AUXILIARIES:
                    switch (eventData.wPortIndex)
                    {
                        case WAuxiliaryIndices.WFS_SIU_VOLUME:
                            auxiliariesServiceProvider.VolumeStateChanged(eventData.wPortStatus);
                            break;
                        case WAuxiliaryIndices.WFS_SIU_UPS:
                            auxiliariesServiceProvider.UpsStateChanged(FwConst.ToUpsStatus(eventData.wPortStatus));
                            break;
                        case WAuxiliaryIndices.WFS_SIU_AUDIBLE_ALARM:
                            auxiliariesServiceProvider.AudibleAlarmStateChanged(FwConst.ToSensorStatus(eventData.wPortStatus));
                            break;
                        case WAuxiliaryIndices.WFS_SIU_ENHANCEDAUDIOCONTROL:
                            auxiliariesServiceProvider.EnhancedAudioControlStateChanged(FwConst.ToEnhancedAudioControlStatus(eventData.wPortStatus));
                            break;
                        case WAuxiliaryIndices.WFS_SIU_ENHANCEDMICROPHONECONTROL:
                            auxiliariesServiceProvider.EnhancedMicrophoneControlStateChanged(FwConst.ToEnhancedAudioControlStatus(eventData.wPortStatus));
                            break;
                        case WAuxiliaryIndices.WFS_SIU_MICROPHONEVOLUME:
                            auxiliariesServiceProvider.MicrophoneVolumeStateChanged(FwConst.ToMicrophoneVolumeStatus(eventData.wPortStatus));
                            break;
                        default:
                            throw new NotImplementedException($"Cant not find event for WFS_SIU_AUXILIARIES port {eventData.wPortIndex}");
                    }
                    break;
            }
        }
    }
}
