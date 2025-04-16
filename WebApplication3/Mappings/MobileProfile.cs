using AutoMapper;
using WebApplication3.DTO;
using WebApplication3.Model;

namespace WebApplication3.Mappings
{
    public class MobileProfile : Profile
    {
        public MobileProfile()
        {
            CreateMap<CreateMobileRequestDto, CreateMobileDto>()
            .ForMember(dest => dest.MobileImage, opt => opt.Ignore()); // Ignore ImageFile

            CreateMap<UpdateMobileDto, CreateMobileDto>()
                .ForMember(dest => dest.MobileImage, opt => opt.Ignore()); // Ignore ImageFile

            CreateMap<MobileModel, MobileDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));

            CreateMap<MobileModel, MobileDetailsDto>()
                .ForMember(dest => dest.Features, opt => opt.Condition(src => src.Features != null)) // Only map Features if they exist
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features));


            CreateMap<CreateMobileDto, MobileModel>();
            CreateMap<UpdateMobileDto, MobileModel>();

            CreateMap<FeaturesModel, FeaturesCreateDTO>();
            CreateMap<FeaturesModel, FeaturesResponseDTO>();
            CreateMap<FeaturesCreateDTO, FeaturesModel>();



            // Cart
            CreateMap<CartAddDTO, CartModel>();

            CreateMap<CartModel, CartResponseDTO>()
                .ForMember(dest => dest.MobileName, opt => opt.MapFrom(src => src.Mobile != null ? src.Mobile.Name : string.Empty))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Mobile != null ? src.Mobile.Price : 0));

            // Order mappings
            CreateMap<CreateOrderDto, OrderModel>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<CreateOrderItemDto, OrderItemModel>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));

            CreateMap<OrderModel, OrderDetailsDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<OrderItemModel, OrderItemDto>();

            CreateMap<CreateOrderDtoFirst, CreateOrderDto>();

            // Order Item
            CreateMap<OrderItemModel, OrderItemDto>();
        }
    }
}
