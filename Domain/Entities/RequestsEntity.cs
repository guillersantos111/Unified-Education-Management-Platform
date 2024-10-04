using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class RequestsEntity
    {
        public int RequestId { get; set; }
        public int StudentId { get; set; }
        public string CertificateType { get; set; }
        public string Purpose { get; set; }
        public string AdditionalComments { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public StudentsEntity Students { get; set; }
    }
}
