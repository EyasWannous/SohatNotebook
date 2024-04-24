using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IRepository;
using SohatNotebook.Entities.DbSet;

namespace SohatNotebook.DataService.Repository;

public class RefreshTokensRepository(AppDbContext context, ILogger<GenericRepository<RefreshToken>> logger)
    : GenericRepository<RefreshToken>(context, logger), IRefreshTokensRepository
{
    public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
    {
        try
        {
            return await dbset.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }
        catch (Exception ex)
        {
            _logger.LogError
            (
                ex,
                "{Repo} GetByRefreshTokenAsync Mehtod has generated an Error",
                typeof(RefreshTokensRepository)
            );

            return null;
        }
    }

    public async Task<bool> MakeRefreshTokenAsUsedAsync(RefreshToken refreshToken)
    {
        try
        {
            var token = await dbset.FirstOrDefaultAsync(rt => rt.Token == refreshToken.Token);
            if (token is null)
                return false;

            token.IsUsed = refreshToken.IsUsed;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError
            (
                ex,
                "{Repo} MakeRefreshTokenAsUsedAsync Mehtod has generated an Error",
                typeof(RefreshTokensRepository)
            );

            return false;
        }
    }
}