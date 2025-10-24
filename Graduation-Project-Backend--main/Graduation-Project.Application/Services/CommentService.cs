using AutoMapper;
using Graduation_Project.Application.DTOs.CommentDTOs;
using Graduation_Project.Application.DTOs.PostDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Services
{
    public class CommentService: ICommentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
       public CommentService(IUnitOfWork unitOfWork, IMapper mapper) {            
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<CommentDTO?>> GetAllAsync(int postId)
        {
            try
            {
                var comments = unitOfWork.CommentRepo.GetAll();
                if (postId > 0)
                {
                    comments = comments.Where(c => c.PostId == postId);
                }
                var result = mapper.Map<IEnumerable<CommentDTO>>(comments);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving all posts: {ex.Message}");
            }
        }
        public async Task<CommentDTO?> GetByIdAsync(int id)
        {
            var comment =  unitOfWork.CommentRepo.Get(id);
            var result= mapper.Map<CommentDTO>(comment);
            return await Task.FromResult(result);
        }
        public async Task<CommentDTO> AddAsync(CreateCommentDTO entity)
        {
            if (entity.PostId <= 0)
                throw new Exception("Post ID must be greater than zero.");
            if (unitOfWork.PostRepo.Get(entity.PostId) == null)
                throw new Exception($"Post with ID {entity.PostId} does not exist.");

            if (entity.ClientId.HasValue)
            {
                var client = unitOfWork.ClientRepo.Get(entity.ClientId.Value);
                if (client == null)
                    throw new Exception($"Client with ID {entity.ClientId.Value} does not exist.");
            }
            if (entity.ServiceProviderId.HasValue)
            {
                var sp = unitOfWork.ServiceProviderRepo.Get(entity.ServiceProviderId.Value);
                if (sp == null)
                    throw new Exception($"Service Provider with ID {entity.ServiceProviderId.Value} does not exist.");
            }
            var commentEntity = mapper.Map<CommentEntity>(entity);
            unitOfWork.CommentRepo.Add(commentEntity);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<CommentDTO>(commentEntity); 
        }
        public async Task UpdateAsync(CommentDTO entity)
        {
            var commentEntity =  unitOfWork.CommentRepo.Get(entity.Id);
            if (commentEntity == null)
                throw new Exception($"Comment with ID {entity.Id} not found.");
            mapper.Map(entity, commentEntity);
            unitOfWork.CommentRepo.Update(commentEntity);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var comment =  unitOfWork.CommentRepo.Get(id);
            if (comment == null)
                throw new Exception($"Comment with ID {id} not found.");
            unitOfWork.CommentRepo.Delete(comment.Id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
