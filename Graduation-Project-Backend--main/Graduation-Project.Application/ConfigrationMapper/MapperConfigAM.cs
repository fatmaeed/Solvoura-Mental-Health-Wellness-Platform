using AutoMapper;
using Graduation_Project.Application.DTOs.CertificateDTOs;
using Graduation_Project.Application.DTOs.ClientDTOs;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.DTOs.OurServiceDTOs;
using Graduation_Project.Application.DTOs.ReservationDTOs;
using Graduation_Project.Application.DTOs.ServiceProviderDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;
using System.Collections.Generic;

namespace Graduation_Project.Application.ConfigrationMapper {

    public class MapperConfigAM : Profile {

        public MapperConfigAM() {
            CreateMap<OurserviceDTO , ServiceEntity>().ReverseMap();
            CreateMap<SessionDto, SessionEntity>().AfterMap((s, d) =>
            {

                d.Status = Enum.TryParse<SessionStatus>(s.Status, out var status) ? status : SessionStatus.NotStarted;
                d.Type = Enum.TryParse<SessionType>(s.Type, out var type) ? type : default;
            }).ReverseMap();

            CreateMap<ReservationRequestDto, ReservationEntity>().AfterMap((s, d) =>
            {
                d.ReservationType = s.Status.ToLower() == "offline"
            ? ReservationType.Offline
            : ReservationType.Online;
                d.RamainingSessionsNumber = s.SessionsNumber;
                d.DoneSessionsNumber = 0;
                d.Date = DateTime.UtcNow;
                
            });
            CreateMap<SessionEntity,ClientSessionDTO>().AfterMap((s, d) =>
            {
                d.StartDateTime = s.StartDateTime;
                d.EndDateTime = s.StartDateTime.Add(s.Duration);
                d.CanCancelOrPostpone = s.Status == SessionStatus.NotStarted || s.Status == SessionStatus.Canceled;
                d.Status = s.Status.ToString();
                d.DoctorName = s.ServiceProvider.FirstName;
                d.Doctorspecialization = s.ServiceProvider.Specialization.ToString();
                d.UserImagePath = s.ServiceProvider.UserImagePath;
            }).ReverseMap();
            CreateMap<SessionEntity, PosponedandcanceledSessionDTO>().AfterMap((s, d) =>
            {
                d.ClientId = s.Reservation.ClientId;
                d.ClientName = s.Reservation.Client.FirstName;
            }).ReverseMap(); 
            CreateMap<ClientEntity,DisplayClientDTO>().ReverseMap();
            CreateMap<EditSessionDto, SessionEntity>();
            
            CreateMap<EditServiceProviderDto, ServiceProviderEntity>()
            .ForMember(dest => dest.Certificates, opt => opt.Ignore())
            .ForMember(dest => dest.UserImagePath, opt => opt.Ignore())
              .ForMember(dest => dest.ExaminationType, opt => opt.MapFrom(src => (ReservationType)src.ExaminationType));


            CreateMap<AddCertificateDTO, CertificateEntity>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CertificateName));

            CreateMap<CreateNotificationDTO, NotificationEntity>();
            CreateMap<NotificationUpdateDto, NotificationEntity>();
            CreateMap<NotificationEntity, DisplayNotificationDTO>();
           
        }
    }
}