using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class UserRoleEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}
