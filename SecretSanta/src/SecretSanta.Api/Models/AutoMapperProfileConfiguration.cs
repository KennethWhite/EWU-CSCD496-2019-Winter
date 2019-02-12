using AutoMapper;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Models
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Gift, GiftViewModel>();

            CreateMap<User, UserViewModel>();
            CreateMap<UserInputViewModel, User>();

            CreateMap<Group, GroupViewModel>();
            CreateMap<GroupInputViewModel, Group>();

            CreateMap<GroupUser, GroupUserViewModel>();
        }
    }
}
