using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Graduation_Project.Application.DTOs.UserLikesDTOs;
using Graduation_Project.Application.Interfaces.IServices;
using Graduation_Project.Application.Interfaces.IUnitOfWorks;
using Graduation_Project.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Application.Services
{
    public class UserLikesService : IUserLikesService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;


        public UserLikesService(IUnitOfWork unitOfWork , IMapper mapper , IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task PutUserLike(UserLikeDTO userLikeDTO)
        {
            int.TryParse(httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value, out int userId);
            var userlike = unitOfWork.UserLikesRepo.GetAllAsQuerable()
                .AsNoTracking().FirstOrDefault(ul => ul.PostId == userLikeDTO.PostId && ul.UserId == userId);
            if (userlike is null) 
            {
                unitOfWork.UserLikesRepo.Add(mapper.Map<UserLikes>(userLikeDTO));
            }else
            {
                 userLikeDTO.Id = userlike.Id;
                unitOfWork.UserLikesRepo.Update(mapper.Map<UserLikes>(userLikeDTO));
            }
            await unitOfWork.SaveChangesAsync();
            }
    }
}
