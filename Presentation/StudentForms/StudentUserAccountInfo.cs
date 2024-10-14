using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Services;

namespace UnifiedEducationManagementSystem.Presentation.StudentForms
{
    public partial class StudentUserAccountInfo : Form
    {
        private readonly IActivityLogsRepository activityLogsRepository;
        private readonly ActivityLogsService activityLogsService;
        private readonly StudentDashboardForm studentDashboardForm;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed studentManagementAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private StudentsEntity loggedInStudent;

        public StudentUserAccountInfo
            (
            ActivityLogsService activityLogsService,
            IActivityLogsRepository activityLogsRepository,
            StudentDashboardForm studentDashboardForm
            )
        {
            InitializeComponent();
            this.activityLogsService = activityLogsService;
            this.activityLogsRepository = activityLogsRepository;
            this.studentDashboardForm = studentDashboardForm;
            homeAnimation = new AnimationCollapsed(10, HomeContainer);
            accountAnimation = new AnimationCollapsed(10, AccountContainer);
            requestAnimation = new AnimationCollapsed(10, RequestContainer);
            studentManagementAnimation = new AnimationCollapsed(10, StudentManagementContainer);
            resetAnimation = new AnimationCollapsed
                (10, HomeContainer, RequestContainer,
                StudentManagementContainer, AccountContainer
                );
        }

        public void SetLoggedInStudent(StudentsEntity studentsEntity)
        {
            loggedInStudent = studentsEntity;
            DisplayStudentProfile(loggedInStudent);
        }


        public void DisplayStudentProfile(StudentsEntity studentsEntity)
        {
            TBStudentId.Text = studentsEntity.StudentId.ToString();
            TBFullName.Text = studentsEntity.FullName;
            TBEmail.Text = studentsEntity.Email;
            TBContactNumber.Text = studentsEntity.Contact.ToString();
        }

        private void BTNChnagePassword_Click(object sender, EventArgs e)
        {
            var forgotPasswordForm = Program.ServiceProvider.GetRequiredService<ForgotPasswordForm>();
            forgotPasswordForm.SetLoggedInStudent(loggedInStudent);
            forgotPasswordForm.Show();
        }

        private async void BTNStudentLogout_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Logged Out", "Student Logged Out");

            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            this.Hide();
        }


        private async void BTNCertificates_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Requested/Viewed Request", "Student Requested a Cetificate");

            var studentCertificateForm = Program.ServiceProvider.GetRequiredService<StudentRequestForm>();
            studentCertificateForm.SetLoggedInStudent(loggedInStudent);
            studentCertificateForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }


        private async void BTNDashboard_Click(object sender, EventArgs e)
        {
            await studentDashboardForm.LoadActivityLogs();
            var studentDashboard = Program.ServiceProvider.GetRequiredService<StudentDashboardForm>();
            resetAnimation.Reset();
            studentDashboard.Show();
            this.Hide();
        }

        private async void BTNStudentProfile_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Viewed Profile", "Student Viewed Profile");

            var studentProfile = Program.ServiceProvider.GetRequiredService<StudentPorfileForm>();
            studentProfile.SetLoggedInStudent(loggedInStudent);
            studentProfile.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNAnimationHome_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationStudentManagement_Click(object sender, EventArgs e)
        {
            studentManagementAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationAccount_Click(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationRequest_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
        }
    }
}
