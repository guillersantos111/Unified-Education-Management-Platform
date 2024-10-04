using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class AssignmentEntity
    {
        public int AssignmentId { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }


        [ForeignKey("StudentId")]
        public virtual StudentsEntity Students { get; set; }

        [ForeignKey("SubjectId")]
        public virtual SubjectsEntity Subjects { get; set; }

    }
}
