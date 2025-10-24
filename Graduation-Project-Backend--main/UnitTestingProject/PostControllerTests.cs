using Graduation_Project.API.Controllers;
using Graduation_Project.Application.DTOs.PostDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingProject
{
    [TestClass]
    public class PostControllerTests
    {
        private class MockPostService : IPostService
        {
            public IEnumerable<PostDTO?> AllPosts = new List<PostDTO?>();
            public bool AddCalled = false;
            public bool UpdateCalled = false;
            public bool DeleteCalled = false;
            public PostDTO? PostById;
            public Task<IEnumerable<PostDTO?>> GetAllAsync() => Task.FromResult(AllPosts);
            public Task AddAsync(CreatePostDTO entity) { AddCalled = true; return Task.CompletedTask; }
            public Task<PostDTO?> GetByIdAsync(int id) => Task.FromResult(PostById);
            public Task UpdateAsync(PostDTO entity) { UpdateCalled = true; return Task.CompletedTask; }
            public Task DeleteAsync(int id) { DeleteCalled = true; return Task.CompletedTask; }
        }
        private MockPostService _service;
        private PostController _controller;
        [TestInitialize]
        public void Setup()
        {
            _service = new MockPostService();
            _controller = new PostController(_service);
        }
        [TestMethod]
        public async Task Get_ReturnsOk()
        {
            _service.AllPosts = new List<PostDTO?> { new PostDTO() };
            var result = await _controller.Get();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task Create_CallsAddAsync_AndReturnsOk()
        {
            var dto = new CreatePostDTO();
            var result = await _controller.Create(dto);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsTrue(_service.AddCalled);
        }
        [TestMethod]
        public async Task Update_CallsUpdateAsync_AndReturnsOk()
        {
            var dto = new PostDTO();
            var result = await _controller.Update(dto);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsTrue(_service.UpdateCalled);
        }
        [TestMethod]
        public async Task Delete_CallsDeleteAsync_AndReturnsOk()
        {
            var result = await _controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsTrue(_service.DeleteCalled);
        }
        [TestMethod]
        public async Task GetByIdAsync_ReturnsOk_WhenPostExists()
        {
            _service.PostById = new PostDTO();
            var result = await _service.GetByIdAsync(1);
            Assert.IsInstanceOfType(result, typeof(PostDTO));
        }
        [TestMethod]
        public async Task GetByIdAsync_ReturnsNull_WhenPostDoesNotExist()
        {
            _service.PostById = null;
            var result = await _service.GetByIdAsync(1);
            Assert.IsNull(result);
        }
    }
} 