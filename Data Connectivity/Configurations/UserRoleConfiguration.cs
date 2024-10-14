using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;


namespace UnifiedEducationManagementSystem.Data_Connectivity.Data
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {

        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId});

            builder.HasOne(ur => ur.User)
                   .WithMany(u => u.UserRoleEntity)
                   .HasForeignKey(ur => ur.UserId);

            builder.HasOne(ur => ur.Role)
                   .WithMany(r => r.UserRoles)
                   .HasForeignKey(ur => ur.RoleId);
        }
    }
}
