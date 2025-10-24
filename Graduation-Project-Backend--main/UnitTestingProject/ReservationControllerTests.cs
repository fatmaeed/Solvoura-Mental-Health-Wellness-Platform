using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.ReservationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class ReservationControllerTests
    {
        private Mock<IReservationService> _serviceMock;
        private ReservationController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IReservationService>();
            _controller = new ReservationController(_serviceMock.Object);
        }

        [TestMethod]
        public async Task GetFreeSessionsAsync_ReturnsOk_OnSuccess()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetFreeSessionsAsync(It.IsAny<int>(), It.IsAny<string>(), null, null, null))
                .ReturnsAsync(new List<SessionDto>());

            // Act
            var result = await _controller.GetFreeSessionsAsync(1, "active");

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task CreateReservationAsync_ReturnsBadRequest_OnNullRequest()
        {
            // Act
            var result = await _controller.CreateReservationAsync(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
} 