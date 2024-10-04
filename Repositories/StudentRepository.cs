using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbContextOptions<UEMPDbContext> _dbContextOptions;

        public StudentRepository(DbContextOptions<UEMPDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }


        public async Task <IEnumerable<StudentsEntity>> GetAllStudentsAsync()
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.Students
                    .Include(s => s.Courses)
                    .ToListAsync();
            }
        }


        public async Task<StudentsEntity> GetStudentByIdAsync(int studentId)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.Students
                    .Include(s => s.Courses)
                    .FirstOrDefaultAsync(s => s.StudentId == studentId);
            }
        }

        public async Task<StudentsEntity> GetStudentByEmailAsync(string email)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.Students
                    .FirstOrDefaultAsync(s => s.Email == email);
            }
        }



        public async Task<int> AddStudentAsync(StudentsEntity student)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                await dbContextOptions.Students.AddAsync(student);
                await dbContextOptions.SaveChangesAsync();

                return student.StudentId; // Added Successfully
            }
        }


        public async Task<bool> UpdateStudentAsync(int studentId, StudentsEntity updatedStudent)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                var existingStudent = await dbContextOptions.Students.FindAsync(studentId);
                if (existingStudent == null)
                {
                    return false; // Student Not Found
                }

                existingStudent.FirstName = updatedStudent.FirstName;
                existingStudent.LastName = updatedStudent.LastName; 
                existingStudent.Email = updatedStudent.Email;
                existingStudent.Gender = updatedStudent.Gender; 
                existingStudent.BirthDate = updatedStudent.BirthDate; 
                existingStudent.Address = updatedStudent.Address; 
                existingStudent.Contact = updatedStudent.Contact; 
                existingStudent.ZipCode = updatedStudent.ZipCode; 
                existingStudent.PlaceOfBirth = updatedStudent.PlaceOfBirth; 
                existingStudent.CivilStatus = updatedStudent.CivilStatus; 
                existingStudent.Nationality = updatedStudent.Nationality; 
                existingStudent.Religion = updatedStudent.Religion; 
                existingStudent.Height = updatedStudent.Height; 
                existingStudent.Weight = updatedStudent.Weight; 
                existingStudent.Campus = updatedStudent.Campus; 
                existingStudent.CourseCode = updatedStudent.CourseCode;

                await dbContextOptions.SaveChangesAsync();
                return true; // Updated Successfully
            }
        }


        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                var student = await dbContextOptions.Students.FindAsync(studentId);

                if (student != null)
                {
                    dbContextOptions.Students.Remove(student);
                    await dbContextOptions.SaveChangesAsync();

                    return true; // Deleted Successfully
                }

                return false; // Student Not Found
            }
        }
    }
}
