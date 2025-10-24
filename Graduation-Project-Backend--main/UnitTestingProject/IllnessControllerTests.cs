using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.IllnessDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace UnitTestingProject
{
    [TestClass]
    public class IllnessControllerTests
    {
        private Mock<IIllnessService> _serviceMock;
        private IllnessController _controller;

        [TestInitialize]
        public void Setup()
        {
            _serviceMock = new Mock<IIllnessService>();
            _controller = new IllnessController(_serviceMock.Object);
        }

        [TestMethod]
        public void GetAll_ReturnsOk_OnSuccess()
        {
            // Arrange
            var either = Either<Failure, List<DisplayIllnessDTO>>.SendRight(new List<DisplayIllnessDTO>());
            _serviceMock.Setup(s => s.GetAllIllnesses()).Returns(either);

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAll_ReturnsBadRequest_OnFailure()
        {
            // Arrange
            var either = Either<Failure, List<DisplayIllnessDTO>>.SendLeft(new Failure("error"));
            _serviceMock.Setup(s => s.GetAllIllnesses()).Returns(either);

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
} 