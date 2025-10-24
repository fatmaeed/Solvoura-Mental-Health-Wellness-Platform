using Graduation_Project.Application.Interfaces.IRepositories;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Infrastructure.Data;

namespace Graduation_Project.Infrastructure.Repositories {

    public class SessionMeetingInfoRepo : BaseRepo<SessionMeetingInfoEntity>, ISessionMeetingInfoRepo {

        public SessionMeetingInfoRepo(MentalDbContext mentalDbContext) : base(mentalDbContext) {
        }

        public SessionMeetingInfoEntity? GetBySessionId(int sessionId) {
            return mentalDbContext.SessionMeetingInfos
                .Where(x => x.SessionId == sessionId && !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

        }
    }
}