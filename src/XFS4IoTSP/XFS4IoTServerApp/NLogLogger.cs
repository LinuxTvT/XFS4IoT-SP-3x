using NLog;

namespace XFS4IoTServerApp
{
    internal class NLogLogger : XFS4IoT.ILogger
    {

        public NLogLogger(Logger logger)
        {
            Logger = logger;
        }
        public NLogLogger(string name)
        {
            Logger = NLog.LogManager.GetLogger(name);
        }

        public void Trace(string SubSystem, string Operation, string Message)
        {
            Logger.Info(SubSystem + " - " + Message);
        }


        public void Warning(string SubSystem, string Message)
        {
            Logger.Warn(SubSystem + " - " + Message);
        }

        public void Log(string SubSystem, string Message)
        {
            Logger.Info(SubSystem + " - " + Message);
        }

        public void TraceSensitive(string SubSystem, string Operation, string Message) => Trace(SubSystem, Operation, Message);

        public void WarningSensitive(string SubSystem, string Message) => Trace(SubSystem, "WARNING", Message);

        public void LogSensitive(string SubSystem, string Message) => Trace(SubSystem, "INFO", Message);

        private NLog.Logger Logger { get; init; }
    }
}
