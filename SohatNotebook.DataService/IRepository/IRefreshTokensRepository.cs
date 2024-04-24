using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.IRepository;

public interface IRefreshTokensRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken);
    Task<bool> MakeRefreshTokenAsUsedAsync(RefreshToken refreshToken);
}