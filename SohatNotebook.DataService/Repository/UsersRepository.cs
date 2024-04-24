using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IRepository;
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.Repository;

public class UsersRepository(AppDbContext context, ILogger<UsersRepository> logger)
    : GenericRepository<User>(context, logger), IUsersRepository
{
    public override async Task<IEnumerable<User>> GetAllAysnc()
    {
        try
        {
            return await dbset.Where(user => user.Status == 1).AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} GetAll Mehtod has generated an Error", typeof(UsersRepository));
            return [];
        }
    }

    public async Task<User?> GetUserByIdentityIdAsync(Guid id)
    {
        try
        {
            return await dbset.FirstOrDefaultAsync(user => user.Status == 1 && user.IdentityId == id);
        }
        catch (Exception ex)
        {
            _logger.LogError
            (
                ex,
                "{Repo} GetUserByIdentityIdAsync Mehtod has generated an Error",
                typeof(UsersRepository)
            );
            return null;
        }
    }

    public async Task<bool> UpdateUserProfileAsync(User user)
    {
        try
        {
            var updatedUser = await dbset.FirstOrDefaultAsync(dbUser => dbUser.Status == 1 && dbUser.Id == user.Id);
            if (updatedUser is null)
                return false;

            updatedUser.FirstName = user.FirstName;
            updatedUser.LastName = user.LastName;
            updatedUser.Address = user.Address;
            updatedUser.Country = user.Country;
            updatedUser.MobileNumber = user.MobileNumber;
            updatedUser.Phone = user.Phone;
            updatedUser.Sex = user.Sex;
            updatedUser.UpdateDate = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Repo} GetAll Mehtod has generated an Error", typeof(UsersRepository));
            return false;
        }
    }
}