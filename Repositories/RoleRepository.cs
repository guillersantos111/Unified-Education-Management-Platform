using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UEMPDbContext _applicationDBContext;

        public RoleRepository (UEMPDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }


        public async Task <RoleEntity> GetRoleByNameAsync(string roleName)
        {
            return await _applicationDBContext.Roles.SingleOrDefaultAsync(r => r.RoleName == roleName);
        }


        public void AddRole(RoleEntity role)
        {
            _applicationDBContext.Roles.Add(role);
            _applicationDBContext.SaveChanges();
        }
    }
}
