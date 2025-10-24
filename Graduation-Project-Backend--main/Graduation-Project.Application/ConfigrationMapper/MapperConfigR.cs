using System;
using AutoMapper;
using Graduation_Project.Application.DTOs.ClientDTOs;
using Graduation_Project.Application.DTOs.CommentDTOs;
using Graduation_Project.Application.DTOs.OurServiceDTOs;
using Graduation_Project.Application.DTOs.PostDTOs;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Application.ConfigrationMapper
{
    public class MapperConfigR : Profile
    {
        public MapperConfigR()
        {
            CreateMap<PostEntity, PostDTO>().AfterMap((src,dist) =>
            {

                var timeSpan = DateTime.Now - src.PostDate.Value;
                if (timeSpan.TotalSeconds < 60)
                    dist.Date = "just now";
                else if (timeSpan.TotalMinutes < 60)
                    dist.Date = $"{(int)timeSpan.TotalMinutes}m";

                else if (timeSpan.TotalHours < 24)
                    dist.Date = $"{(int)timeSpan.TotalHours}h";

                else if (timeSpan.TotalDays < 30)
                    dist.Date = $"{(int)timeSpan.TotalDays}d ";

                else if (timeSpan.TotalDays < 365)
                    dist.Date = $"{(int)(timeSpan.TotalDays / 30)}mo";
                else
                    dist.Date = $"{(int)(timeSpan.TotalDays / 365)}y";
                dist.UserLikeId = src?.UserLikes?.Id;
                
            }).ReverseMap();
            CreateMap<CreatePostDTO, PostEntity>();
            CreateMap<UpdatePostDTO, PostEntity>();

            CreateMap<CommentEntity, CommentDTO>().ReverseMap();
            CreateMap<CreateCommentDTO, CommentEntity>();
            CreateMap<UpdateCommentDTO, CommentEntity>();
            CreateMap<ClientEntity, DisplayClientDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt
                .MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
    .ReverseMap()
    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender)))
                .ReverseMap();

            CreateMap<UpdateServiceProviderDTO, ServiceProviderEntity>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateClientDTO, ClientEntity>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap< ServiceEntity, DisplayServices>().ReverseMap();

        }
    }
}
