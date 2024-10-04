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

        public int AcademicYear { get; set; }

        public string Term { get; set; }

        public virtual StudentsEntity Students { get; set; }
        public virtual SubjectsEntity Subjects { get; set; }
    }
}
