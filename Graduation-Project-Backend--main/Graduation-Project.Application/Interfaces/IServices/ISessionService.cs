using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Utils;

namespace Graduation_Project.Application.Interfaces.IServices {
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface ISessionService {

        public Task CreateSessionsFromSP(CreateSessionsFromSPDTO sessionsFromSPDTO);

        public List<DisplaySessionsForSPDTO> GetAllSessionsForServiceProvider(int serviceProviderId);

        public Either<Failure, DisplayMeetingSessionDTO> GetMeetingSession(int id);

         Task EditSession(EditSessionDto sessionDto);
        Task DeleteSession(int id);
        Task<DisplaySessionDto> GetSessionById(int id);
    }
}