using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Presentation.Animation;
using UnifiedEducationManagementSystem.Presentation.Forms;
using UnifiedEducationManagementSystem.Presentation.SuperAdminForms;

namespace UnifiedEducationManagementSystem.Presentation.AdminForms
{
    public partial class AdminRequestForm : Form
    {
        private readonly IRequestRepository requestRepository;
        private readonly AnimationCollapsed homeAnimation;
        private readonly AnimationCollapsed enrollentAnimation;
        private readonly AnimationCollapsed accountAnimation;
        private readonly AnimationCollapsed resetAnimation;
        private readonly AnimationCollapsed resetAnimations;

        private UserEntity loggedInAdmin;

        public AdminRequestForm(IRequestRepository requestRepository)
        {
            InitializeComponent();
            this.requestRepository = requestRepository;
            homeAnimation = new AnimationCollapsed(10, HomeContainer);
            enrollentAnimation = new AnimationCollapsed(10, EnrollmentContainer);
            accountAnimation = new AnimationCollapsed(10, AccountContainer);
            resetAnimation = new AnimationCollapsed(10, HomeContainer, EnrollmentContainer, AccountContainer);
        }

        public void SetLoggedInAdmin(UserEntity user)
        {
            loggedInAdmin = user;
            LBLAdminName.Text = $"{loggedInAdmin.FirstName} {loggedInAdmin.LastName}";
        }


        public async Task LoadRequestsAsync()
        {
            var requests = await requestRepository.GetAllRequestAsync();
            DGVStudentRequest.DataSource = requests;

            foreach (DataGridViewRow dgvRow in DGVStudentRequest.Rows)
            {
                var statusCell = dgvRow.Cells["DGVColumnStatus"];
                if (statusCell.Value != null)
                {
                    string status = statusCell.Value.ToString();
                    if (status == "Approve")
                    {
                        statusCell.Style.BackColor = Color.LimeGreen;
                    }
                    else if (status == "Decline")
                    {
                        statusCell.Style.BackColor = Color.Crimson;
                    }
                    else if (status == "Pending")
                    {
                        statusCell.Style.BackColor = Color.Goldenrod;
                    }
                }
            }
        }


        private async void AdminRequestForm_Load(object sender, EventArgs e)
        {
            await LoadRequestsAsync();
        }


        private async void DGVStudentRequest_CellContentClick(object sender, DataGridViewCellEventArgs dgvCellEventArgs)
        {
            try
            {
                if (dgvCellEventArgs.RowIndex >= 0 &&
                    (
                    DGVStudentRequest.Columns[dgvCellEventArgs.ColumnIndex].Name == "DGVBTNApprove" ||
                    DGVStudentRequest.Columns[dgvCellEventArgs.ColumnIndex].Name == "DGVBTNDecline"
                    ))
                {

                    var requestIdCell = DGVStudentRequest.Rows[dgvCellEventArgs.RowIndex].Cells["DGVColumnRequestId"];

                    if (requestIdCell.Value != null && int.TryParse(requestIdCell.Value.ToString(), out int requestId))
                    {
                        if (DGVStudentRequest.Columns[dgvCellEventArgs.ColumnIndex].Name == "DGVBTNApprove")
                        {
                            await requestRepository.ApproveRequestStatusAsync(requestId);

                            DGVStudentRequest.Rows[dgvCellEventArgs.RowIndex].Cells["DGVColumnStatus"].Style.BackColor = Color.LimeGreen;
                        }
                        else if (DGVStudentRequest.Columns[dgvCellEventArgs.ColumnIndex].Name == "DGVBTNDecline")
                        {
                            await requestRepository.DeclineRequestStatusAsync(requestId);

                            DGVStudentRequest.Rows[dgvCellEventArgs.RowIndex].Cells["DGVColumnStatus"].Style.BackColor = Color.Crimson;
                        }

                        await LoadRequestsAsync();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error :{e.Message}");
            }
        }


        private void BTNDashboard_Click(object sender, EventArgs e)
        {
            var adminDashBoard = Program.ServiceProvider.GetRequiredService<AdminDashboardForm>();
            adminDashBoard.Show();
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

        private void BTNCreateAccout_Click(object sender, EventArgs e)
        {
            var adminCreateAccount = Program.ServiceProvider.GetRequiredService<AdminCreateAccountForm>();
            adminCreateAccount.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNLogout_Click(object sender, EventArgs e)
        {
            var logout = Program.ServiceProvider.GetRequiredService<LoginForm>();
            logout.Show();
            resetAnimation.Reset();
            this.Hide();
        }

        private void BTNHome_Click(object sender, EventArgs e)
        {
            homeAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationAccount_Click(object sender, EventArgs e)
        {
            accountAnimation.Toggle();
            resetAnimation.Reset();
        }

        private void BTNAnimationEnrollments_Click(object sender, EventArgs e)
        {
            enrollentAnimation.Toggle();
            resetAnimation.Reset();
        }
    }
}
