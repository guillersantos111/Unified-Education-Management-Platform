using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Helpers;
using UnifiedEducationManagementSystem.Presentation.AdminForms;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;

namespace UnifiedEducationManagementSystem.Presentation.Forms
{
    public partial class AdminCreateAccountForm : Form
    {
        private readonly ICreateUserRepository createUserRepository;
        private readonly IRoleRepository roleRepository;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private readonly AnimationCollapsed activityLogAnimation;
        private readonly AnimationCollapsed studentManagementAnimation;

        public AdminCreateAccountForm(ICreateUserRepository createUserRepository, IRoleRepository roleRepository)
        {
            InitializeComponent();
            this.createUserRepository = createUserRepository;
            this.roleRepository = roleRepository;
            this.homeAnimation = new AnimationCollapsed(10, HomeContainer);
            this.accountAnimation = new AnimationCollapsed(10, AccountContainer);
            this.enrollmentAnimation = new AnimationCollapsed(10, EnrollmentContainer);
            this.requestAnimation = new AnimationCollapsed(10, RequestContainer);
            this.activityLogAnimation = new AnimationCollapsed(10, ActivityLogsContainer);
            this.studentManagementAnimation = new AnimationCollapsed(10, StudentContainer);
            this.resetAnimation = new AnimationCollapsed
                (10, HomeContainer, AccountContainer, 
                EnrollmentContainer, RequestContainer, 
                ActivityLogsContainer, StudentContainer
                );
        }

        private async void BTNCreateAccount(object sender, EventArgs e)
        {
            string selectedRole = CBRoles.SelectedItem?.ToString();
            string selectedgender = CBGender.SelectedItem?.ToString(); ;
            DateTime birthDate = DTPBirthDate.Value;
            string lastName = TXTLastName.Text;
            string firstName = TXTFirstName.Text;
            string middleName = TXTMiddleName.Text;
            string email = TXTEmail.Text;
            string password = TXTPassword.Text;
            string confirmPassword = TXTConfirmPassword.Text;

            if (string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(middleName) ||
                string.IsNullOrEmpty(selectedgender) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) ||
                string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var createUserDto = new UserDto
            {
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                RoleName = selectedRole,
                Gender = selectedgender,
                BirthDate = birthDate,
            };

                await createUserRepository.CreateUserAndAssignRoleAsync(createUserDto);
                MessageBox.Show("User created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearControls.Clear(this);
        }

        private void CBShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TXTPassword.PasswordChar = CBShowPassword.Checked ? '\0' : '*';
            TXTConfirmPassword.PasswordChar = CBShowPassword.Checked ? '\0' : '*';
        }


        private void BTNCertificates_Click(object sender, EventArgs e)
        {
            var adminRequestForm = Program.ServiceProvider.GetRequiredService<AdminRequestForm>();
            adminRequestForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNCreateAccout_Click(object sender, EventArgs e)
        {
            var adminCreateAccountForm = Program.ServiceProvider.GetRequiredService<AdminCreateAccountForm>();
            adminCreateAccountForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNEnrollment_Click(object sender, EventArgs e)
        {
            var adminEnrollmentForm = Program.ServiceProvider.GetRequiredService<AdminEnrollmentForm>();
            adminEnrollmentForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNLogout_Click_1(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNDashboard_Click(object sender, EventArgs e)
        {
            var adminDashboardForm = Program.ServiceProvider.GetRequiredService<AdminDashboardForm>();
            adminDashboardForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNAnimationAccount_Click(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationEnrollments_Click(object sender, EventArgs e)
        {
            enrollmentAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationHome_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationActivityLogs_Click(object sender, EventArgs e)
        {
            activityLogAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationStudentManagement_Click(object sender, EventArgs e)
        {
            studentManagementAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationRequest_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAssignSubject_Click(object sender, EventArgs e)
        {
            var adminAssignStudentSubjectForm = Program.ServiceProvider.GetRequiredService<AdminAssignStudentSubjectForm>();
            adminAssignStudentSubjectForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNStudentActivityLogs_Click(object sender, EventArgs e)
        {
            var adminStudentActivityLogs = Program.ServiceProvider.GetRequiredService<AdminStudentActivityLogs>();
            adminStudentActivityLogs.Show();
            resetAnimation.Reset();
            this.Hide();
        }
    }
}
