using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.DTOs;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class AssignSubjectRepository : IAssignSubjectRepository
    {
        private readonly DbContextOptions<UEMPDbContext> _dbContextOptions;

        public AssignSubjectRepository(DbContextOptions<UEMPDbContext> dbContextOptions)
        {
            this._dbContextOptions = dbContextOptions;
        }


        public async Task<List<StudentsDto>> GetStudentsAsync()
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.Students
                    .Select(s => new StudentsDto
                    {
                        StudentId = s.StudentId,
                        LastName = s.LastName,
                        FirstName = s.FirstName,
                        MiddleName = s.MiddleName,
                        CourseCode = s.CourseCode
                    })
                    .ToListAsync();
            }
        }


        public async Task<IEnumerable<StudentSubjectEnrollmentEntity>> GetAssignedOrEnrolledStudentsAsync()
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.StudentSubjectEnrollment
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.Subjects
                    .Select(sb => new SubjectDto
                    {
                        SubjectCode = sb.SubjectCode,
                        DescriptiveTitle = sb.DescriptiveTitle
                    })
                    .ToListAsync();
            }
        }
        public async Task<IEnumerable<SubjectsEntity>> GetSubjectsByCourseIdAsync(int courseId)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.Subjects
                    .Where(sb => sb.CourseId == courseId)
                    .ToListAsync();
            }
        }


        public async Task<bool>AssignSubjectToStudentAsync(int studentId, int subjectId)
        {
            using (var dbContextOption = new UEMPDbContext(_dbContextOptions))
            {
                var studentSubjectEnrollment = new StudentSubjectEnrollmentEntity
                {
                    StudentId = studentId,
                    SubjectId = subjectId
                };

                // Attemp to Add the Entity and Save Changes
                await dbContextOption.StudentSubjectEnrollment.AddAsync(studentSubjectEnrollment);
                return await dbContextOption.SaveChangesAsync() > 0; // Return True If The Changers is Successful
            }
        }
    }
}
