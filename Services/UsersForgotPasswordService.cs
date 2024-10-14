using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Helpers;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces.Services
{
    public class UsersForgotPasswordService
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersForgotPasswordService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<bool> RequestPasswordResetAsync(string lastName, string firstName, string middleName, string email, string newPassword)
        {

            var user = await unitOfWork.createUserRepository.GetByDetailsAsync(lastName, firstName, middleName, email);
            if (user == null)
            {
                return false;
            }

            var (newPasswordHash, newPasswordSalt) = EncryptionHelper.CreateHashAndSalt(newPassword);

            user.PasswordHash = newPasswordHash;
            user.PasswordSalt = newPasswordSalt;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await unitOfWork.createUserRepository.UpdateAsync(user);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
