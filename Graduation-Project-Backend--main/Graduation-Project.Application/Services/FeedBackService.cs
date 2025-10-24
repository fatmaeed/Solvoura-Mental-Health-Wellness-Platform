using AutoMapper;
using Graduation_Project.Application.DTOs.FeedBackDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Services {
    public class FeedBackService : IFeedBackService {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedBackService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;


        }

        public async Task CreateFeedBack(CreateFeedBackDTO feedBackDTO) {
            SessionEntity? sessionEntity = _unitOfWork.SessionRepo.Get(feedBackDTO.SessionId);

            if (sessionEntity == null) {
                throw new ArgumentException("Session not found.");
            }
            int revieweeId;
            if (feedBackDTO.EvaluatorId == sessionEntity.ServiceProviderId) {
                revieweeId = sessionEntity.Reservation!.ClientId;
            } else {
                revieweeId = (int)sessionEntity!.ServiceProviderId!;

            }

            FeedBackEntity feedBack = _mapper.Map<FeedBackEntity>(feedBackDTO);
            feedBack.RevieweeId = revieweeId;
            _unitOfWork.FeedbackRepo.Add(feedBack);

            try {
                await _unitOfWork.SaveChangesAsync();
            } catch (Exception ex) {
                throw new ApplicationException("Could not save feedback at this time.", ex);
            }
        }
    }
}
