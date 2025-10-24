using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graduation_Project.Application.DTOs.UserLikesDTOs;

namespace Graduation_Project.Application.Interfaces.IServices
{
    public interface IUserLikesService
    {
        public Task PutUserLike(UserLikeDTO userLikeDTO);
    }
}
