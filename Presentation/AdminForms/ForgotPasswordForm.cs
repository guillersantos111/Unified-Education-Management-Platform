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
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces.Services;
using UnifiedEducationManagementSystem.Helpers;

namespace UnifiedEducationManagementSystem.Presentation.StudentForms
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly UsersForgotPasswordService _userService;
        public ForgotPasswordForm(UsersForgotPasswordService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

        private async void BTNRestPassword_Click(object sender, EventArgs e)
        {
            string lastName = TXTLastName.Text;
            string firstNane = TXTFirstName.Text;
            string middleName = TXTMiddleName.Text;
            string email = TXTEmail.Text;
            string newPassword = TXTPassword.Text;
            string confirmPassword = TXTConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstNane) ||
                string.IsNullOrWhiteSpace(middleName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool result = await _userService.RequestPasswordResetAsync(lastName, firstNane, middleName, email, newPassword);

                if (result)
                {
                    MessageBox.Show("Your password has been reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The provided information is incorrect. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ClearControls.Clear(this);
        }

        private void CBShowPassword_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BTNExit_Click(object sender, EventArgs e)
        {
            this.Hide();

        }
    }
}
