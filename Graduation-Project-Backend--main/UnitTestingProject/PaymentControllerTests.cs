using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.PaymentDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _serviceMock;
        private PaymentController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IPaymentService>();
            _controller = new PaymentController(_serviceMock.Object);
        }

        [TestMethod]
        public async Task Create_ReturnsOk_OnValidInput()
        {
            // Arrange
            var dto = new CreatePaymentDTO();
            _serviceMock.Setup(s => s.CreatePayment(dto)).ReturnsAsync(1);
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.Create(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _serviceMock.Verify(s => s.CreatePayment(dto), Times.Once);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_OnNullDto()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_OnInvalidModel()
        {
            // Arrange
            var dto = new CreatePaymentDTO();
            _controller.ModelState.AddModelError("error", "error");

            // Act
            var result = await _controller.Create(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
} 