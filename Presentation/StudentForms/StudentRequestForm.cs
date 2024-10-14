using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Helpers;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Repositories;
using UnifiedEducationManagementSystem.Services;

namespace UnifiedEducationManagementSystem.Presentation.StudentForms
{
    public partial class StudentRequestForm : Form
    {
        private readonly IActivityLogsRepository activityLogsRepository;
        private readonly ActivityLogsService activityLogsService;
        private readonly IRequestRepository certificateRepository;
        private readonly StudentDashboardForm studentDashboardForm;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed studentManagementAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private StudentsEntity loggedInStudent;


        public StudentRequestForm
            (
            ActivityLogsService activityLogsService,
            IActivityLogsRepository activityLogsRepository,
            IRequestRepository certificateRepository,
            StudentDashboardForm studentDashboardForm,
            StudentsEntity loggedIdStudent
            )


        {
            InitializeComponent();
            this.activityLogsService = activityLogsService;
            this.activityLogsRepository = activityLogsRepository;
            this.certificateRepository = certificateRepository;
            this.studentDashboardForm = studentDashboardForm;
            this.loggedInStudent = loggedIdStudent;
            homeAnimation = new AnimationCollapsed(10, HomeContainer);
            accountAnimation = new AnimationCollapsed(10, AccountContainer);
            requestAnimation = new AnimationCollapsed(10, RequestContainer);
            studentManagementAnimation = new AnimationCollapsed(10, StudentContainer);
            resetAnimation = new AnimationCollapsed
                (10, HomeContainer, RequestContainer,
                StudentContainer, AccountContainer
                );
        }

        public void SetLoggedInStudent(StudentsEntity students)
        {
            this.loggedInStudent = students;
            DisplayStudentName();
        }

        private void DisplayStudentName()
        {
            LBLStudentName.Text = $"{loggedInStudent.FirstName} {loggedInStudent.LastName}";
        }


        private async void BTNSubmit_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(CBCertificateType.SelectedItem?.ToString()) ||
                string.IsNullOrEmpty(DTPRequestDate.ToString()) ||
                string.IsNullOrEmpty(TBPurpose.Text) ||
                string.IsNullOrEmpty(TBAdditionalComments.Text))
            {
                MessageBox.Show("All Fields are Required");
                return;
            }

            var request = new RequestsEntity()
            {
                StudentId = loggedInStudent.StudentId,
                CertificateType = CBCertificateType.SelectedItem.ToString(),
                Purpose = TBPurpose.Text,
                AdditionalComments = TBAdditionalComments.Text,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            await certificateRepository.AddRequestAsync(request);
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Requested a Certificate", "Student Requested a Certificate");
            MessageBox.Show("Request Submitted Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearControls.Clear(this);

        }


        private void BTNStudentLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            this.Hide();
        }

        private async void BTNLogout_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Logged Out", "Student Logged Out");

            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
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

        private async void BTNDashboard_Click_1(object sender, EventArgs e)
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

        private async void BTNCreateAccout_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Requested/Viewed Request", "Student Requested a Cetificate");

            var studentAccountInfo = Program.ServiceProvider.GetRequiredService<StudentUserAccountInfo>();
            studentAccountInfo.SetLoggedInStudent(loggedInStudent);
            studentAccountInfo.Show();
            resetAnimation.Reset();
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
    }
}
