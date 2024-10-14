using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Domain
{
    public class StudentsEntity
    {
        [Key]
        public int StudentId { get; set; }

        public int? CourseId { get; set; }

        public byte[] ProfilePicture { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        public string FullName => $"{LastName + ","} {FirstName + ","} {MiddleName}".Trim();


        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public long Contact { get; set; }

        [Required]
        public int? ZipCode { get; set; }

        [Required]
        public string Campus { get; set; }

        [Required]

        public string PlaceOfBirth { get; set; }

        [Required]

        public string CivilStatus { get; set; }

        [Required]

        public string Nationality { get; set; }

        [Required]

        public string Religion { get; set; }

        [Required]
        public int? Height { get; set; }

        [Required]
        public int? Weight { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public virtual CoursesEntity Courses { get; set; }

        public string CourseCode { get; set; }

        // Config Access

        public virtual ICollection<ActivityLogsEntity> ActivityLogs { get; set; }
        public virtual ICollection<RequestsEntity> RequestCertificates { get ; set; }
        public virtual ICollection<StudentSubjectEnrollmentEntity> StudentSubjectEnrollmentEntities { get; set; }
    }
}
