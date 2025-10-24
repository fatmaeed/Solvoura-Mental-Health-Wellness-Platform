using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IServiceProviderService
    {
        public List<DisplayServiceProviderDTO> GetAllServiceProviders();
        public List<DisplayServiceProviderDTO> GetUnApprovedServiceProviders();
        public Task ApproveServiceProvider(int id);
         public Task RejectServiceProvider(int id);
        //public Task CreateServiceProvider(DisplayServiceProviderDTO serviceProvider);
        public Task<bool> UpdateServiceProviderAsync(int id, UpdateServiceProviderDTO dto);
         public Task DeleteServiceProvider(int id);
  

        public DisplayServiceProviderDTO GetServiceProviderById(int id);

         IEnumerable<PosponedandcanceledSessionDTO> GetSessionByStatus(int providerId);

         Task HandeDecideOnSession(SessionDecisionforClientDTO request);

        Task EditServiceProvider(EditServiceProviderDto providerDto);
        Task<IList<DisplayMeetingSessionDTO>> GetIncomingSessions(int proiderId);

    }
}
