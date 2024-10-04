using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.DTOs
{
    public class RequestDto
    {
        public int RequestId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CertificateType { get; set; }
        public string Purpose { get; set; }
        public string AdditionalComments { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
