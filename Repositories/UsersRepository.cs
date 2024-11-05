using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Domain.Services;
using UnifiedEducationManagementSystem.Helpers;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class CreateUserRepository : ICreateUserRepository
    {
        private readonly UEMPDbContext applicationDBContext;
        private readonly RoleManagementService roleManagementService;
        private readonly IRoleRepository roleRepository;

        public CreateUserRepository(UEMPDbContext applicationDBContext, RoleManagementService roleManagementService, IRoleRepository roleRepository)
        {
            this.applicationDBContext = applicationDBContext;
            this.roleRepository = roleRepository;
            this.roleManagementService = roleManagementService;
        }

        public async Task CreateUserAndAssignRoleAsync(UserDto userDto)
        {
            var errorMessages = new List<string>();

            if (userDto.Password != userDto.ConfirmPassword)
            {
                errorMessages.Add("Passwords do not match.");
            }

            if (errorMessages.Any())
            {
                MessageBox.Show(string.Join("\n", errorMessages), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var existingUser = await applicationDBContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                MessageBox.Show($"A user with the email '{userDto.Email}' already exists. Please use a different email.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var (passwordHash, passwordSalt) = EncryptionHelper.CreateHashAndSalt(userDto.Password);

            var user = new UserEntity
            {
                LastName = userDto.LastName,
                FirstName = userDto.FirstName,
                MiddleName = userDto.MiddleName,
                Gender = userDto.Gender,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
                BirthDate = userDto.BirthDate
            };

            applicationDBContext.Users.Add(user);
            await applicationDBContext.SaveChangesAsync();

            var role = await roleRepository.GetRoleByNameAsync(userDto.RoleName);
            if (role == null)
            {
                MessageBox.Show($"Role '{userDto.RoleName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await AssignRoleToUserAsync(user, role);

            var assignedRole = await applicationDBContext.UserRoles
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == user.UserId && ur.RoleId == role.RoleId);
        }

        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            return await applicationDBContext.Users
                .Include(u => u.UserRoleEntity)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await applicationDBContext.Users
                .Include(u => u.UserRoleEntity)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AssignRoleToUserAsync(UserEntity user, RoleEntity role)
        {
            var existingUserRole = await applicationDBContext.UserRoles
                .AnyAsync(ur => ur.UserId == user.UserId && ur.RoleId == role.RoleId);

            if (!existingUserRole)
            {
                var userRole = new UserRoleEntity
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId,
                };

                applicationDBContext.UserRoles.Add(userRole);
                await applicationDBContext.SaveChangesAsync();
            }
        }

        public async Task<UserEntity> GetByDetailsAsync(string lastName, string firstName, string middleName, string email)
        {
            return await applicationDBContext.Users
                .Where(u => u.LastName == lastName &&
                            u.FirstName == firstName &&
                            u.MiddleName == middleName &&
                            u.Email == email)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(UserEntity user)
        {
            applicationDBContext.Users.Update(user);
            await applicationDBContext.SaveChangesAsync();
        }
    }
}
