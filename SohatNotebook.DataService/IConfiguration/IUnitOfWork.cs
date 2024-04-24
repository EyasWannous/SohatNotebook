using SohatNotebook.DataService.IRepository;

namespace SohatNotebook.DataService.IConfiguration;

public interface IUnitOfWork
{
    IUsersRepository UsersRepository { get; }
    IRefreshTokensRepository RefreshTokens { get; }
    IHealthDataRepository HealthDataRepository { get; }
    Task CompleteAsync();
}