using Graduation_Project.Application.DTOs.CommentDTOs;
using Graduation_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO?>> GetAllAsync(int postId);
        Task<CommentDTO?> GetByIdAsync(int id);
        Task<CommentDTO> AddAsync(CreateCommentDTO entity);
        Task UpdateAsync(CommentDTO entity);
        Task DeleteAsync(int id);
    }
}
