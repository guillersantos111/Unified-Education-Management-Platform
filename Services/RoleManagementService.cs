using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Repositories;

namespace UnifiedEducationManagementSystem.Domain.Services
{
    public class RoleManagementService
    {
        private readonly UEMPDbContext _applicationDBContext;

        public RoleManagementService(UEMPDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public void SaveRole(UserEntity user, string roleName)
        {
            var role = _applicationDBContext.Roles
                .SingleOrDefault(r => r.RoleName == roleName);

            if (role == null)
            {
                MessageBox.Show($"Role '{roleName}' does not exist.");
                return;
            }
            var userRoleEntity = new UserRoleEntity
            {
                UserId = user.UserId,
                RoleId = role.RoleId,
            };
            _applicationDBContext.UserRoles.Add(userRoleEntity);
            _applicationDBContext.SaveChanges();
        }
    }
}