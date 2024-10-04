using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class SubjectsEntity
    {
        [Key]
        public int SubjectId { get; set; }

        [ForeignKey ("CourseId")]
        public int CourseId { get; set; }

        public string YearLevel { get; set; }

        public string Term { get; set; }

        public string SubjectCode { get; set; }

        public string DescriptiveTitle { get; set; }

        public int Credits { get; set; }

        public string PreRequisites { get; set; }


        public virtual ICollection<StudentSubjectEnrollmentEntity> StudentSubjectEnrollment { get; set; }
    }
}
