using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DragonStallion.Common.Config
{
    public class FileConfigProvider<T> : IConfigProvider<T>
    {
        private readonly AppSettings _appSettings;

        public FileConfigProvider(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<T> GetConfigAsync()
        {
            if (File.Exists(_appSettings.ConfigPath))
            {
                var json = await File.ReadAllTextAsync(_appSettings.ConfigPath);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }

        public async Task SetConfigAsync(T config)
        {
            var configFolder = Path.GetDirectoryName(_appSettings.ConfigPath);
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            await File.WriteAllTextAsync(_appSettings.ConfigPath, json);
        }
    }
}