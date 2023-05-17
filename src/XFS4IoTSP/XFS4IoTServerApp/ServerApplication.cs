using System.Diagnostics;
using XFS3xCardReader;
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
                var cardReaderDevice = new CardReaderDevice("IDC30");
                var cardReaderService = new CardReaderServiceProvider(EndpointDetails,
                                                                ServiceName: "CardReader",
                                                                cardReaderDevice,
                                                                new NLogLogger("Devices.CardReader"),
                                                                new FilePersistentData());
                cardReaderDevice.SetServiceProvider = cardReaderService;


                Publisher.Add(cardReaderService);

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
