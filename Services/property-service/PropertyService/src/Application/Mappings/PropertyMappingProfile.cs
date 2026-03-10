using AutoMapper;
using PropertyService.src.Application.DTOs.Requests.Amenity;
using PropertyService.src.Application.DTOs.Requests.Branch;
using PropertyService.src.Application.DTOs.Requests.Room;
using PropertyService.src.Application.DTOs.Requests.RoomFacility;
using PropertyService.src.Application.DTOs.Requests.RoomImage;
using PropertyService.src.Application.DTOs.Requests.RoomPrice;
using PropertyService.src.Application.DTOs.Requests.Reservation;
using PropertyService.src.Application.DTOs.Requests.RoomHistory;
using PropertyService.src.Application.DTOs.Requests.RoomMaintenance;
using PropertyService.src.Application.DTOs.Requests.RoomType;
using PropertyService.src.Application.DTOs.Responses.Amenity;
using PropertyService.src.Application.DTOs.Responses.Reservation;
using PropertyService.src.Application.DTOs.Responses.RoomHistory;
using PropertyService.src.Application.DTOs.Responses.RoomMaintenance;
using PropertyService.src.Application.DTOs.Responses.RoomStatusLog;
using PropertyService.src.Application.DTOs.Responses.Branch;
using PropertyService.src.Application.DTOs.Responses.Room;
using PropertyService.src.Application.DTOs.Responses.RoomFacility;
using PropertyService.src.Application.DTOs.Responses.RoomImage;
using PropertyService.src.Application.DTOs.Responses.RoomPrice;
using PropertyService.src.Application.DTOs.Responses.RoomType;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Domain.Enums;

namespace PropertyService.src.Application.Mappings
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
