using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate {  get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        public ICollection<UserRoleEntity> UserRoleEntity { get; set; }

    }
}
