using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class NotificationControllerTests
    {
        private class MockNotifiService : INotifiService
        {
            public DisplayNotificationDTO? Notification;
            public IEnumerable<DisplayNotificationDTO> Unread = new List<DisplayNotificationDTO>();
            public IEnumerable<DisplayNotificationDTO> AllForUser = new List<DisplayNotificationDTO>();
            public bool AddCalled = false;
            public bool UpdateCalled = false;
            public bool DeleteCalled = false;
            public DisplayNotificationDTO? GetNotification(int id) => Notification;
            public IEnumerable<DisplayNotificationDTO> GetAllUnread(int userId) => Unread;
            public IEnumerable<DisplayNotificationDTO> GetAllNotificationsForUser(int userId) => AllForUser;
            public Task Add(CreateNotificationDTO notification) { AddCalled = true; return Task.CompletedTask; }
            public Task Update(NotificationUpdateDto notification) { UpdateCalled = true; return Task.CompletedTask; }
            public Task Delete(int id) { DeleteCalled = true; return Task.CompletedTask; }
        }
        private MockNotifiService _service;
        private NotificationController _controller;
        [TestInitialize]
        public void Setup()
        {
            _service = new MockNotifiService();
            _controller = new NotificationController(_service);
        }
        [TestMethod]
        public void Get_ReturnsOk()
        {
            _service.Notification = new DisplayNotificationDTO() { Message ="mesage1" , Title="Title1",Type="success"};
            var result = _controller.Get(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void GetAllUnReadNotifications_ReturnsOk()
        {
            _service.Unread = new List<DisplayNotificationDTO> { new DisplayNotificationDTO() { Message = "mesage1", Title = "Title1", Type = "success" } };
            var result = _controller.GetAllUnReadNotifications(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void GetAllNotifications_ReturnsOk()
        {
            _service.AllForUser = new List<DisplayNotificationDTO> { new DisplayNotificationDTO() { Message = "mesage1", Title = "Title1", Type = "success" } };
            var result = _controller.GetAllNotifications(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void AddNoti_CallsAdd_AndReturnsOk()
        {
            var dto = new CreateNotificationDTO() { Message = "mesage1", Title = "Title1", Type = "success" };
            var result = _controller.AddNoti(dto);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(_service.AddCalled);
        }
        [TestMethod]
        public async Task Update_CallsUpdate_AndReturnsOk()
        {
            var dto = new NotificationUpdateDto() { Message = "mesage1", Title = "Title1", Type = "success" };
            var result = await _controller.Update(dto);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsTrue(_service.UpdateCalled);
        }
        [TestMethod]
        public async Task Delete_CallsDelete_AndReturnsNoContent()
        {
            var result = await _controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.IsTrue(_service.DeleteCalled);
        }
    }
} 