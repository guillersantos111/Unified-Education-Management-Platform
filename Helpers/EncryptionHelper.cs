using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UnifiedEducationManagementSystem.Helpers
{
    public static class EncryptionHelper
    {
        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int IterationCount = 10000;

        public static (byte[] Hash, byte[] Salt) CreateHashAndSalt (string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var RNG = RandomNumberGenerator.Create())
            {
                RNG.GetBytes(salt);
            }


            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: KeySize
                );

            return (hash, salt);
        }

        public static bool VerifyPassword(string enteredPassword, byte[] storedHash, byte[] storedSalt)
        {
            byte[] hash = KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: IterationCount,
                numBytesRequested: KeySize
                );

            return hash.SequenceEqual(storedHash);
        }
    }
}
