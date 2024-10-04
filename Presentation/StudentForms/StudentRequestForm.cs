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

namespace UnifiedEducationManagementSystem.Presentation.StudentForms
{
    public partial class StudentRequestForm : Form
    {
        private readonly IRequestRepository certificateRepository;
        private readonly StudentDashboardForm studentDashboardForm;
        private readonly AnimationCollapsed homeAnimation;
        private StudentsEntity loggedInStudent;


        public StudentRequestForm
            (
            IRequestRepository certificateRepository,
            StudentDashboardForm studentDashboardForm,
            StudentsEntity loggedIdStudent
            )


        {
            InitializeComponent();
            this.certificateRepository = certificateRepository;
            this.studentDashboardForm = studentDashboardForm;
            this.loggedInStudent = loggedIdStudent;
            homeAnimation = new AnimationCollapsed(10, HomeContainer);
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
            MessageBox.Show("Request Submitted Successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearControls.Clear(this);

        }

        private void BTNDashBoard_Click(object sender, EventArgs e)
        {
            var studentDashBoard = Program.ServiceProvider.GetRequiredService<StudentDashboardForm>();
            studentDashBoard.Show();
            this.Hide();
        }

        private void BTNHome_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
        }

        private void BTNStudentLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            this.Hide();
        }

        private void BTNLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            this.Hide();
        }
    }
}
