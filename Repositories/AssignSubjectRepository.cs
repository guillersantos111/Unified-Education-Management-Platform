using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            _dbContextOptions = dbContextOptions;
        }

        public async Task<List<SubjectandSchedulesEntity>> GetSubjectsByCourseId(int courseId)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.SubjectandSchedules
                    .Where(sb => sb.CourseId == courseId)
                    .ToListAsync();
            }
        }




        public async Task<IEnumerable<StudentSubjectEnrollmentEntity>> GetAssignedOrEnrolledStudentsAsync()
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.StudentSubjectEnrollment
                    .Select(sse => new StudentSubjectEnrollmentEntity 
                    {
                        StudentId = sse.StudentId
                    })
                    .ToListAsync();
            }
        }


        public async Task<IEnumerable<SubjectandSchedulesEntity>> GetSubjectsByCourseIdAsync(int courseId)
        {
            using (var dbContextOptions = new UEMPDbContext(_dbContextOptions))
            {
                return await dbContextOptions.SubjectandSchedules
                    .Where(sb => sb.CourseId == courseId)
                    .ToListAsync();
            }
        }


        public async Task<bool>AssignSubjectToStudentAsync(int studentId, int subjectId, int courseId)
        {
            using (var dbContextOption = new UEMPDbContext(_dbContextOptions))
            {

                var existingEnrollment = await dbContextOption.StudentSubjectEnrollment
                    .FirstOrDefaultAsync(e => e.StudentId == studentId && e.SubjectId == subjectId && e.CourseId == courseId);

                if (existingEnrollment != null)
                {
                    MessageBox.Show("Student Already Enrolled", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                var studentSubjectEnrollment = new StudentSubjectEnrollmentEntity
                {
                    StudentId = studentId,
                    SubjectId = subjectId,
                    CourseId = courseId
                };

                await dbContextOption.StudentSubjectEnrollment.AddAsync(studentSubjectEnrollment);
                return await dbContextOption.SaveChangesAsync() > 0; // Return True If The Changes is Successful
            }
        }

        public async Task<bool> RemoveAssignedSubjectToStudentAsync(int studentId)
        {
            using (var dbContextOption = new UEMPDbContext(_dbContextOptions))
            {
                var enrollment = await dbContextOption.StudentSubjectEnrollment
                    .FirstOrDefaultAsync(sse => sse.StudentId == studentId);

                if (enrollment != null)
                {
                    dbContextOption.StudentSubjectEnrollment.Remove(enrollment);
                    await dbContextOption.SaveChangesAsync();
                    return true;
                }

                return false; // No entry found
            }
        }
    }
}
