using ComponentFactory.Krypton.Toolkit;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Helpers;
using UnifiedEducationManagementSystem.Repositories;

namespace UnifiedEducationManagementSystem.Presentation.SuperAdminForms
{
    public partial class EnrollStudentForm : Form
    {

        private readonly ICourseRepository courseRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ICreateUserRepository createuserRepository;
        private byte[] ProfilePicture;


        public event EventHandler<StudentsEntity> StudentEnrolled;
        public int StudentId { get; set; } = 0;

        public EnrollStudentForm(ICourseRepository courseRepository ,IStudentRepository studentRepository ,ICreateUserRepository createuserRepository)
        {
            InitializeComponent();
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.createuserRepository = createuserRepository;
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await LoadCoursesAndCampusesAsync();
            if (StudentId != 0)
            {
                await PopulateStudentDetailsAsync();
            }
        }


        public async Task PopulateStudentDetailsAsync()
        {
            var student = await studentRepository.GetStudentByIdAsync(StudentId);
            if (student != null)
            {
                TBEmail.Text = student.Email;
                TBLastName.Text = student.LastName;
                TBFirstName.Text = student.FirstName;
                TBMiddleName.Text = student.MiddleName;
                CBMale.Checked = student.Gender == "Male";
                CBFemale.Checked = student.Gender == "Female";
                DTPBirthDate.Value = student.BirthDate;
                TBAddress.Text = student.Address;
                TBContact.Text = student.Contact.ToString();
                TBZipCode.Text = student.ZipCode.ToString();
                TBHeight.Text = student.Height.ToString();
                TBWeight.Text = student.Weight.ToString();
                TBPlaceOfBirth.Text = student.PlaceOfBirth;
                CBCivilStatus.SelectedItem = student.CivilStatus;
                TBNationality.Text = student.Nationality;
                TBReligion.Text = student.Religion;
                CBCampus.SelectedItem = student.Campus;
                CBCourses.SelectedItem = student.Courses;
                CBCourseCode.SelectedItem = student.CourseCode;
            }
        }


        private async Task LoadCoursesAndCampusesAsync()
        {
            try
            {
                var courses = await courseRepository.GetAllCoursesAsync();

                if (courses != null && courses.Any())
                {
                    var campuses = courses
                        .Where(c => !string.IsNullOrEmpty(c.Campus))
                        .Select(c => c.Campus)
                        .Distinct()
                        .ToList();

                    CBCourses.DataSource = courses
                        .ToList();
                    CBCourseCode.DataSource = courses
                        .Select(c => c.CourseCode)
                        .ToList();
                    CBCampus.DataSource = campuses;

                    CBCourses.DisplayMember = "Course";
                    CBCourses.ValueMember = "CourseId";

                    CBCourseCode.DisplayMember = "CourseCode";
                    CBCampus.DisplayMember = "Campus";
                }
                else
                {
                    MessageBox.Show("No courses available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading courses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ClearControls.Clear(this);
            }
        }

        private async void BTNSubmit_Click(object sender, EventArgs e)
        {
            string Gender = CBMale.Checked ? "Male" : (CBFemale.Checked ? "Female" : null);
            string CivilStatus = CBCivilStatus.SelectedItem?.ToString();
            string CourseCode = CBCourseCode.SelectedItem?.ToString();
            string Campus = CBCampus.SelectedItem?.ToString();

            if (!long.TryParse(TBContact.Text, out long Contact) ||
                !int.TryParse(TBZipCode.Text, out int ZipCode) ||
                !int.TryParse(TBHeight.Text, out int Height) ||
                !int.TryParse(TBWeight.Text, out int Weight) ||
                string.IsNullOrEmpty(Gender) ||
                string.IsNullOrEmpty(CivilStatus) ||
                string.IsNullOrEmpty(CourseCode) || 
                string.IsNullOrEmpty(Campus))
            {
                MessageBox.Show("All Fields are Required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var courses = (await courseRepository.GetAllCoursesAsync())
                .FirstOrDefault(c => c.CourseCode == CourseCode);

            var student = new StudentsEntity()
            {
                StudentId = StudentId,
                ProfilePicture = ProfilePicture,
                Email = TBEmail.Text,
                LastName = TBLastName.Text,
                FirstName = TBFirstName.Text,
                MiddleName = TBMiddleName.Text,
                Gender = Gender,
                BirthDate = DTPBirthDate.Value,
                Address = TBAddress.Text,
                Contact = Contact,
                ZipCode = ZipCode,
                PlaceOfBirth = TBPlaceOfBirth.Text,
                CivilStatus = CBCivilStatus.SelectedItem.ToString(),
                Nationality = TBNationality.Text,
                Religion = TBReligion.Text,
                Height = Height,
                Weight = Weight,
                Campus = CBCampus.SelectedItem.ToString(),
                CourseCode = CBCourseCode.SelectedItem.ToString(),
                CourseId = courses.CourseId
            };

            if (StudentId == 0)
            {
                StudentId = await studentRepository.AddStudentAsync(student);
                MessageBox.Show("Student enrolled successfully.");
            }
            else
            {
                await studentRepository.UpdateStudentAsync(StudentId, student);
                MessageBox.Show("Student details updated successfully.");
            }


            StudentEnrolled?.Invoke(this, student);
            ClearControls.Clear(this);
            StudentId = 0;
        }

        private void ApplicationClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void BTNBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files |*.jp;*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ProfilePicture = File.ReadAllBytes(openFileDialog.FileName);
                    PBProfilePicture.Image = Image.FromStream(new MemoryStream(ProfilePicture));
                }
            }
        }

        private void kryptonPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
