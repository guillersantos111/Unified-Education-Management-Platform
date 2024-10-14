using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface IAssignSubjectRepository
    {
        Task<List<SubjectandSchedulesEntity>> GetSubjectsByCourseId(int courseId);
        Task<IEnumerable<SubjectandSchedulesEntity>> GetSubjectsByCourseIdAsync(int courseId);
        Task<IEnumerable<StudentSubjectEnrollmentEntity>> GetAssignedOrEnrolledStudentsAsync();
        Task<bool> AssignSubjectToStudentAsync(int studentId, int subjectId, int courseId);
        Task<bool> RemoveAssignedSubjectToStudentAsync(int studentId);
    }
}
