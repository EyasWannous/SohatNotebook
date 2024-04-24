using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.IRepository;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<User?> GetUserByIdentityIdAsync(Guid id);
    Task<bool> UpdateUserProfileAsync(User user);
}