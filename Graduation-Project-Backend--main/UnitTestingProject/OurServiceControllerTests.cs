using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.OurServiceDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class OurServiceControllerTests
    {
        private Mock<IOurService> _serviceMock;
        private OurServiceController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IOurService>();
            _controller = new OurServiceController(_serviceMock.Object);
        }

        [TestMethod]
        public async Task GetAllServices_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ServiceEntity?>());

            // Act
            var result = await _controller.GetAllServices();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetServiceById_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(new DisplayServices());

            // Act
            var result = await _controller.GetServiceById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task AddService_ReturnsOk_OnValidInput()
        {
            // Arrange
            var dto = new OurserviceDTO();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.AddService(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _serviceMock.Verify(s => s.AddAsync(dto), Times.Once);
        }

        [TestMethod]
        public async Task AddService_ReturnsBadRequest_OnNullDto()
        {
            // Act
            var result = await _controller.AddService(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnsOk_OnSuccess()
        {
            // Arrange
            var dto = new OurserviceDTO();
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnsStatus500_OnFailure()
        {
            // Arrange
            var dto = new OurserviceDTO();
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            var statusResult = result as ObjectResult;
            Assert.IsNotNull(statusResult);
            Assert.AreEqual(500, statusResult.StatusCode);
        }

        [TestMethod]
        public async Task DeleteService_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteService(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
} 