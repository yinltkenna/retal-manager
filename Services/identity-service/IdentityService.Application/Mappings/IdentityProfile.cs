using AutoMapper;
using IdentityService.Application.DTOs.Requests.Permession;
using IdentityService.Application.DTOs.Requests.Role;
using IdentityService.Application.DTOs.Requests.User;
using IdentityService.Application.DTOs.Responses.Permessions;
using IdentityService.Application.DTOs.Responses.Roles;
using IdentityService.Application.DTOs.Responses.User;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mappings
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            // user mappings
            CreateMap<User, UserDetailResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<User, UserProfileResponse>();
            CreateMap<User, UserListResponse>();

            // requests -> entity
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.UserPhoneNumber));
            CreateMap<UpdateUserRequest, User>();

            // role mappings
            CreateMap<Role, RoleDetailResponse>();
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();

            // permission mappings
            CreateMap<Permission, PermissionResponse>();
            CreateMap<Permission, PermissionTreeResponse>();
            CreateMap<CreatePermissionRequest, Permission>();
            CreateMap<UpdatePermissionRequest, Permission>();
        }
    }
}