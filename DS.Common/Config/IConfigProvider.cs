using System.Threading.Tasks;

namespace DS.Common.Config
{
    public interface IConfigProvider<T>
    {
        Task<T> GetConfigAsync();
        Task SetConfigAsync(T config);
    }
}