using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.UserLikesDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class UserLikesControllerTests
    {
        private Mock<IUserLikesService> _userLikesServiceMock;
        private UserLikesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _userLikesServiceMock = new Mock<IUserLikesService>();
            _controller = new UserLikesController(_userLikesServiceMock.Object);
        }

        [TestMethod]
        public async Task PutUserLike_ReturnsNoContent_OnValidInput()
        {
            // Arrange
            var dto = new UserLikeDTO();
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.PutUserLike(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _userLikesServiceMock.Verify(s => s.PutUserLike(dto), Times.Once);
        }

        [TestMethod]
        public async Task PutUserLike_ReturnsBadRequest_OnNullDto()
        {
            // Act
            var result = await _controller.PutUserLike(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task PutUserLike_ReturnsBadRequest_OnInvalidModel()
        {
            // Arrange
            var dto = new UserLikeDTO();
            _controller.ModelState.AddModelError("error", "error");

            // Act
            var result = await _controller.PutUserLike(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
} 