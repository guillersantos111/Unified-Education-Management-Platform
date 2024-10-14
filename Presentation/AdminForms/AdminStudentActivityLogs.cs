using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Services;

namespace UnifiedEducationManagementSystem.Presentation.AdminForms
{
    public partial class AdminStudentActivityLogs : Form
    {
        private readonly IActivityLogsRepository activityLogsRepository;
        private readonly ActivityLogsService activityLogsService;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed studentAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed activitylogsAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed resetAnimation;

        public AdminStudentActivityLogs
            (
            ActivityLogsService activityLogsService,
            IActivityLogsRepository activityLogsRepository
            )
        {
            InitializeComponent();
            this.activityLogsService = activityLogsService;
            this.activityLogsRepository = activityLogsRepository;
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
        }

        public async Task LoadActivityLogs()
        {
            var logs = await activityLogsRepository.GetStudentActivityLogsInAdminSide();
            DGVStudentActivityLogs.DataSource = logs;
        }

        private async void AdminStudentActivityLogs_Load(object sender, EventArgs e)
        {
            await LoadActivityLogs();
        }

        private void BTNCertificates_Click(object sender, EventArgs e)
        {
            var adminrequestForm = Program.ServiceProvider.GetRequiredService<AdminRequestForm>();
            adminrequestForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNRequest_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNEnrollment_Click(object sender, EventArgs e)
        {
            var adminEnrollmentForm = Program.ServiceProvider.GetRequiredService<AdminEnrollmentForm>();
            adminEnrollmentForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNAnimationEnrollments_Click(object sender, EventArgs e)
        {
            enrollmentAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNCreateAccout_Click(object sender, EventArgs e)
        {
            var createAccountForm = Program.ServiceProvider.GetRequiredService<AdminCreateAccountForm>();
            createAccountForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNAnimationAccount_Click(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
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

        private void BTNStudentManagement_Click(object sender, EventArgs e)
        {
            studentAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNDashboard_Click(object sender, EventArgs e)
        {
            var adminDashBoardForm = Program.ServiceProvider.GetRequiredService<AdminDashboardForm>();
            adminDashBoardForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNHome_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }
    }
}
