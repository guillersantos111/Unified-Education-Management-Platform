using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class StudentSubjectEnrollmentEntity
    {
        [Key]
        public int StudentSubjectEnrollmentId { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [ForeignKey("SubjectId")]
        public int SubjectId { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }


        public virtual StudentsEntity Students { get; set; }
        public virtual SubjectandSchedulesEntity Subjects { get; set; }
        public virtual CoursesEntity Courses { get; set; }
    }
}
