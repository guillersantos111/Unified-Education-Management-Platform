using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Helpers;

namespace UnifiedEducationManagementSystem.Presentation.AdminForms
{
    public partial class AdminAssignStudentSubjectForm : Form
    {
        private readonly IAssignSubjectRepository assignSubjectRepository;
        private readonly IStudentRepository studentRepository;

        public AdminAssignStudentSubjectForm(IAssignSubjectRepository assignSubjectRepository, IStudentRepository studentRepository)
        {
            InitializeComponent();
            this.assignSubjectRepository = assignSubjectRepository;
            this.studentRepository = studentRepository;

            DGVStudentsList.SelectionChanged += DGVStudentList_SelectionChanged;
        }


        public async Task LoadEnrolledStudentListAsync()
        {
            var studentList = await studentRepository.GetAllStudentsAsync();
            DGVStudentsList.DataSource = studentList;
        }


        public async Task LoadAssignedOrEnrolledStudentAsync()
        {
            var assignOrEnrolledStudentList = await assignSubjectRepository.GetAssignedOrEnrolledStudentsAsync();

            LVAssignedOrEnrolledStudents.Items.Clear();

            foreach (var assignedStudentList in assignOrEnrolledStudentList)
            {
                var item = new ListViewItem(assignedStudentList.StudentSubjectEnrollmentId.ToString())
                {
                    Tag = assignedStudentList.StudentId,
                    SubItems =
                    {
                        assignedStudentList.StudentId.ToString(),
                        assignedStudentList.AcademicYear.ToString(),
                        assignedStudentList.Term
                    }
                };

                LVAssignedOrEnrolledStudents.Items.Add(item);
            }
            LVAssignedOrEnrolledStudents.Refresh();
        }


        private async void AdminAssignStudentSubjectForm_Load(object sender, System.EventArgs e)
        {
            await LoadEnrolledStudentListAsync();
        }


        private async void DGVStudentList_SelectionChanged(object sender, System.EventArgs e)
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

                // Call the Method and Capture the Return Value
                bool isAssigned = await assignSubjectRepository.AssignSubjectToStudentAsync(studentId, subjectId);

                if (isAssigned)
                {
                    MessageBox.Show("Subject assigned successfully!");
                    await LoadAssignedOrEnrolledStudentAsync();
                }
            }
        }
    }
}
