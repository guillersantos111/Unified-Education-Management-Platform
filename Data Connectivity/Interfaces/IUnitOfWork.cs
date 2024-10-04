using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface IUnitOfWork
    {
        ICreateUserRepository createUserRepository { get; }
        Task SaveChangesAsync();
    }
}
