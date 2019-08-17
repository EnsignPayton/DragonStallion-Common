using System.Threading.Tasks;

namespace DragonStallion.Common.Config
{
    public interface IConfigProvider<T>
    {
        Task<T> GetConfigAsync();
        Task SetConfigAsync(T config);
    }
}