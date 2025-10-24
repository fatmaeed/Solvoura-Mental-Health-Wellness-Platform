using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.FeedBackDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class FeedBackControllerTests
    {
        private Mock<IFeedBackService> _serviceMock;
        private FeedBackController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IFeedBackService>();
            _controller = new FeedBackController(_serviceMock.Object);
        }

        [TestMethod]
        public async Task CreateFeedBack_ReturnsCreated_OnValidInput()
        {
            // Arrange
            var dto = new CreateFeedBackDTO();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.CreateFeedBack(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            _serviceMock.Verify(s => s.CreateFeedBack(dto), Times.Once);
        }

        [TestMethod]
        public async Task CreateFeedBack_ReturnsBadRequest_OnNullDto()
        {
            // Act
            var result = await _controller.CreateFeedBack(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateFeedBack_ReturnsBadRequest_OnInvalidModel()
        {
            // Arrange
            var dto = new CreateFeedBackDTO();
            _controller.ModelState.AddModelError("error", "error");

            // Act
            var result = await _controller.CreateFeedBack(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
} 