using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiedEducationManagementSystem.Domain.DTOs;
using UnifiedEducationManagementSystem.Domain.Entities;

namespace UnifiedEducationManagementSystem.Data_Connectivity.Interfaces
{
    public interface IRequestRepository
    {
        Task AddRequestAsync(RequestsEntity request);
        Task <List<RequestDto>> GetAllRequestAsync();
        Task ApproveRequestStatusAsync(int requestId);
        Task DeclineRequestStatusAsync(int requestId);
        Task <List<RequestsEntity>> GetRequestByStatusAsync(string status);
        Task <bool> UpdateRequestStatusAsync(int requestId, string newStatus);
    }
}


