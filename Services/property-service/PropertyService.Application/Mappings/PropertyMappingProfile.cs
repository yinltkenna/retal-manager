using AutoMapper;
using PropertyService.Application.DTOs.Requests.Amenity;
using PropertyService.Application.DTOs.Requests.Branch;
using PropertyService.Application.DTOs.Requests.Reservation;
using PropertyService.Application.DTOs.Responses.RoomType;
using PropertyService.Application.DTOs.Requests.RoomPrice;
using PropertyService.Application.DTOs.Responses.RoomMaintenance;
using PropertyService.Application.DTOs.Responses.RoomStatusLog;
using PropertyService.Application.DTOs.Requests.Room;
using PropertyService.Application.DTOs.Responses.RoomImage;
using PropertyService.Application.DTOs.Requests.RoomType;
using PropertyService.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.Application.DTOs.Responses.Room;
using PropertyService.Application.DTOs.Responses.Branch;
using PropertyService.Application.DTOs.Responses.Reservation;
using PropertyService.Application.DTOs.Responses.RoomPrice;
using PropertyService.Application.DTOs.Responses.Amenity;
using PropertyService.Application.DTOs.Responses.RoomHistory;
using PropertyService.Application.DTOs.Requests.RoomImage;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Enums;

namespace PropertyService.Application.Mappings
{
    public class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            CreateMap<CreateBranchRequest, Branch>();
            CreateMap<UpdateBranchRequest, Branch>();
            CreateMap<Branch, BranchResponse>();

            CreateMap<CreateRoomTypeRequest, RoomType>();
            CreateMap<UpdateRoomTypeRequest, RoomType>();
            CreateMap<RoomType, RoomTypeResponse>();

            CreateMap<CreateRoomRequest, Room>();
            CreateMap<UpdateRoomRequest, Room>();
            CreateMap<Room, RoomResponse>();

            CreateMap<CreateRoomPriceRequest, RoomPrice>();
            CreateMap<UpdateRoomPriceRequest, RoomPrice>();
            CreateMap<RoomPrice, RoomPriceResponse>();

            CreateMap<CreateRoomImageRequest, RoomImage>();
            CreateMap<RoomImage, RoomImageResponse>();

            CreateMap<CreateAmenityRequest, Amenity>();
            CreateMap<UpdateAmenityRequest, Amenity>();
            CreateMap<Amenity, AmenityResponse>();

            

            CreateMap<CreateReservationRequest, Reservation>();
            CreateMap<Reservation, ReservationResponse>();

            CreateMap<RoomHistory, RoomHistoryResponse>();

            CreateMap<CreateRoomMaintenanceRequest, RoomMaintenance>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => MaintenanceStatus.Pending));
            CreateMap<UpdateRoomMaintenanceRequest, RoomMaintenance>();
            CreateMap<RoomMaintenance, RoomMaintenanceResponse>();

            CreateMap<RoomStatusLog, RoomStatusLogResponse>();
        }
    }
}
