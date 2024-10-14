using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Services;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UEMPDbContext _uempDbContext;
        private readonly IRoleRepository _roleRepository;
        private readonly RoleManagementService _roleManagementService;
        private ICreateUserRepository _createUserRepository;

        // Constructor initializes the dependencies for the UnitOfWork
        public UnitOfWork(UEMPDbContext uempDbContext, IRoleRepository roleRepository, RoleManagementService roleManagementService)
        {
            _uempDbContext = uempDbContext;
            _roleRepository = roleRepository;
            _roleManagementService = roleManagementService;
        }

        // Lazy-loaded UserRepository to ensure it's instantiated only when needed
        public ICreateUserRepository createUserRepository
        {
            get
            {
                if (_createUserRepository == null)
                {
                    _createUserRepository = new CreateUserRepository(
                        _uempDbContext,
                        _roleManagementService,
                        _roleRepository);
                }
                return _createUserRepository;
            }
        }

        // Saves changes to the database asynchronously
        public async Task SaveChangesAsync()
        {
            await _uempDbContext.SaveChangesAsync();
        }
    }
}
