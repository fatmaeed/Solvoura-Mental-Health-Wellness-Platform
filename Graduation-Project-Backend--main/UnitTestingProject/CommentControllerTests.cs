using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.CommentDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class CommentControllerTests
    {
        private Mock<ICommentService> _serviceMock;
        private CommentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<ICommentService>();
            _controller = new CommentController(_serviceMock.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(new List<CommentDTO?>());

            // Act
            var result = await _controller.GetAll(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetById_ReturnsOk_OnFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new CommentDTO());

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_OnNull()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CommentDTO)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Add_ReturnsCreated_OnSuccess()
        {
            // Arrange
            var dto = new CreateCommentDTO();
            var added = new CommentDTO { Id = 1 };
            _serviceMock.Setup(s => s.AddAsync(dto)).ReturnsAsync(added);

            // Act
            var result = await _controller.Add(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task Update_ReturnsNoContent_OnSuccess()
        {
            // Arrange
            var dto = new CommentDTO();
            _serviceMock.Setup(s => s.UpdateAsync(dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsNoContent_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
} 