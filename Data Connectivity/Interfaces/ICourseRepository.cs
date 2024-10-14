using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface ICourseRepository
    {
        Task <List<CoursesEntity>> GetAllCoursesAsync();
    }
}
