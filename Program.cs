using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing.Text;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces.Services;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Services;
using UnifiedEducationManagementSystem.Helpers;
using UnifiedEducationManagementSystem.Presentation.AdminForms;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.StudentForms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Repositories;
using UnifiedEducationManagementSystem.Services;

static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var services = new ServiceCollection();

        services.AddDbContext<UEMPDbContext>(options =>
            options.UseSqlServer(
                "Database-Connection-String",
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }));

        // Repositories
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IActivityLogsRepository, ActivityLogsRepository>();
        services.AddScoped<IAssignSubjectRepository, AssignSubjectRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICreateUserRepository, CreateUserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Entities
        services.AddScoped<StudentsEntity>();

        // Services
        services.AddScoped<ActivityLogsService>();
        services.AddScoped<RoleManagementService>();
        services.AddScoped<UsersForgotPasswordService>();

        // Admin Forms
        services.AddScoped<LoginForm>();
        services.AddScoped<AdminDashboardForm>();
        services.AddScoped<AdminEnrollmentForm>();
        services.AddScoped<AdminAssignStudentSubjectForm>();
        services.AddScoped<ForgotPasswordForm>();
        services.AddScoped<AdminCreateAccountForm>();
        services.AddScoped<EnrollStudentForm>();
        services.AddScoped<AdminRequestForm>();
        services.AddScoped<AdminStudentActivityLogs>();

        // Student Forms
        services.AddScoped<StudentDashboardForm>();
        services.AddScoped<StudentPorfileForm>();
        services.AddScoped<StudentRequestForm>();
        services.AddScoped<StudentUserAccountInfo>();


        ServiceProvider = services.BuildServiceProvider();

        Application.Run(ServiceProvider.GetRequiredService<LoginForm>());
    }
}


