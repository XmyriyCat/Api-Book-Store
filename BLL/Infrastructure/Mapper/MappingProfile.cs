using AutoMapper;
using BLL.DTO.Author;
using BLL.DTO.Book;
using BLL.DTO.Delivery;
using BLL.DTO.Genre;
using BLL.DTO.Order;
using BLL.DTO.OrderLine;
using BLL.DTO.PaymentWay;
using BLL.DTO.Publisher;
using BLL.DTO.Role;
using BLL.DTO.Shipment;
using BLL.DTO.User;
using BLL.DTO.Warehouse;
using BLL.DTO.WarehouseBook;
using DLL.Models;

namespace BLL.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Author
            CreateMap<CreateAuthorDto, Author>();
            CreateMap<UpdateAuthorDto, Author>();

            // Book
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.PublisherId,
                    opt => opt.MapFrom(src => src.IdPublisher));
                //.ForMember(dest => dest.Genres,
                //    opt => opt.MapFrom(src => src.GenresId.Select(genreId => new Genre { Id = genreId })))
                //.ForMember(dest => dest.Authors,
                //    opt => opt.MapFrom(src => src.AuthorsId.Select(authorId => new Author { Id = authorId })));
                CreateMap<UpdateBookDto, Book>()
                    .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.IdPublisher));
                //.ForMember(dest => dest.Genres,
                //    opt => opt.MapFrom(src => src.GenresId.Select(genreId => new Genre { Id = genreId })))
                //.ForMember(dest => dest.Authors,
                //    opt => opt.MapFrom(src => src.AuthorsId.Select(authorId => new Author { Id = authorId })));

            // Delivery
            CreateMap<CreateDeliveryDto, Delivery>();
            CreateMap<UpdateDeliveryDto, Delivery>();

            // Genre
            CreateMap<CreateGenreDto, Genre>();
            CreateMap<UpdateGenreDto, Genre>();

            // OrderLine
            CreateMap<CreateOrderLineDto, OrderLine>();
            CreateMap<UpdateOrderLineDto, OrderLine>();

            // Orders
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.OrderLine,
                    opt => opt.MapFrom(src => src.OrderLineId.Select(orderLineId => new OrderLine { Id = orderLineId })));
            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.OrderLine,
                    opt => opt.MapFrom(src => src.OrderLineId.Select(orderLineId => new OrderLine { Id = orderLineId })));

            // PaymentWay
            CreateMap<CreatePaymentWayDto, PaymentWay>();
            CreateMap<UpdatePaymentWayDto, PaymentWay>();

            // Publisher
            CreateMap<CreatePublisherDto, Publisher>();
            CreateMap<UpdatePublisherDto, Publisher>();

            // Role
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();

            // Shipment
            CreateMap<CreateShipmentDto, Shipment>();
            CreateMap<UpdateShipmentDto, Shipment>();

            // User
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.RolesIds.Select(roleId => new Role { Id = roleId })));
            CreateMap<UpdateUserDto, User>()
                .ForMember(desc => desc.Roles,
                    opt => opt.MapFrom(src => src.RolesIds.Select(roleId => new Role { Id = roleId })));

            // Warehouse
            CreateMap<CreateWarehouseDto, Warehouse>();
            CreateMap<UpdateWarehouseDto, Warehouse>();

            // WarehouseBook
            CreateMap<CreateWarehouseBookDto, WarehouseBook>();
            CreateMap<UpdateWarehouseBookDto, WarehouseBook>();
        }
    }
}
