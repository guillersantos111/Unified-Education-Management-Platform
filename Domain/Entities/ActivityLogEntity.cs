using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class ActivityLogsEntity
    {
        [Key]
        public int LogId { get; set; }

        public int StudentId { get; set; } 
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }

        public virtual StudentsEntity Students { get; set; }
    }

}
