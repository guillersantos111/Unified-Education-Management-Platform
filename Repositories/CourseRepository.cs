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
    public class CourseRepository : ICourseRepository
    {
        private readonly UEMPDbContext applicationDBContext;

        public CourseRepository(UEMPDbContext applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }

        public async Task<List<CoursesEntity>> GetAllCoursesAsync()
        {
            return await applicationDBContext.Courses
                .ToListAsync();
        }
    }
}
