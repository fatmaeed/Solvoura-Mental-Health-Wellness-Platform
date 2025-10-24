using Graduation_Project.Application.DTOs.ReservationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IReservationService 
    {
        Task<List<SessionDto>> GetFreeSessionsAsync( int serviceProviderId,string status,DateTime? startDate = null,DateTime? endDate = null,TimeSpan? duration = null);

         Task<ReservationRequestDto> CreateReservationAsync(ReservationRequestDto request);

    }
}
