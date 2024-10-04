using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class AdmissionEntity
    {
        public int AdmissionId { get; set; }
        public int StudentId { get; set; }
        public DateTime AdmissionDate { get; set; }

        public StudentsEntity Student { get; set; }
    }
}
