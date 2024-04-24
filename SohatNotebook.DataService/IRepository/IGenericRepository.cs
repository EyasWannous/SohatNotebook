namespace SohatNotebook.DataService.IRepository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAysnc();
    Task<T?> GetByIdAysnc(Guid id);
    Task<bool> AddAysnc(T entity);
    Task<bool> DeleteAysnc(Guid id, string userId);
    Task<bool> UpsertAysnc(T entity);
}