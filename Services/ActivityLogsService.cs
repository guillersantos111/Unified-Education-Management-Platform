using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Services
{
    public class ActivityLogsService
    {
        private readonly IActivityLogsRepository activityLogsRepository;

        public ActivityLogsService(IActivityLogsRepository activityLogsRepository)
        {
            this.activityLogsRepository = activityLogsRepository;
        }

        public Task<List<ActivityLogsEntity>> GetStudentActivityLogs(int studentId)
        {
            return activityLogsRepository.GetStudentActivityLogs(studentId);
        }

        public async Task LogActivity(int studentId, string action, string details, string LogLevel = "Info")
        {
            var logs = new ActivityLogsEntity
            {
                StudentId = studentId,
                Action = action,
                Timestamp = DateTime.Now,
                Details = details 
            };

            await activityLogsRepository.AddActivityLog(logs);
        }
    }
}
