using AutoMapper;
using Graduation_Project.Application.DTOs.CertificateDTOs;
using Graduation_Project.Application.DTOs.FeedBackDTOs;
using Graduation_Project.Application.DTOs.PaymentDTOs;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.UserLikesDTOs;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Graduation_Project.Application.ConfigrationMapper {

    public class MapperConfigAB : Profile {

        public MapperConfigAB()
        {
            
           CreateMap<CertificateEntity, CertificateDTO>();

            CreateMap<ServiceProviderEntity, DisplayServiceProviderDTO>()
                .ForMember(dest => dest.Specialization,
                    opt => opt.MapFrom(src => src.Specialization.ToString()))
                .ForMember(dest => dest.ExaminationType,
                    opt => opt.MapFrom(src => src.ExaminationType.ToString()))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src.Address)).ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.Certificates,
                    opt => opt.MapFrom(src => src.Certificates));
            //----------------
            CreateMap<CreateSessionsFromSPDTO, SessionEntity>().AfterMap((src,dist) =>
            {
                dist.Type = EnumHandler<SessionType>.GetEnumValue(src.Type);
            }).ReverseMap();

            CreateMap<SessionEntity ,DisplaySessionsForSPDTO>().AfterMap((src, dist) =>
            {
                dist.Type = EnumHandler<SessionType>.GetEnumName(src.Type);
                dist.Status = EnumHandler<SessionStatus>.GetEnumName(src.Status);
                dist.IsReversed = src.ReservationId is not null ;
            }).ReverseMap();


            CreateMap<CreatePaymentDTO , PaymentEntity>().AfterMap((src,dist) =>
            {
                dist.Status = EnumHandler<PaymentStatus>.GetEnumValue(src.Status);
            }).ReverseMap();

            CreateMap<CreateFeedBackDTO, FeedBackEntity>().ReverseMap();
            CreateMap<UserLikeDTO, UserLikes>()
                .ReverseMap();

        }
    }
}