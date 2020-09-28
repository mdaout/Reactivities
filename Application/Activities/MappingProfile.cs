using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Activities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
            .ForMember(destination => destination.UserName, origin => origin.MapFrom(source => source.AppUser.UserName))
            .ForMember(destination => destination.DisplayName, origin => origin.MapFrom(source => source.AppUser.DisplayName))
            .ForMember(d => d.image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Uri));
           // .ForMember(destination => destination.IsHost, origin => origin.MapFrom(source => source.IsHost));
          //  .ForMember(destination => destination.image, origin => origin.MapFrom(source => source.))




        }
    }
}