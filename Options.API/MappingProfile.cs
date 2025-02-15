using AutoMapper;
using Options.DbContext.Models;
using Options.Domain.Models;

namespace Options.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Option, OptionRequestModel>().ReverseMap();
            CreateMap<Settings, UpdateSettingsRequestModel>().ReverseMap();
            CreateMap<UserProfile, UserProfileRequestModel>().ReverseMap();
        }
    }

}
