using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class ActivityLogsRepository : IActivityLogsRepository
    {
        private readonly DbContextOptions<UEMPDbContext> _dbContextOptions;

        public ActivityLogsRepository(DbContextOptions<UEMPDbContext> dbContextOptions) 
        {
            _dbContextOptions = dbContextOptions;
        }


        public async Task AddActivityLog(ActivityLogsEntity activityLogsEntity)
        {
            using (var dbContextOptnions = new UEMPDbContext(_dbContextOptions))
            {
                dbContextOptnions.ActivityLogs.Add(activityLogsEntity);
                await dbContextOptnions.SaveChangesAsync();
            }
        }


        public async Task<List<ActivityLogsEntity>> GetStudentActivityLogs(int studentId)
        {
           using (var dbContextOptnions = new UEMPDbContext(_dbContextOptions))
           {
                return await dbContextOptnions.ActivityLogs
                    .Where(logs => logs.StudentId == studentId)
                    .OrderByDescending(logs => logs.Timestamp)
                    .ToListAsync();
           }
        }

        public async Task <List<ActivityLogsEntity>> GetStudentActivityLogsInAdminSide()
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                 return await dbContextOptions.ActivityLogs
                     .ToListAsync();
            }
        }
    }
}
