using Graduation_Project.Application.DTOs.IllnessDTOs;
using Graduation_Project.Application.Utils;

namespace Graduation_Project.Application.Interfaces.IServices {

    public interface IIllnessService {

        public Either<Failure, List<DisplayIllnessDTO>> GetAllIllnesses();

        public Either<Failure, DisplayIllnessDTO> GetIllness(int id);

        //public void AddIllness(CreateIllnessDTO illness);
        //public void UpdateIllness(UpdateIllnessDTO illness);
        //public void DeleteIllness(int id);
    }
}