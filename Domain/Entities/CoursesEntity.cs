using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class CoursesEntity
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Course { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseCode { get; set; }

        public string Campus { get; set; }

        public string Department { get; set; }

        public virtual ICollection<StudentsEntity> Students { get; set; }    }
}
