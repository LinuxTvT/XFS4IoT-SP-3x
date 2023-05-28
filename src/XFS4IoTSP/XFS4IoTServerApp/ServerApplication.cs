using Lights.XFS3xLights;
using Printer.XFS3xPrinter;
using System.Diagnostics;
using XFS3xCardReader;
using XFS3xPinPad;
using XFS4IoTServer;

namespace XFS4IoTServerApp
{
    internal class ServerApplication
    {
        static async Task Main(/*string[] args*/)
        {
            try
            {
                Console.WriteLine("Running Server Application");
                Debug.WriteLine("Running Server Application");
                Logger.Info($"Running Server Application");

                var Publisher = new ServicePublisher(new NLogLogger(NLog.LogManager.GetLogger("ServicePublisher")), new ServiceConfiguration());
                var EndpointDetails = Publisher.EndpointDetails;

                // Cardreader Service Provider
                /*
               var cardReaderDevice = new CardReaderDevice("IDC30");
               var cardReaderService = new CardReaderServiceProvider(EndpointDetails,
                                                               ServiceName: "CardReader",
                                                               cardReaderDevice,
                                                               new NLogLogger("Devices.CardReader"),
                                                               new FilePersistentData());
               cardReaderDevice.SetServiceProvider = cardReaderService;
                */
                /*
                // Pin Pad Service Provider
                var serviceName = "PIN30";
                var pinPadDevice = new XFS3xPinPadDevice(serviceName);
                var PinPadService = new PinPadServiceProvider(EndpointDetails,
                                                                ServiceName: serviceName,
                                                                pinPadDevice,
                                                                new NLogLogger("Devices.PinPad"),
                                                                pinPadDevice);
                pinPadDevice.SetServiceProvider = PinPadService;
                */

                // Printer Service
                /*
                var ptrServiceName = "PrintReceipt";
                var printerDevice = new XFS3xPrinter(ptrServiceName);
                var printerService = new PrinterServiceProvider(EndpointDetails,
                                                                ServiceName: ptrServiceName,
                                                                printerDevice,
                                                                new NLogLogger("Devices.Printer"),
                                                                printerDevice);
                printerDevice.SetServiceProvider = printerService;
                */

                var lightsServiceName = "SIU30";
                var lightsDevice = new XFS3xLights(lightsServiceName);
                var lightsService = new LightsServiceProvider(EndpointDetails,
                                                                ServiceName: lightsServiceName,
                                                                lightsDevice,
                                                                new NLogLogger("Devices.Lights"));
                lightsDevice.SetServiceProvider = lightsService;

                //Publisher.Add(cardReaderService);
                //Publisher.Add(PinPadService);
                //Publisher.Add(printerService);
                Publisher.Add(lightsService);

                // CancellationSource object allows to restart service when it's signalled.
                CancellationSource cancelToken = new(new NLogLogger(NLog.LogManager.GetLogger("CancellationSource")));
                await Publisher.RunAsync(cancelToken);

                Console.WriteLine("Exit Server Application");

            }
            catch (Exception e) when (e.InnerException != null)
            {
                Logger.Warn($"Unhandled exception {e.InnerException.Message}");
            }
            catch (Exception e)
            {
                Logger.Warn($"Unhandled exception {e.Message}");
            }

        }

        /// <summary>
        /// NLog logger
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
