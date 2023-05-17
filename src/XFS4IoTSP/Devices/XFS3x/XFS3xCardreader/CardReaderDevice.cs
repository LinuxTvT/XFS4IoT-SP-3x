namespace XFS3xCardReader
{

    public partial class CardReaderDevice
    {
        public static readonly AutoResetEvent MediaInsertEvent = new(false);
        public static readonly AutoResetEvent MediaDetectedEvent = new(false);
        public static readonly AutoResetEvent MediaRemovedEvent = new(false);
        public static readonly AutoResetEvent ExecuteCompleteEvent = new(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="timouet"></param>
        /// <returns></returns>
        public static async Task<bool> WaitOne(WaitHandle handle, int timouet = -1) => await Task.Run(() => { return handle.WaitOne(timouet); });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handles"></param>
        /// <param name="timouet"></param>
        /// <returns></returns>
        public static async Task<int> WaitAny(WaitHandle[] handles, int timouet = -1) => await Task.Run(() => { return WaitHandle.WaitAny(handles, timouet); });

        /// <summary>
        /// NLog logger for this class
        /// </summary>
        private static readonly NLog.Logger s_logger = NLog.LogManager.GetCurrentClassLogger();
    }


}
