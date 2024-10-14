using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Presentation.AdminForms;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.Forms;

namespace UnifiedEducationManagementSystem.Presentation.SuperAdminForms
{
    public partial class AdminEnrollmentForm : Form
    {

        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private readonly AnimationCollapsed studentAnimation;
        private readonly AnimationCollapsed activityLogsAnimation;

        public AdminEnrollmentForm(IStudentRepository studentRepository, ICourseRepository courseRepository)
        {
            InitializeComponent();
            homeAnimation = new AnimationCollapsed(10, HomeContainer);
            requestAnimation = new AnimationCollapsed(10, RequestContainer);
            accountAnimation = new AnimationCollapsed(10, AccountContainer);
            enrollmentAnimation = new AnimationCollapsed(10, EnrollmentContainer);
            studentAnimation = new AnimationCollapsed(10, StudentContainer);
            activityLogsAnimation = new AnimationCollapsed(10, ActivityLogsContainer);
            resetAnimation = new AnimationCollapsed
                (10, HomeContainer, RequestContainer, 
                AccountContainer, EnrollmentContainer,
                StudentContainer, ActivityLogsContainer
                );

            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            InitializeAsync();
        }


        private async void InitializeAsync()
        {
            await LoadAllEnrolledStudentAsync();
        }


        private async void LoadNewlyEnrolledStudent(object sender, StudentsEntity student)
        {
            await LoadAllEnrolledStudentAsync();
        }


        private async Task LoadAllEnrolledStudentAsync()
        {
            var students = await studentRepository.GetAllStudentsAsync();

            if (students == null || !students.Any())
            {
                MessageBox.Show("No students available.");
                return;
            }

            LVEnrolledStudents.Items.Clear();

            foreach (var student in students)
            {
                var item = new ListViewItem(student.StudentId.ToString())
                {
                    Tag = student.StudentId,
                    SubItems =
                    {
                        student.LastName,
                        student.FirstName,
                        student.MiddleName,
                        student.Gender,
                        student.BirthDate.ToShortDateString(),
                        student.Address,
                        student.Contact.ToString(),
                        student.Campus,
                        student.Courses?.Course ?? "",
                        student.Courses?.CourseCode ?? ""
                    }
                };

                LVEnrolledStudents.Items.Add(item);
            }
            LVEnrolledStudents.Refresh();
        }


        private int GetSelectedStudentId()
        {
            if (LVEnrolledStudents.SelectedItems.Count > 0)
            {
                var selectedItem = LVEnrolledStudents.SelectedItems[0];
                if (int.TryParse(selectedItem.Text, out int studentId))
                {
                    return studentId;
                }
                else
                {
                    MessageBox.Show("The selected student's ID is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            else
            {
                MessageBox.Show("No student is selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }


        private void BTNEnroll_Click(object sender, EventArgs e)
        {
            var enrollForm = Program.ServiceProvider.GetRequiredService<EnrollStudentForm>();
            enrollForm.StudentEnrolled -= LoadNewlyEnrolledStudent;
            enrollForm.StudentEnrolled += LoadNewlyEnrolledStudent;
            enrollForm.Show();
        }


        private async void BTNEdit_Click(object sender, EventArgs e)
        {
            var studentId = GetSelectedStudentId();
            if (studentId != -1)
            {
                var student = await studentRepository.GetStudentByIdAsync(studentId);
                if (student != null)
                {
                    var enrollForm = Program.ServiceProvider.GetRequiredService<EnrollStudentForm>();
                    enrollForm.StudentId = studentId;
                    enrollForm.StudentEnrolled += LoadNewlyEnrolledStudent;

                    await enrollForm.PopulateStudentDetailsAsync();
                    enrollForm.Show();
                }
            }
        }


        private async void BTNRemove_Click(object sender, EventArgs e)
        {
            var studentId = GetSelectedStudentId();

            if (studentId == -1) return;

            var confirmation = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                    bool success = await studentRepository.DeleteStudentAsync(studentId);

                    if (success)
                    {
                        LVEnrolledStudents.Items.Clear();
                        await LoadAllEnrolledStudentAsync();
                    }
                    else
                    {
                        MessageBox.Show("Student not found or could not be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }


        private void BTNDashboard_Click_1(object sender, EventArgs e)
        {
            var adminDashBoardForm = Program.ServiceProvider.GetRequiredService<AdminDashboardForm>();
            adminDashBoardForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }


        private void BTNCreateAccout_Click(object sender, EventArgs e)
        {
            var adminCreateAccount = Program.ServiceProvider.GetRequiredService<AdminCreateAccountForm>();
            adminCreateAccount.Show();
            resetAnimation.Reset();
            this.Hide();
        }


        private void BTNCertificates_Click(object sender, EventArgs e)
        {
            var adminRequestForm = Program.ServiceProvider.GetRequiredService<AdminRequestForm>();
            adminRequestForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }


        private void BTNEnrollmentLogout_Click(object sender, EventArgs e)
        {
            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }


        private void BTNHomeAnimation_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }


        private void BTNAccountAnimation_Click(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
            resetAnimation.Reset();
        }


        private void BTNEnrollmentsAnimation_Click(object sender, EventArgs e)
        {
            enrollmentAnimation.Toggle();
            resetAnimation.Reset();
        }


        private void BTNRequestAnimation_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAssignSubjects_Click(object sender, EventArgs e)
        {
            var adminAssignSubjectForm = Program.ServiceProvider.GetRequiredService<AdminAssignStudentSubjectForm>();
            adminAssignSubjectForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNStudentManagementAnimation_Click(object sender, EventArgs e)
        {
            studentAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNActivityLogsAnimation_Click(object sender, EventArgs e)
        {
            activityLogsAnimation.Toggle();
            resetAnimation.Reset();
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
