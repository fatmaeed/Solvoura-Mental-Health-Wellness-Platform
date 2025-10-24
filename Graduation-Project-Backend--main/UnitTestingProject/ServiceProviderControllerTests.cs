using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class ServiceProviderControllerTests
    {
        private class MockServiceProviderService : IServiceProviderService
        {
            public List<DisplayServiceProviderDTO> AllProviders = new();
            public List<DisplayServiceProviderDTO> UnapprovedProviders = new();
            public DisplayServiceProviderDTO ProviderById;
            public bool ApproveThrows = false;
            public bool RejectThrows = false;
            public bool UpdateResult = true;
            public bool DeleteThrows = false;
            public bool HandleSessionThrows = false;
            public bool EditThrows = false;
            public bool EditNotFound = false;
            public bool EditBadRequest = false;
            public IEnumerable<PosponedandcanceledSessionDTO> SessionByStatus = new List<PosponedandcanceledSessionDTO>();
            public IList<DisplayMeetingSessionDTO> IncomingSessions = new List<DisplayMeetingSessionDTO>();
            public Task<IList<DisplayMeetingSessionDTO>> GetIncomingSessions(int providerId) => Task.FromResult(IncomingSessions);
            public List<DisplayServiceProviderDTO> GetAllServiceProviders() => AllProviders;
            public List<DisplayServiceProviderDTO> GetUnApprovedServiceProviders() => UnapprovedProviders;
            public Task ApproveServiceProvider(int id) { if (ApproveThrows) throw new KeyNotFoundException(); return Task.CompletedTask; }
            public Task RejectServiceProvider(int id) { if (RejectThrows) throw new KeyNotFoundException(); return Task.CompletedTask; }
            public Task<bool> UpdateServiceProviderAsync(int id, UpdateServiceProviderDTO dto) => Task.FromResult(UpdateResult);
            public Task DeleteServiceProvider(int id) { if (DeleteThrows) throw new KeyNotFoundException(); return Task.CompletedTask; }
            public DisplayServiceProviderDTO GetServiceProviderById(int id) => ProviderById;
            public IEnumerable<PosponedandcanceledSessionDTO> GetSessionByStatus(int providerId) => SessionByStatus;
            public Task HandeDecideOnSession(SessionDecisionforClientDTO request) { if (HandleSessionThrows) throw new KeyNotFoundException(); return Task.CompletedTask; }
            public Task EditServiceProvider(EditServiceProviderDto providerDto) {
                if (EditThrows) throw new System.Exception();
                if (EditNotFound) throw new System.ArgumentException();
                if (EditBadRequest) throw new System.InvalidOperationException();
                return Task.CompletedTask;
            }
        }
        private class MockSessionService : ISessionService
        {
            public List<DisplaySessionsForSPDTO> AllSessions = new();
            public bool CreateSessionsCalled = false;
            public Task CreateSessionsFromSP(CreateSessionsFromSPDTO session) { CreateSessionsCalled = true; return Task.CompletedTask; }
            public List<DisplaySessionsForSPDTO> GetAllSessionsForServiceProvider(int serviceProviderId) => AllSessions;
            public Graduation_Project.Application.Utils.Either<Graduation_Project.Application.Utils.Failure, DisplayMeetingSessionDTO> GetMeetingSession(int id) => null;
            public Task EditSession(EditSessionDto sessionDto) => Task.CompletedTask;
            public Task DeleteSession(int id) => Task.CompletedTask;
            public Task<DisplaySessionDto> GetSessionById(int id) => Task.FromResult(new DisplaySessionDto());
        }
        private MockServiceProviderService _serviceProvider;
        private MockSessionService _sessionService;
        private ServiceProviderController _controller;
        [TestInitialize]
        public void Setup()
        {
            _serviceProvider = new MockServiceProviderService();
            _sessionService = new MockSessionService();
            _controller = new ServiceProviderController(_serviceProvider, _sessionService);
        }
        [TestMethod]
        public void GetAll_ReturnsOk_WhenProvidersExist()
        {
            _serviceProvider.AllProviders = new List<DisplayServiceProviderDTO> { new DisplayServiceProviderDTO() };
            var result = _controller.GetAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAllUnApprovedServiceProviders_ReturnsOk_WhenExist()
        {
            _serviceProvider.UnapprovedProviders = new List<DisplayServiceProviderDTO> { new DisplayServiceProviderDTO() };
            var result = _controller.GetAllUnApprovedServiceProviders();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
       
        [TestMethod]
        public void GetById_ReturnsOk_WhenProviderExists()
        {
            _serviceProvider.ProviderById = new DisplayServiceProviderDTO();
            var result = _controller.GetById(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void GetById_ReturnsNotFound_WhenProviderNull()
        {
            _serviceProvider.ProviderById = null;
            var result = _controller.GetById(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task CreateSessions_ReturnsOk_OnSuccess()
        {
            var dto = new CreateSessionsFromSPDTO();
            _controller.ModelState.Clear();
            var result = await _controller.CreateSessions(dto);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsTrue(_sessionService.CreateSessionsCalled);
        }
        [TestMethod]
        public async Task CreateSessions_ReturnsBadRequest_OnNull()
        {
            var result = await _controller.CreateSessions(null);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
        [TestMethod]
        public async Task CreateSessions_ReturnsBadRequest_OnInvalidModel()
        {
            var dto = new CreateSessionsFromSPDTO();
            _controller.ModelState.AddModelError("error", "error");
            var result = await _controller.CreateSessions(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public void GetSessionsForServiceProvider_ReturnsOk_WhenSessionsExist()
        {
            _sessionService.AllSessions = new List<DisplaySessionsForSPDTO> { new DisplaySessionsForSPDTO() };
            var result = _controller.GetSessionsForServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
       
        [TestMethod]
        public void GetSessionByStatus_ReturnsOk_WhenSessionsExist()
        {
            _serviceProvider.SessionByStatus = new List<PosponedandcanceledSessionDTO> { new PosponedandcanceledSessionDTO() };
            var result = _controller.GetSessionByStatus(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
       
        [TestMethod]
        public async Task ApproveServiceProvider_ReturnsOk_OnSuccess()
        {
            _serviceProvider.ApproveThrows = false;
            var result = await _controller.ApproveServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task ApproveServiceProvider_ReturnsNotFound_OnKeyNotFound()
        {
            _serviceProvider.ApproveThrows = true;
            var result = await _controller.ApproveServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        [TestMethod]
        public async Task RejectServiceProvider_ReturnsOk_OnSuccess()
        {
            _serviceProvider.RejectThrows = false;
            var result = await _controller.RejectServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task RejectServiceProvider_ReturnsNotFound_OnKeyNotFound()
        {
            _serviceProvider.RejectThrows = true;
            var result = await _controller.RejectServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        [TestMethod]
        public async Task Update_ReturnsNoContent_OnSuccess()
        {
            _serviceProvider.UpdateResult = true;
            var dto = new UpdateServiceProviderDTO();
            var result = await _controller.Update(1, dto);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
        [TestMethod]
        public async Task Update_ReturnsNotFound_OnFailure()
        {
            _serviceProvider.UpdateResult = false;
            var dto = new UpdateServiceProviderDTO();
            var result = await _controller.Update(1, dto);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task DeleteServiceProvider_ReturnsOk_OnSuccess()
        {
            _serviceProvider.DeleteThrows = false;
            var result = await _controller.DeleteServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task DeleteServiceProvider_ReturnsNotFound_OnKeyNotFound()
        {
            _serviceProvider.DeleteThrows = true;
            var result = await _controller.DeleteServiceProvider(1);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        [TestMethod]
        public async Task HandleDecideOnSession_ReturnsOk_OnSuccess()
        {
            _serviceProvider.HandleSessionThrows = false;
            var dto = new SessionDecisionforClientDTO();
            var result = await _controller.HandleDecideOnSession(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task HandleDecideOnSession_ReturnsNotFound_OnKeyNotFound()
        {
            _serviceProvider.HandleSessionThrows = true;
            var dto = new SessionDecisionforClientDTO();
            var result = await _controller.HandleDecideOnSession(dto);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        [TestMethod]
        public async Task EditServiceProvider_ReturnsOk_OnSuccess()
        {
            _serviceProvider.EditThrows = false;
            var dto = new EditServiceProviderDto();
            var result = await _controller.EditServiceProvider(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task EditServiceProvider_ReturnsNotFound_OnArgumentException()
        {
            _serviceProvider.EditNotFound = true;
            var dto = new EditServiceProviderDto();
            var result = await _controller.EditServiceProvider(dto);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        [TestMethod]
        public async Task EditServiceProvider_ReturnsBadRequest_OnInvalidOperationException()
        {
            _serviceProvider.EditBadRequest = true;
            var dto = new EditServiceProviderDto();
            var result = await _controller.EditServiceProvider(dto);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public async Task EditServiceProvider_ReturnsStatusCode500_OnException()
        {
            _serviceProvider.EditThrows = true;
            var dto = new EditServiceProviderDto();
            var result = await _controller.EditServiceProvider(dto);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objResult = result as ObjectResult;
            Assert.AreEqual(500, objResult.StatusCode);
        }
        [TestMethod]
        public async Task IncomingSessions_ReturnsOk_OnSuccess()
        {
            _serviceProvider.IncomingSessions = new List<DisplayMeetingSessionDTO> { new DisplayMeetingSessionDTO() {DoctorName="Ahmed" , PatientName= "Ali" , Status = "1" } };
            var result = await _controller.IncomingSessions(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
} 