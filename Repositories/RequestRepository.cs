using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using UnifiedEducationManagementSystem.Data_Connectivity.Data;
using UnifiedEducationManagementSystem.Data_Connectivity.Interfaces;
using UnifiedEducationManagementSystem.Domain.DTOs;
using UnifiedEducationManagementSystem.Domain.Entities;
using UnifiedEducationManagementSystem.Domain.Services;

namespace UnifiedEducationManagementSystem.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly UEMPDbContext uempDbContext;

        public RequestRepository(UEMPDbContext uempDbContext)
        {
            this.uempDbContext = uempDbContext;
        }


        public async Task AddRequestAsync(RequestsEntity request)
        {
            var students = await uempDbContext.Students
                .FindAsync(request.StudentId);

            if (students == null)
            {
                MessageBox.Show($"Student with ID {request.StudentId} does not exist.");
            }

            request.FirstName = students.FirstName;
            request.LastName = students.LastName;

            await uempDbContext.Requests.AddAsync(request);
            await uempDbContext.SaveChangesAsync();
        }

        public async Task<List<RequestDto>> GetAllRequestAsync()
        {
            return await uempDbContext.Requests
                .Select(r => new RequestDto
                {
                    RequestId = r.RequestId,
                    LastName = r.LastName,
                    FirstName = r.FirstName,
                    CertificateType = r.CertificateType,
                    Purpose = r.Purpose,
                    AdditionalComments = r.AdditionalComments,
                    RequestDate = r.RequestDate,
                    Status = r.Status
                })
               .ToListAsync();
        }

        public async Task <bool> UpdateRequestStatusAsync(int requestId, string newStatus)
        {
            var requestToUpdate = await uempDbContext.Requests.FirstOrDefaultAsync(r => r.RequestId == requestId);

            if (requestToUpdate != null &&
                requestToUpdate.Status == "Pending" ||
                requestToUpdate.Status == "Approve" ||
                requestToUpdate.Status == "Decline")
            {
                requestToUpdate.Status = newStatus;
                await uempDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task ApproveRequestStatusAsync(int requestId)
        {
            await UpdateRequestStatusAsync(requestId, "Approve");
        }

        public async Task DeclineRequestStatusAsync(int requestId)
        {
            await UpdateRequestStatusAsync(requestId, "Decline");
        }

        public async Task <List<RequestsEntity>> GetRequestByStatusAsync(string status)
        {
            return await uempDbContext.Requests
                .Where(c => c.Status == status)
                .ToListAsync();
        }
    }
}