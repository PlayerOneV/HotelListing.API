namespace HotelListing.API.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();  
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> Exists(int id);
    }
}
