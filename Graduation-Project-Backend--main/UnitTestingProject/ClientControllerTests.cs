using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.ClientDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class ClientControllerTests
    {
        private class MockClientService : IClientService
        {
            public List<DisplayClientDTO> Clients = new();
            public DisplayClientDTO Client;
            public List<ClientSessionDTO> Sessions = new();
            public bool UpdateResult = true;
            public bool DeleteCalled = false;
            public bool HandleSessionCalled = false;
            public List<DoctorsForClientDto> Doctors = new();
            public Task<List<DisplayClientDTO>> GetAllClients() => Task.FromResult(Clients);
            public List<ClientSessionDTO> GetClientSessions(int clientId) => Sessions;
            public Task HandleSessionForClient(ClientRequestForSessionDTO request, int userId) { HandleSessionCalled = true; return Task.CompletedTask; }
            public Task<DisplayClientDTO> GetById(int id) => Task.FromResult(Client);
            public Task DeleteClient(int id) { DeleteCalled = true; return Task.CompletedTask; }
            public Task<bool> UpdateClientAsync(int id, UpdateClientDTO dto) => Task.FromResult(UpdateResult);
            public Task<List<DoctorsForClientDto>> GetDoctorsForClient(int id) => Task.FromResult(Doctors);
        }

        private MockClientService _service;
        private ClientController _controller;

        [TestInitialize]
        public void Setup()
        {
            _service = new MockClientService();
            _controller = new ClientController(_service);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOk_WhenClientsExist()
        {
            _service.Clients = new List<DisplayClientDTO> { new DisplayClientDTO() };
            var result = await _controller.GetAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetAll_ReturnsNotFound_WhenNoClients()
        {
            _service.Clients = new List<DisplayClientDTO>();
            var result = await _controller.GetAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetClientById_ReturnsOk_WhenClientExists()
        {
            _service.Client = new DisplayClientDTO();
            var result = await _controller.GetClientById(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetClientById_ReturnsNotFound_WhenClientNull()
        {
            _service.Client = null;
            var result = await _controller.GetClientById(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void GetClientSessions_ReturnsOk_WhenSessionsExist()
        {
            _service.Sessions = new List<ClientSessionDTO> { new ClientSessionDTO() };
            var result = _controller.GetClientSessions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetClientSessions_ReturnsNotFound_WhenNoSessions()
        {
            _service.Sessions = new List<ClientSessionDTO>();
            var result = _controller.GetClientSessions(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task HandleSessionForClient_ReturnsOk_OnSuccess()
        {
            var dto = new ClientRequestForSessionDTO { ClientId = 1, SessionId = 1, ActionType = SessionActionType.Cancel, Reason = "test" };
            var result = await _controller.HandleSessionForClient(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(_service.HandleSessionCalled);
        }

        [TestMethod]
        public async Task Update_ReturnsNoContent_OnSuccess()
        {
            _service.UpdateResult = true;
            var dto = new UpdateClientDTO();
            _controller.ModelState.Clear();
            var result = await _controller.Update(1, dto);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Update_ReturnsNotFound_OnFailure()
        {
            _service.UpdateResult = false;
            var dto = new UpdateClientDTO();
            _controller.ModelState.Clear();
            var result = await _controller.Update(1, dto);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Update_ReturnsBadRequest_OnInvalidModel()
        {
            var dto = new UpdateClientDTO();
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.Update(1, dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOk_OnSuccess()
        {
            var result = await _controller.delete(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(_service.DeleteCalled);
        }

        [TestMethod]
        public async Task GetDoctorsForClient_ReturnsList()
        {
            _service.Doctors = new List<DoctorsForClientDto> { new DoctorsForClientDto() };
            var result = await _service.GetDoctorsForClient(1);
            Assert.IsInstanceOfType(result, typeof(List<DoctorsForClientDto>));
            Assert.AreEqual(1, result.Count);
        }
    }
} 