using AutoMapper;
using Graduation_Project.Application.DTOs.AccountDTOs;
using Graduation_Project.Application.DTOs.CertificateDTOs;
using Graduation_Project.Application.DTOs.IllnessDTOs;
using Graduation_Project.Application.DTOs.NotificationDTOs;
using Graduation_Project.Application.DTOs.SessionDTOs;
using Graduation_Project.Application.DTOs.SymptomDTOs;
using Graduation_Project.Application.Utils;
using Graduation_Project.Domain.Entities;
using Graduation_Project.Domain.Enums;

namespace Graduation_Project.Application.ConfigrationMapper {

    public class MapperConfigH : Profile {

        public MapperConfigH() {
            //------
            CreateMap<IllnessEntity, DisplayIllnessDTO>().AfterMap((src, dest) => {
                dest.Level = EnumHandler<LevelsType>.GetEnumName(src.Level);
                dest.IllnessDescription = src.Description;
                dest.IllnessName = src.Title;
                dest.Image = $"{ConstantData.backEndServerName}{src.ImagePath}";
            }).ForMember(dest => dest.Symptoms, opt => opt.MapFrom(src => src.Symptoms))
            .ForMember(dest => dest.RelatedSpecialties, opt => opt.MapFrom(src => src.related));

            CreateMap<RelatedToEntity, string?>().ConvertUsing(src => EnumHandler<Specialization>.GetEnumName(src.RelatedTo));

            CreateMap<SymptomEntity, DisplaySymptomDTO>().AfterMap((src, dest) => {
                dest.SymptomName = src.Title;
                dest.Levels = EnumHandler<LevelsType>.GetEnumName(src.Levels);
            });
            CreateMap<CreateCertificateDTO, CertificateEntity>().AfterMap(async (src, dest) => {
                dest.Name = src.CertificateName;
                dest.Description = src.Description;
                dest.IssueDate = src.IssueDate;
                UploadingImageStatus up = await ImageHandler.UploadImage(src.Image, "Certificates");
                if(up is UploadImageSuccess)
                {
                    var success = up as UploadImageSuccess;
                    dest.ImagePath = success.Path;
                }
            });

            //------
            CreateMap<RegisterUserDTO, UserEntity>();

            CreateMap<RegisterClientDTO, UserEntity>().IncludeBase<RegisterUserDTO, UserEntity>();
            CreateMap<RegisterServiceProviderDTO, UserEntity>().IncludeBase<RegisterUserDTO, UserEntity>();
            CreateMap<PersonDTO, PersonEntity>().
                ForMember(dest => dest.Gender, opt => opt.MapFrom(src => (Gender)src.Gender));
            //-----------
            CreateMap<RegisterClientDTO, ClientEntity>().AfterMap((src, dest) => {
                dest.NeededServices = src.NeededSpecilization == null ? null : (Specialization)src.NeededSpecilization;
                dest.NationalId = src.NationalNumber;
                dest.AlternativePhoneNumber = src.SecondPhoneNumber;
                dest.HistoryIllness = src.IllnessesHistory;
            }).IncludeBase<PersonDTO, PersonEntity>();
            //-----------
            CreateMap<RegisterServiceProviderDTO, ServiceProviderEntity>().
                ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => (Specialization)src.Specialization)).
                ForMember(dest => dest.ExaminationType, opt => opt.MapFrom(src => (ReservationType)src.ExaminationType))
                .AfterMap((src, dest) => {
                    dest.NationalId = src.NationalNumber;
                    dest.Experience = src.ExperienceDescription;
                    dest.NumberOfExp = src.ExperienceInYears;
                }).IncludeBase<PersonDTO, PersonEntity>();

            CreateMap<SessionEntity, DisplayMeetingSessionDTO>().AfterMap((src, dest) => {
                dest.SessionId = src.Id;
                dest.Status = EnumHandler<SessionStatus>.GetEnumName(src.Status)!;
                dest.DoctorName = src.ServiceProvider.FirstName + " " + src.ServiceProvider.LastName;
                dest.PatientName = src.Reservation?.Client.FirstName + " " + src.Reservation?.Client.LastName;
            });

            CreateMap<SessionUserConnectionEntity, SessionUserConnectionDTO>().ReverseMap();
            CreateMap<ConnectionSessionMeetingEntity, BaseConnectionSessionMeetingDTO>().ReverseMap();
            CreateMap<ConnectionSessionMeetingEntity, CreateConnectionSessionMeetingDTO>().IncludeBase<ConnectionSessionMeetingEntity, BaseConnectionSessionMeetingDTO>();
            CreateMap<ConnectionSessionMeetingEntity, DisplayConnectionSessionMeetingDTO>().IncludeBase<ConnectionSessionMeetingEntity, BaseConnectionSessionMeetingDTO>();
            CreateMap<CreateNotificationDTO, NotificationEntity>().ReverseMap();
            CreateMap<NotificationEntity, DisplayNotificationDTO>().AfterMap((src, dest) => {
                dest.NotificationTime = src.CreatedAt;
                dest.SenderName = src?.User?.UserName;
            });

            CreateMap<UserNotificationEntity, DisplayNotificationDTO>()
                 .ConstructUsing((src, context) =>
                     context.Mapper.Map<DisplayNotificationDTO>(src.Notification!)
                 ).AfterMap((src, dest) => {
                     dest.Id = src.NotificationId;
                 });
        }
    }
}