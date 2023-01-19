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
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore.Design;

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

            CreateMap<UpdateBookDto, Book>()
                    .ForMember(dest => dest.PublisherId,
                        opt => opt.MapFrom(src => src.IdPublisher));

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
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();

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
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<RegistrationUserDto, User>();
            CreateMap<LoginUserDto, User>();
            CreateMap<User, AuthorizedUserDto>();
            CreateMap<GoogleJsonWebSignature.Payload, User>()
                .ForMember(dest => dest.Login,
                    opt => opt.MapFrom(x => x.Email))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(x => x.Email))
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(x => x.EmailVerified))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(x => x.Name));

            // Warehouse
            CreateMap<CreateWarehouseDto, Warehouse>();
            CreateMap<UpdateWarehouseDto, Warehouse>();

            // WarehouseBook
            CreateMap<CreateWarehouseBookDto, WarehouseBook>();
            CreateMap<UpdateWarehouseBookDto, WarehouseBook>();
        }
    }
}
