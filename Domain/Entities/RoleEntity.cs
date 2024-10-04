using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Domain.Entities
{
    public class RoleEntity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public ICollection <UserRoleEntity> UserRoles { get; set; }
    }
}
