using Graduation_Project.API.Controllers;
using Graduation_Project.API.Controllers.Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class SessionControllerTests
    {
        private Mock<ISessionService> _serviceMock;
        private SessionController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<ISessionService>();
            _controller = new SessionController(_serviceMock.Object);
        }

        [TestMethod]
        public void Get_ReturnsOk_OnSuccess()
        {
            // Arrange
            var either = Either<Failure, DisplayMeetingSessionDTO>.SendRight(new DisplayMeetingSessionDTO() { DoctorName ="amin", PatientName="hassan" , Status="1"});
            _serviceMock.Setup(s => s.GetMeetingSession(It.IsAny<int>())).Returns(either);

            // Act
            var result = _controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetSessionById_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetSessionById(It.IsAny<int>())).ReturnsAsync(new DisplaySessionDto());

            // Act
            var result = await _controller.GetSessionById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task EditSession_ReturnsOk_OnSuccess()
        {
            // Arrange
            var dto = new EditSessionDto();
            _serviceMock.Setup(s => s.EditSession(dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditSession(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task EditSession_ReturnsBadRequest_OnNullDto()
        {
            // Act
            var result = await _controller.EditSession(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task DeleteSession_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteSession(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteSession(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
} 