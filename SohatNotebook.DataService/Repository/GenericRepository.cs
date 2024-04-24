using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IRepository;

namespace SohatNotebook.DataService.Repository;

public class GenericRepository<T>(AppDbContext context, ILogger<GenericRepository<T>> logger) : IGenericRepository<T> where T : class
{
    protected readonly ILogger<GenericRepository<T>> _logger = logger;
    protected readonly AppDbContext _context = context;
    internal DbSet<T> dbset = context.Set<T>();

    public virtual async Task<bool> AddAysnc(T entity)
    {
        await dbset.AddAsync(entity);

        return true;
    }

    public Task<bool> DeleteAysnc(Guid id, string userId)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<IEnumerable<T>> GetAllAysnc()
        => await dbset.ToListAsync();

    public virtual async Task<T?> GetByIdAysnc(Guid id)
        => await dbset.FindAsync(id);

    public Task<bool> UpsertAysnc(T entity)
    {
        throw new NotImplementedException();
    }
}