using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Presentation.AdminForms;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;

namespace UnifiedEducationManagementSystem.Presentation.Forms
{
    public partial class AdminDashboardForm : Form
    {
        private readonly ICreateUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly AdminRequestForm adminRequestForm;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed studentAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed activitylogsAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private UserEntity loggedInAdmin;

        public AdminDashboardForm(ICreateUserRepository userRepository, IRoleRepository roleRepository,
            IStudentRepository studentRepository, ICourseRepository courseRepository, AdminRequestForm adminRequestForm)
        {
            InitializeComponent();
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.homeAnimation = new AnimationCollapsed(10, HomeContainer);
            this.studentAnimation = new AnimationCollapsed(10, StudentContainer);
            this.enrollmentAnimation = new AnimationCollapsed(10, EnrollmentContainer);
            this.requestAnimation = new AnimationCollapsed(10, RequestContainer);
            this.activitylogsAnimation = new AnimationCollapsed(10, ActivityLogsContainer);
            this.accountAnimation = new AnimationCollapsed(10, AccountContainer);
            this.resetAnimation = new AnimationCollapsed
                (10, HomeContainer, EnrollmentContainer, 
                RequestContainer, EnrollmentContainer, 
                StudentContainer, AccountContainer, 
                ActivityLogsContainer
                );

            this.adminRequestForm = adminRequestForm;
        }

        public void SetLoggedInAdmin(UserEntity admin)
        {
            loggedInAdmin = admin;
            LBLAdminName.Text = $"{loggedInAdmin.FirstName} {loggedInAdmin.LastName}";
        }


        private void BTNLogout_Click_1(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNCreateAccout_Click_1(object sender, EventArgs e)
        {
            var createAccountForm = Program.ServiceProvider.GetRequiredService<AdminCreateAccountForm>();
            createAccountForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private async void BTNCertificates_Click(object sender, EventArgs e)
        {
            await adminRequestForm.LoadRequestsAsync();
            var AdminrequestForm = Program.ServiceProvider.GetRequiredService<AdminRequestForm>();
            AdminrequestForm.SetLoggedInAdmin(loggedInAdmin);
            AdminrequestForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNEnrollment_Click_1(object sender, EventArgs e)
        {
            var adminEnrollmentForm = Program.ServiceProvider.GetRequiredService<AdminEnrollmentForm>();
            adminEnrollmentForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNHome_Click_1(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationAccount_Click_1(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNStudentManagement_Click_1(object sender, EventArgs e)
        {
            studentAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationEnrollments_Click_1(object sender, EventArgs e)
        {
            enrollmentAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNActivityLogs_Click_1(object sender, EventArgs e)
        {
            activitylogsAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNRequest_Click_1(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
        }

        private async void BTNAssign_Click(object sender, EventArgs e)
        {
            var adminAssignSubjects = Program.ServiceProvider.GetRequiredService<AdminAssignStudentSubjectForm>();
            adminAssignSubjects.Show();
            await adminAssignSubjects.LoadEnrolledStudentListAsync();
            await adminAssignSubjects.LoadAssignedOrEnrolledStudentAsync();
            resetAnimation.Reset();
            this.Hide();
        }
    }
}
