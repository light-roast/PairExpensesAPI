using AutoMapper;
using PairExpensesAPI.Entities;
using PairXpensesAPI;

namespace PairXpensesAPI.MappingProfiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			// Mapping from UserReq to User
			CreateMap<UserReq, User>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map the Id property from UserReq to Id in User
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Map the Name property from UserReq to Name in User
                .ForMember(dest => dest.PairRole, opt => opt.MapFrom(src => src.PairRole));
        }
	}

	public class InverseUserProfile : Profile
	{
		public InverseUserProfile()
		{
			// Inverse mapping from User to UserReq
			CreateMap<User, UserReq>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map the Id property from User to Id in UserReq
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.PairRole, opt => opt.MapFrom(src => src.PairRole));
		}
	}
}
