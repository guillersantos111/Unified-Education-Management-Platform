using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Linq;
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
using System.ComponentModel;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;

namespace UnifiedEducationManagementSystem.Presentation.AdminForms
{
    public partial class AdminAssignStudentSubjectForm : Form
    {
        private readonly IAssignSubjectRepository assignSubjectRepository;
        private readonly IStudentRepository studentRepository;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed studentAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed activitylogsAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private readonly UEMPDbContext uEMPDbContext;
        private BindingList<StudentSubjectEnrollmentEntity> enrollments; // Use BindingList for change notifications

        public AdminAssignStudentSubjectForm(IAssignSubjectRepository assignSubjectRepository, IStudentRepository studentRepository)
        {
            InitializeComponent();
            uEMPDbContext = new UEMPDbContext();
            this.assignSubjectRepository = assignSubjectRepository;
            this.studentRepository = studentRepository;
            this.homeAnimation = new AnimationCollapsed(10, HomeContainer);
            this.studentAnimation = new AnimationCollapsed(10, StudentContainer);
            this.enrollmentAnimation = new AnimationCollapsed(10, EnrollmentContainer);
            this.requestAnimation = new AnimationCollapsed(10, RequestContainer);
            this.activitylogsAnimation = new AnimationCollapsed(10, ActivityLogsContainer);
            this.accountAnimation = new AnimationCollapsed(10, AccountContainer);
            this.resetAnimation = new AnimationCollapsed(10, HomeContainer, EnrollmentContainer,
                RequestContainer, EnrollmentContainer,
                StudentContainer, AccountContainer,
                ActivityLogsContainer);

            DGVStudentsList.SelectionChanged += DGVStudentList_SelectionChanged;
            
        }

        public async Task LoadEnrolledStudentListAsync()
        {
            var studentList = await studentRepository.GetAllStudentsAsync();
            DGVStudentsList.DataSource = studentList;
        }

        public async Task LoadAssignedOrEnrolledStudentAsync()
        {
            var studentSubjectEnrollmentEntities = await assignSubjectRepository.GetAssignedOrEnrolledStudentsAsync();
            enrollments = new BindingList<StudentSubjectEnrollmentEntity>(studentSubjectEnrollmentEntities.ToList());
            DGVStudentSubjectEnrolled.DataSource = enrollments;
        }

        private async void AdminAssignStudentSubjectForm_Load(object sender, EventArgs e)
        {
            await LoadEnrolledStudentListAsync();
            await LoadAssignedOrEnrolledStudentAsync();
        }

        private async void DGVStudentList_SelectionChanged(object sender, EventArgs e)
        {
            if (DGVStudentsList.SelectedRows.Count > 0)
            {
                var selectedStudentRow = DGVStudentsList.SelectedRows[0];

                if (selectedStudentRow.Cells["StudentCourseId"].Value is int courseId &&
                    selectedStudentRow.Cells["StudentId"].Value is int studentId)
                {
                    string courseCode = Convert.ToString(selectedStudentRow.Cells["CourseCode"].Value);
                    await LoadSubjectsByCourseIdAsync(courseId);
                }
            }
        }

        private int GetSelectedStudentSubjectEnrollmentData()
        {
            if (DGVStudentSubjectEnrolled.SelectedRows.Count > 0)
            {
                var selectedRow = DGVStudentSubjectEnrolled.SelectedRows[0];

                if (selectedRow.Cells["AssignedStudentId"].Value != null &&
                    int.TryParse(selectedRow.Cells["AssignedStudentId"].Value.ToString(), out int studentId))
                {
                    // Check if the ID is valid (not zero or negative)
                    if (studentId > 0)
                    {
                        return studentId; // Return the valid ID
                    }
                }
            }

            MessageBox.Show("Please select a valid record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return -1; // Return -1 to indicate invalid selection
        }

        private async Task LoadSubjectsByCourseIdAsync(int courseId)
        {
            var subjects = await assignSubjectRepository.GetSubjectsByCourseIdAsync(courseId);
            DGVSubjectsList.DataSource = subjects.ToList();
        }

        private async void BTNEnrollSubjects_Click(object sender, EventArgs e)
        {
            if (DGVStudentsList.CurrentRow != null && DGVSubjectsList.CurrentRow != null)
            {
                // Access and Retrieve the Selected Value
                int studentId = (int)DGVStudentsList.CurrentRow.Cells["StudentId"].Value;
                int subjectId = (int)DGVSubjectsList.CurrentRow.Cells["SubjectId"].Value;
                int courseId = (int)DGVStudentsList.CurrentRow.Cells["StudentCourseId"].Value;

                // Call the Method and Get the Return Value
                bool isAssigned = await assignSubjectRepository.AssignSubjectToStudentAsync(studentId, subjectId, courseId);

                if (isAssigned)
                {
                    MessageBox.Show("Subject Assigned Successfully!", "Success Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadAssignedOrEnrolledStudentAsync();
                }
            }
        }

        private async void BTNRemoveAssignedStudent_Click(object sender, EventArgs e)
        {
            int studentId = GetSelectedStudentSubjectEnrollmentData();

            if (studentId == -1) return;

            var confirmation = MessageBox.Show("Are you sure you want to delete this student subject enrollment?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    bool success = await assignSubjectRepository.RemoveAssignedSubjectToStudentAsync(studentId);

                    if (success)
                    {
                        // Remove from the BindingList
                        var enrollmentToRemove = enrollments.FirstOrDefault(sse => sse.StudentId == studentId);
                        if (enrollmentToRemove != null)
                        {
                            enrollments.Remove(enrollmentToRemove);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to remove student subject enrollment. No entry found with the specified ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while removing the enrollment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BTNHome_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNStudentManagement_Click(object sender, EventArgs e)
        {
            studentAnimation.Toggle();
            resetAnimation.Reset();
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

        private void BTNActivityLogs_Click(object sender, EventArgs e)
        {
            activitylogsAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNRequest_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
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
            var createAccountForm = Program.ServiceProvider.GetRequiredService<AdminCreateAccountForm>();
            createAccountForm.Show();
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

        private void BTNDashboard_Click(object sender, EventArgs e)
        {
            var adminDashboardForm = Program.ServiceProvider.GetRequiredService<AdminDashboardForm>();
            adminDashboardForm.Show();
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

        private void BTNLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }
    }
}
