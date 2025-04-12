namespace WebApplication3.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan duration);
        void Remove(string key);
    }
}
