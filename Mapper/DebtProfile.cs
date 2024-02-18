using AutoMapper;
using PairExpensesAPI.Entities;
using PairXpensesAPI;

namespace PairXpensesAPI.MappingProfiles
{
	public class DebtProfile : Profile
	{
		public DebtProfile()
		{
			// Mapping from DebtReq to Debt
			CreateMap<DebtReq, Debt>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map the Id property from DebtReq to Id in Debt
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Map the Name property from DebtReq to Name in Debt
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value)) // Map the Value property from DebtReq to Value in Debt
				.ForMember(dest => dest.CreateDate, opt => opt.Ignore()) // Ignore the CreateDate property as it's typically set automatically on the server
				.ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate)); // Map the UpdateDate property from DebtReq to UpdateDate in Debt
		}
	}

	public class InverseDebtProfile : Profile
	{
		public InverseDebtProfile()
		{
			// Inverse mapping from Debt to DebtReq
			CreateMap<Debt, DebtReq>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map the Id property from Debt to Id in DebtReq
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Map the Name property from Debt to Name in DebtReq
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value)) // Map the Value property from Debt to Value in DebtReq
				.ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate)) // Map the UpdateDate property from Debt to UpdateDate in DebtReq
				.ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate)); // Map the CreateDate property from Debt to CreateDate in DebtReq
		}
	}
}
