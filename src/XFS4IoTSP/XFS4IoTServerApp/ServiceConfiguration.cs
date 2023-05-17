using System.Configuration;
using XFS4IoTServer;

namespace XFS4IoTServerApp
{
    internal class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
            try
            {
                var exeConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (exeConfig != null)
                {
                    var appSettings = exeConfig.AppSettings;
                    if (appSettings != null)
                    {
                        Settings = appSettings.Settings;
                    }
                }
                Settings ??= new KeyValueConfigurationCollection();
            }
            catch (ConfigurationErrorsException ex)
            {
                Settings = new KeyValueConfigurationCollection();
                Logger.Warn($"Exception caught in the constructor {nameof(ServiceConfiguration)}. {ex.Message}");
            }
        }

        /// <summary>
        /// Get configuration value associated with the key specified.
        /// Returns null if specified value doesn't exist in the configuration. 
        /// </summary>
        /// <param name="name">Name of the configuration value</param>
        /// <returns>Configuration value</returns>
        public string? Get(string name)
        {
            var configValue = Settings[name]?.Value;
            Logger.Warn($"Configuration Get({name}={configValue} in {nameof(ServiceConfiguration)}");
            return configValue;
        }

        /// <summary>
        /// NLog logger
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// The collection of configuration value
        /// </summary>
        private KeyValueConfigurationCollection Settings { get; init; }
    }
}
