using System.Security.Claims;
using AutoMapper;
using Graduation_Project.Application.DTOs.PostDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Graduation_Project.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PostService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }



        public Task AddAsync(CreatePostDTO entity)
        {
            if (entity.ClientId.HasValue)
            {
                var client = unitOfWork.ClientRepo.Get(entity.ClientId.Value);
                if (client == null)
                    throw new Exception($"Client with ID {entity.ClientId.Value} does not exist.");
            }
            // Check if Service Provider exists (if specified)
            if (entity.ServiceProviderId.HasValue)
            {
                var sp = unitOfWork.ServiceProviderRepo.Get(entity.ServiceProviderId.Value);
                if (sp == null)
                    throw new Exception($"Service Provider with ID {entity.ServiceProviderId.Value} does not exist.");
            }
            var postEntity = mapper.Map<PostEntity>(entity);
            postEntity.PostDate = DateTime.Now;
            unitOfWork.PostRepo.Add(postEntity);
            return unitOfWork.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var post = unitOfWork.PostRepo.Get(id);
                if (post == null)
                    throw new Exception($"Post with ID {id} not found.");

                unitOfWork.PostRepo.Delete(post.Id);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting post: {ex.Message}");
            }
        }

        public async Task<IEnumerable<PostDTO>> GetAllAsync()
        {
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value, out int userId);
            try
            {
                var posts = unitOfWork.PostRepo.GetAll();
                var result = mapper.Map<IEnumerable<PostDTO>>(posts);

                foreach (var post in result)
                {
                    var userLike = unitOfWork.UserLikesRepo.GetAllAsQuerable().AsNoTracking().Where(ul => ul.PostId == post.Id).ToList();
                    if (userLike.Count() == 0)
                        post.IsLikedByCurrentUser = false;
                    else if (post.ClientId is not null)
                    {
                        var targetUL = userLike.FirstOrDefault(ul => userId == ul.UserId);
                        post.IsLikedByCurrentUser = targetUL?.Isliked ?? false;
                    }
                    else if (post.ServiceProviderId is not null)
                    {
                        post.IsLikedByCurrentUser = userLike.FirstOrDefault(ul => userId == ul.UserId)?.Isliked ?? false;
                    }
                    else
                    {
                        post.IsLikedByCurrentUser = false;
                    }
                }
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all posts: {ex.Message}");
            }
        }

        public async Task<PostDTO?> GetByIdAsync(int id)
        {
            try
            {
                var post = unitOfWork.PostRepo.Get(id);
                return await Task.FromResult(mapper.Map<PostDTO?>(post));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving post by ID {id}: {ex.Message}");
            }
        }

        public async Task UpdateAsync(PostDTO dto)
        {
            try
            {
                var post = unitOfWork.PostRepo.GetAllAsQuerable().AsNoTracking().FirstOrDefault(p => p.Id == dto.Id);
                var postEntity = mapper.Map<PostEntity>(dto);
                postEntity.PostDate = post?.PostDate;
                unitOfWork.PostRepo.Update(postEntity);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating post: {ex.Message}");
            }
        }


    }
}