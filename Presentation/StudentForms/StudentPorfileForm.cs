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
using UnifiedEducationManagementSystem.Presentation.Animation_and_Effects;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Repositories;
using UnifiedEducationManagementSystem.Services;

namespace UnifiedEducationManagementSystem.Presentation.StudentForms
{
    public partial class StudentPorfileForm : Form
    {
        private readonly IActivityLogsRepository activityLogsRepository;
        private readonly ActivityLogsService activityLogsService;
        private readonly IAssignSubjectRepository assignSubjectRepository;
        private readonly IStudentRepository studentRepository;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed studentAnimation;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed activitylogsAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed resetAnimation;

        private StudentsEntity loggedInStudent;
        public StudentPorfileForm
            (
            IAssignSubjectRepository assignSubjectRepository,
            IStudentRepository studentRepository,
            ActivityLogsService activityLogsService,
            IActivityLogsRepository activityLogsRepository
            )
        {
            InitializeComponent();
            this.activityLogsService = activityLogsService;
            this.activityLogsRepository = activityLogsRepository;
            this.assignSubjectRepository = assignSubjectRepository;
            this.studentRepository = studentRepository;
            this.homeAnimation = new AnimationCollapsed(10, HomeContainer);
            this.accountAnimation = new AnimationCollapsed(10, AccountContainer);
            this.requestAnimation = new AnimationCollapsed(10, RequestContainer);
            this.studentAnimation = new AnimationCollapsed(10, StudentManagementContainer);
            this.resetAnimation = new AnimationCollapsed
                (10, HomeContainer, RequestContainer,
                StudentManagementContainer, AccountContainer
                );
        }

        public void SetLoggedInStudent(StudentsEntity studentsEntity)
        {
            loggedInStudent = studentsEntity;
            DisplayProfilePicture(loggedInStudent);
            DisplayStudentInformation(loggedInStudent);

            _ = LoadEnrolledSubjectsAsync(loggedInStudent.CourseId.Value);
        }


        public void DisplayProfilePicture(StudentsEntity studentsEntity)
        {
            if (studentsEntity?.ProfilePicture != null && studentsEntity.ProfilePicture.Length > 0)
            {
                using (var memoryStream = new MemoryStream(studentsEntity.ProfilePicture))
                {
                    PBStudentProfile.Image = Image.FromStream(memoryStream);
                }
            }
        }


        public void DisplayStudentInformation(StudentsEntity student)
        {
            LBLStudentFullName.Text = student.FullName;
            LBLStudentId.Text = student.StudentId.ToString();
            LBLLastName.Text = student.LastName;
            LBLFirstName.Text = student.FirstName;
            LBLMiddleName.Text = student.MiddleName;
            LBLGender.Text = student.Gender;
            LBLBirtdate.Text = student.BirthDate.ToShortDateString();
            LBLPlaceOfBirth.Text = student.PlaceOfBirth;
            LBLReligion.Text = student.Religion;
            LBLNationality.Text = student.Nationality;
            LBLZipCode.Text = student.ZipCode.ToString();
            LBLCivilStatus.Text = student.CivilStatus;
            LBLCampus.Text = student.Campus;
            LBLContact.Text = student.Contact.ToString();
            LBLCourseCode.Text = student.CourseCode;
            LBLEmail.Text = student.Email;
            LBLHeight.Text = student.Height.ToString();
            LBLWeight.Text = student.Weight.ToString();
            LBLAddress.Text = student.Address;
        }


        public async Task LoadEnrolledSubjectsAsync(int courseId)
        {
            var subjectsEnrolled = await assignSubjectRepository.GetSubjectsByCourseId(courseId);
            DGVSubjectsEnrolledList.DataSource = subjectsEnrolled;
        }

        private async void PBStudentProfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png\""
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);

                loggedInStudent.ProfilePicture = imageBytes;

                DisplayProfilePicture(loggedInStudent);

                if (loggedInStudent != null && loggedInStudent.StudentId > 0)
                {
                    bool isUpdated = await studentRepository.UpdateStudentAsync(loggedInStudent.StudentId, loggedInStudent);

                    if (isUpdated)
                    {
                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void BTNStudentLogout_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Logged Out", "Student Logged Out");

            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            resetAnimation.Reset();
            loginForm.Show();
            this.Hide();
        }


        private async void BTNDashboard_Click(object sender, EventArgs e)
        {

            var studentDashboardForm = Program.ServiceProvider.GetRequiredService<StudentDashboardForm>();
            resetAnimation.Reset();
            studentDashboardForm.Show();
            await studentDashboardForm.LoadActivityLogs();
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


        private async void BTNStudentAccout_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Viewed Account Info", "Student Viewed Account");

            var studentUserAccountInfoForm = Program.ServiceProvider.GetRequiredService<StudentUserAccountInfo>();
            studentUserAccountInfoForm.SetLoggedInStudent(loggedInStudent);
            studentUserAccountInfoForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNAnimationHome_Click(object sender, EventArgs e)
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

        private void BTNRequestAnimation_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
        }
    }
}
