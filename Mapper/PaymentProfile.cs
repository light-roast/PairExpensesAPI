using AutoMapper;
using PairExpensesAPI.Entities;
using PairXpensesAPI;

namespace PairXpensesAPI.MappingProfiles
{
	public class PaymentProfile : Profile
	{
		public PaymentProfile()
		{
			// Mapping from PaymentReq to Payment
			CreateMap<PaymentReq, Payment>()
				.ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore the Id property as it's typically generated automatically in the database
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Map the Name property from PaymentReq to Name in Payment
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value)) // Map the Value property from PaymentReq to Value in Payment
				.ForMember(dest => dest.CreateDate, opt => opt.Ignore()) // Ignore the CreateDate property as it's typically set automatically on the server
				.ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate)); // Map the UpdateDate property from PaymentReq to UpdateDate in Payment
		}
	}

	public class InversePaymentProfile : Profile
	{
		public InversePaymentProfile()
		{
			// Inverse mapping from Payment to PaymentReq
			CreateMap<Payment, PaymentReq>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Map the Id property from Payment to Id in PaymentReq
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) // Map the Name property from Payment to Name in PaymentReq
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value)) // Map the Value property from Payment to Value in PaymentReq
				.ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate)) // Map the UpdateDate property from Payment to UpdateDate in PaymentReq
				.ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate)); // Map the CreateDate property from Payment to CreateDate in PaymentReq
		}
	}
}
