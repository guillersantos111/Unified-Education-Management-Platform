using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.StudentForms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;
using UnifiedEducationManagementSystem.Repositories;
using UnifiedEducationManagementSystem.Services;

namespace UnifiedEducationManagementSystem.Presentation.Forms
{
    public partial class StudentDashboardForm : Form
    {
        private readonly IActivityLogsRepository activityLogsRepository;
        private readonly ActivityLogsService activityLogsService;
        private readonly IStudentRepository studentRepository;
        private readonly IAssignSubjectRepository assignSubjectRepository;
        private readonly ICreateUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRequestRepository certificateRepository;
        private readonly StudentPorfileForm studentPorfileForm;
        private readonly AnimationCollapsed enrollmentAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed requestAnimation;
        private readonly AnimationCollapsed studentManagementAnimation;
        private readonly AnimationCollapsed resetAnimation;

        private ActivityLogsEntity activityLogsEntity;
        private StudentsEntity loggedInStudent;

        public StudentDashboardForm
            (
            ActivityLogsService activityLogsService,
            IActivityLogsRepository activityLogsRepository,
            IStudentRepository studentRepository,
            IAssignSubjectRepository assignSubjectRepository,
            ICreateUserRepository userRepository,
            IRoleRepository roleRepository,
            IRequestRepository certificateRepository,
            StudentPorfileForm studentPorfileForm,
            StudentsEntity loggedInStudent
            )
        {
            InitializeComponent();
            this.activityLogsService = activityLogsService;
            this.activityLogsRepository = activityLogsRepository;
            this.studentRepository = studentRepository;
            this.assignSubjectRepository = assignSubjectRepository;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.certificateRepository = certificateRepository;
            this.studentPorfileForm = studentPorfileForm;
            this.loggedInStudent = loggedInStudent;
            this.accountAnimation = new AnimationCollapsed(10, AccountContainer);
            this.requestAnimation = new AnimationCollapsed(10, RequestContainer);
            this.studentManagementAnimation = new AnimationCollapsed(10, StudentManagementContainer);
            this.resetAnimation = new AnimationCollapsed
                (10, HomeContainer,RequestContainer,
                StudentManagementContainer, AccountContainer
                );
        }

        public void SetLoggedIsStudent(StudentsEntity students)
        {
            this.loggedInStudent = students;
            LBLFullName.Text = $"{loggedInStudent.LastName} {loggedInStudent.FirstName}";
            _ = LoadSchedule(loggedInStudent.CourseId.Value);
            _ = LoadActivityLogs();
        }


        public async Task LoadActivityLogs()
        {
            // Fetch activity logs for the logged-in student
            var logs = await activityLogsRepository.GetStudentActivityLogs(loggedInStudent.StudentId);
            DGVActivityLogs.DataSource = logs;
        }


        private async Task LoadSchedule(int courseId)
        {
            var displaySchedule = await assignSubjectRepository.GetSubjectsByCourseId(courseId);
            DGVSchedule.DataSource = displaySchedule;
        }


        private void BTNRequestAnimation_Click(object sender, EventArgs e)
        {
            requestAnimation.Toggle();
            resetAnimation.Reset();
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

        private async void BTNStudentLogout_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Logged Out", "Student Logged Out");

            var loginForm = Program.ServiceProvider.GetRequiredService<LoginForm>();
            loginForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private async void BTNStudentProfile_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Viewed Profile", "Student Viewed Profile");

            var studentProfile = Program.ServiceProvider.GetRequiredService<StudentPorfileForm>();
            studentPorfileForm.SetLoggedInStudent(loggedInStudent);
            studentProfile.Show();
            resetAnimation.Reset();
            this.Hide();
        }


        private async void BTNUserAccountInfo_Click(object sender, EventArgs e)
        {
            await activityLogsService.LogActivity(loggedInStudent.StudentId, "Viewed Account Info", "Student Viewed Account");

            var studentUserAccountInfoForm = Program.ServiceProvider.GetRequiredService<StudentUserAccountInfo>();
            studentUserAccountInfoForm.SetLoggedInStudent(loggedInStudent);
            studentUserAccountInfoForm.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNAnimationAccount_Click(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNStudentManagement_Click(object sender, EventArgs e)
        {
            studentManagementAnimation.Toggle();
            resetAnimation.Reset();
        }
    }
}
