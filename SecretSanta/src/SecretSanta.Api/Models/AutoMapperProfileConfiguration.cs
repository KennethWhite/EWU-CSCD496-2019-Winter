using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Models
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<UserInputViewModel, User>();
            CreateMap<User, UserInputViewModel>();
            CreateMap<User, UserViewModel>();
            
            CreateMap<GiftViewModel, Gift>();
            CreateMap<Gift, GiftViewModel>();
            
            CreateMap<GroupInputViewModel, Group>();
            CreateMap<Group, GroupInputViewModel>();
            CreateMap<Group, GroupViewModel>();
        }
    }
}