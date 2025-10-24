using Graduation_Project.Application.DTOs.ClientDTOs;

namespace Graduation_Project.Application.Interfaces.IServices {

    public interface IClientService {
      Task  <List<DisplayClientDTO>> GetAllClients();
        List<ClientSessionDTO> GetClientSessions(int clientId);

        Task HandleSessionForClient(ClientRequestForSessionDTO request, int userId);

        Task<DisplayClientDTO> GetById(int id);
        Task DeleteClient(int id);
        Task<bool> UpdateClientAsync(int id, UpdateClientDTO dto);

        Task<List<DoctorsForClientDto>> GetDoctorsForClient(int id);
    }
}