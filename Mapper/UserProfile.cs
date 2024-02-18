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
				.ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore the Id property as it's typically generated automatically in the database
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)); // Map the Name property from UserReq to Name in User
		}
	}

	public class InverseUserProfile : Profile
	{
		public InverseUserProfile()
		{
			// Inverse mapping from User to UserReq
			CreateMap<User, UserReq>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)); // Map the Name property from User to Name in UserReq
		}
	}
}
