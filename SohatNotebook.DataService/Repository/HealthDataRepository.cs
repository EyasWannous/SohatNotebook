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

public class HealthDataRepository(AppDbContext context, ILogger<GenericRepository<HealthData>> logger)
    : GenericRepository<HealthData>(context, logger), IHealthDataRepository
{
    public async Task<bool> UpdateHealthDataAysnc(HealthData healthData)
    {
        try
        {
            var updatedHealthData = await dbset.FirstOrDefaultAsync
            (
                dbHealthData => dbHealthData.Status == 1
                && dbHealthData.Id == healthData.Id
            );

            if (updatedHealthData is null)
                return false;

            updatedHealthData.BloodType = healthData.BloodType;
            updatedHealthData.Height = healthData.Height;
            updatedHealthData.Weight = healthData.Weight;
            updatedHealthData.Race = healthData.Race;
            updatedHealthData.UseGlasses = healthData.UseGlasses;
            updatedHealthData.UpdateDate = DateTime.UtcNow;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError
            (
                ex,
                "{Repo} UpdateHealthDataAysnc Mehtod has generated an Error",
                typeof(HealthDataRepository)
            );

            return false;
        }
    }

    public override async Task<IEnumerable<HealthData>> GetAllAysnc()
    {
        try
        {
            return await dbset.Where(healthData => healthData.Status == 1).AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError
            (
                ex,
                "{Repo} GetAllAsync Mehtod has generated an Error",
                typeof(HealthDataRepository)
            );

            return [];
        }
    }

    public override async Task<HealthData?> GetByIdAysnc(Guid id)
    {
        try
        {
            return await dbset.FirstOrDefaultAsync(healthData => healthData.Status == 1 && healthData.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError
            (
                ex,
                "{Repo} GetByIdAsync Mehtod has generated an Error",
                typeof(HealthDataRepository)
            );

            return null;
        }
    }
}