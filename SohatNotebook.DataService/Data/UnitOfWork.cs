using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.DataService.IRepository;
using SohatNotebook.DataService.Repository;

namespace SohatNotebook.DataService.Data;

public class UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory) : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<UnitOfWork> _logger = loggerFactory.CreateLogger<UnitOfWork>();

    public IUsersRepository UsersRepository { get; private set; }
        = new UsersRepository(context, loggerFactory.CreateLogger<UsersRepository>());

    public IRefreshTokensRepository RefreshTokens { get; private set; }
        = new RefreshTokensRepository(context, loggerFactory.CreateLogger<RefreshTokensRepository>());

    public IHealthDataRepository HealthDataRepository { get; private set; }
        = new HealthDataRepository(context, loggerFactory.CreateLogger<HealthDataRepository>());

    public async Task CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();

}