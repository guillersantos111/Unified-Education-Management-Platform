using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface IStudentRepository
    {
        Task<int> AddStudentAsync(StudentsEntity student);
        Task <bool>UpdateStudentAsync(int studentId, StudentsEntity updatedStudent);
        Task<bool> DeleteStudentAsync (int studentId);
        Task <StudentsEntity> GetStudentByIdAsync(int studentId);
        Task <IEnumerable<StudentsEntity>> GetAllStudentsAsync();
        Task<StudentsEntity> GetStudentByEmailAsync(string email);
    }
}
