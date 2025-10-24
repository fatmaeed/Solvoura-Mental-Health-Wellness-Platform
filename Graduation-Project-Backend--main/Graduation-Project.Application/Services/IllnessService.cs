using AutoMapper;
using Graduation_Project.Application.DTOs.IllnessDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;

namespace Graduation_Project.Application.Services {

    public class IllnessService : IIllnessService {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public IllnessService(IUnitOfWork unitOfWork, IMapper mapper) {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public Either<Failure, List<DisplayIllnessDTO>> GetAllIllnesses() {
            try {
                List<IllnessEntity> illnesses = unitOfWork.IllnessRepo.GetAll().ToList();
                return Either<Failure, List<DisplayIllnessDTO>>.SendRight(mapper.Map<List<DisplayIllnessDTO>>(illnesses));
            } catch (Exception ex) {
                return Either<Failure, List<DisplayIllnessDTO>>.SendLeft(new Failure(ex.Message));
            }
        }

        public Either<Failure, DisplayIllnessDTO> GetIllness(int id) {
            throw new NotImplementedException();
        }
    }
}