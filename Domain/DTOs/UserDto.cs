using System;
using System.ComponentModel.DataAnnotations;

namespace UnifiedEducationManagementSystem.Domain
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public string Password { get ; set; }

        public string ConfirmPassword { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string RoleName { get; set; }
    }
}
