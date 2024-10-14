using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface ICreateUserRepository
    {
        Task <UserEntity> GetUserByIdAsync(int userId);
        Task <UserEntity> GetUserByEmailAsync(string email);

        Task UpdateAsync(UserEntity user);

        Task AssignRoleToUserAsync(UserEntity user, RoleEntity role);

        Task CreateUserAndAssignRoleAsync(UserDto userDto);

        Task<UserEntity> GetByDetailsAsync(string lastName, string firstName, string middleName, string email);
    }
}
