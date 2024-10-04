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
using UnifiedEducationManagementSystem.Helpers;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.StudentForms;
using UnifiedEducationManagementSystem.Repositories;

namespace UnifiedEducationManagementSystem.Presentation.SuperAdminForms
{
    public partial class LoginForm : Form
    {

        private readonly ICreateUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IStudentRepository istudentRepository;
        private readonly ForgotPasswordForm forgotPassword;

        public LoginForm
            (
            ICreateUserRepository userRepository, 
            IRoleRepository roleRepository,
            IStudentRepository studentRepository,
            ICourseRepository courseRepository, 
            IStudentRepository istudentRepository,
            ForgotPasswordForm forgotPassword

            )

        {
            InitializeComponent();
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.forgotPassword = forgotPassword;
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.istudentRepository = istudentRepository;
        }


        private async void BTNLogin_Click_1(object sender, EventArgs e)
        {
            {
                string email = TXTEmail.Text;
                string password = TXTPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Email and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var user = await userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    MessageBox.Show("No user found with this email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!EncryptionHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    MessageBox.Show("Invalid Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var userRoles = user.UserRoleEntity?.Select(ur => ur.Role.RoleName).ToList() ?? new List<string>();
                if (userRoles.Contains("Admin"))
                {
                    var adminDashboardForm = Program.ServiceProvider.GetRequiredService<AdminDashboardForm>();
                    adminDashboardForm.SetLoggedInAdmin(user);
                    adminDashboardForm.Show();
                }
                else
                {
                    var users = await userRepository.GetUserByEmailAsync(email);
                    if (users == null)
                    {
                        MessageBox.Show("No user found with this email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var student = await studentRepository.GetStudentByEmailAsync(email);
                    if (student == null)
                    {
                        MessageBox.Show("No student record found for this user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var studentDashboardForm = Program.ServiceProvider.GetRequiredService<StudentDashboardForm>();
                    studentDashboardForm.SetLoggedIsStudent(student);
                    studentDashboardForm.Show();

                }
                this.Hide();
                ClearControls.Clear(this);
            }
        }

        private void LNKForgotPassword_LinkClicked(object sender, EventArgs e)
        {
            var forgotPassword = Program.ServiceProvider.GetRequiredService<ForgotPasswordForm>();
            forgotPassword.Show();
        }

        private void CBShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TXTPassword.PasswordChar = CBShowPassword.Checked ? '\0' : '*';
        }

        private void ApplicationClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
