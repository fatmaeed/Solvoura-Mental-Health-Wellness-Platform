using Graduation_Project.Application.DTOs.PostDTOs;
using Graduation_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IPostService
    {
        Task<IEnumerable<PostDTO?>> GetAllAsync();
        public Task AddAsync(CreatePostDTO entity);
        Task<PostDTO?> GetByIdAsync(int id);
        Task UpdateAsync(PostDTO entity);
        Task DeleteAsync(int id);
    }
}
