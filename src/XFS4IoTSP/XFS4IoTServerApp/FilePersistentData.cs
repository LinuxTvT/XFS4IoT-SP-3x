using System.Text.Json;
using XFS4IoTServer;

namespace XFS4IoTServerApp
{
    internal class FilePersistentData : IPersistentData
    {
        public FilePersistentData()
        {
        }

        private static string FileName(string name)
        {
            return $"storage/{name}";
        }

        public bool Store<TValue>(string name, TValue obj) where TValue : class
        {
            string data;
            var options = new JsonSerializerOptions { WriteIndented = true };
            Logger.Debug($"==> Store value, name = [{name}]");
            try
            {
                data = JsonSerializer.Serialize<TValue>(obj, options);
                if (string.IsNullOrEmpty(data))
                    return false;
            }
            catch (Exception ex)
            {
                Logger.Warn($"Exception caught on serializing persistent data. {ex.Message}");
                return false;
            }

            // The data is serialized and stored it on the file system
            try
            {
                File.WriteAllText(FileName(name), data);
            }
            catch (Exception ex)
            {
                Logger.Warn($"Exception caught on writing data. {name}, {ex.Message}");
                return false;
            }

            return true;
        }

        public TValue? Load<TValue>(string name) where TValue : class
        {
            // Load serialized data from the file system

            Logger.Debug($"<== Load value, name = [{name}]");
            string data;
            try
            {
                data = File.ReadAllText(FileName(name));
            }
            catch (Exception ex)
            {
                Logger.Warn($"Exception caught on reading persistent data. {name}, {ex.Message}");
                return null;
            }

            TValue? value;
            // Unserialize read data
            try
            {
                value = JsonSerializer.Deserialize<TValue>(data);
            }
            catch (Exception ex)
            {
                Logger.Warn($"Exception caught on unserializing persistent data. {ex.Message}");
                return null;
            }

            return value;
        }

        /// <summary>
        /// NLog logger
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    }
}
