using AutoMapper;
using IdentityService.src.Application.DTOs.Requests.Permession;
using IdentityService.src.Application.DTOs.Requests.Role;
using IdentityService.src.Application.DTOs.Requests.User;
using IdentityService.src.Application.DTOs.Responses.Permessions;
using IdentityService.src.Application.DTOs.Responses.Roles;
using IdentityService.src.Application.DTOs.Responses.User;
using IdentityService.src.Domain.Entities;

namespace IdentityService.src.Application.Mapping
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