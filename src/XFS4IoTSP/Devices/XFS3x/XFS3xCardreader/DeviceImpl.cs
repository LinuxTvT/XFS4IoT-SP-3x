using XFS4IoT;
using XFS4IoTServer;

namespace XFS3xCardReader
{
    public partial class CardReaderDevice
    {
        public async Task RunAsync(CancellationToken token)
        {
            CardReaderServiceProvider? cardReaderServiceProvider = SetServiceProvider as CardReaderServiceProvider;
            for (; ; )
            {
                Device.UpdateStatus(CommonStatus, CardReaderStatus);
                bool haveEvent = await WaitOne(MediaRemovedEvent, 1000);
                if (haveEvent)
                {
                    await cardReaderServiceProvider.IsNotNull().MediaRemovedEvent();
                }
            }
        }

        public XFS4IoTServer.IServiceProvider? SetServiceProvider { get; set; } = null;

    }
}