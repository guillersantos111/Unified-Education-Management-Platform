using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.StudentForms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Repositories;

namespace UnifiedEducationManagementSystem.Presentation.Forms
{
    public partial class StudentDashboardForm : Form
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICreateUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRequestRepository certificateRepository;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private StudentsEntity loggedInStudent;

        public StudentDashboardForm
            (
            IStudentRepository studentRepository, 
            ICreateUserRepository userRepository,
            IRoleRepository roleRepository,
            IRequestRepository certificateRepository
            )
        {
            InitializeComponent();
            this.studentRepository = studentRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.certificateRepository = certificateRepository;
            this.enrollmentAnimation = new AnimationCollapsed(10, EnrollmentContainer);
            this.accountAnimation = new AnimationCollapsed(10, AccountContainer);
            this.requestAnimation = new AnimationCollapsed(10, RequestContainer);
        }


        public void SetLoggedIsStudent(StudentsEntity students)
        {
            this.loggedInStudent = students;
            LBLStudentName.Text = $"{loggedInStudent.FirstName} {loggedInStudent.LastName}";
        }


        private void LBLStudentName_Click(object sender, EventArgs e)
        {
            var studentProfile = Program.ServiceProvider.GetRequiredService<StudentPorfileForm>();
            studentProfile.Show();
            this.Hide();
        }


        private void BTNRequestAnimation_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
        }

        private void BTNCertificates_Click(object sender, EventArgs e)
        {
            var studentCertificateForm = Program.ServiceProvider.GetRequiredService<StudentRequestForm>();
            studentCertificateForm.SetLoggedInStudent(loggedInStudent);
            studentCertificateForm.Show();
            this.Hide();
        }

        private void BTNStudentLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            this.Hide();
        }
    }
}
