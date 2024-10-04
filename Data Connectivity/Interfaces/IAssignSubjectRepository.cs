using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.DTOs;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface IAssignSubjectRepository
    {
        Task<List<StudentsDto>> GetStudentsAsync();
        Task<IEnumerable<SubjectsEntity>> GetSubjectsByCourseIdAsync(int courseId);
        Task<IEnumerable<StudentSubjectEnrollmentEntity>> GetAssignedOrEnrolledStudentsAsync();
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
        Task<bool> AssignSubjectToStudentAsync(int studentId, int subjectId);
    }
}
