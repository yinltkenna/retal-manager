using AutoMapper;
using EventContracts.Authorization.Permissions.PropertyService;
using PropertyService.Application.DTOs.Requests.Amenity;
using PropertyService.Application.DTOs.Responses.Amenity;
using PropertyService.Application.Interfaces;
using PropertyService.Application.TemplateResponses;
using PropertyService.Domain.Entities;
using PropertyService.Domain.Interfaces;

namespace PropertyService.Application.Services
{
    public class AmenityService(IUnitOfWork unitOfWork,
                               IMapper mapper,
                               IPermissionChecker permissionChecker) : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPermissionChecker _permissionChecker = permissionChecker;

        public async Task<ApiResponse<AmenityResponse>> CreateAsync(CreateAmenityRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(AmenityPermissions.CREATE))
                return ApiResponse<AmenityResponse>.FailResponse("Unauthorized");

            var entity = _mapper.Map<Amenity>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Amenity>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<AmenityResponse>.SuccessResponse(_mapper.Map<AmenityResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(AmenityPermissions.DELETE))
                return ApiResponse<bool>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Amenity not found");

            _unitOfWork.Repository<Amenity>().Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<AmenityResponse>>> GetAllAsync()
        {
            if (!await _permissionChecker.HasPermissionAsync(AmenityPermissions.VIEW))
                return ApiResponse<List<AmenityResponse>>.FailResponse("Unauthorized");

            var list = await _unitOfWork.Repository<Amenity>().ListAsync();
            return ApiResponse<List<AmenityResponse>>.SuccessResponse(_mapper.Map<List<AmenityResponse>>(list));
        }

        public async Task<ApiResponse<AmenityResponse>> GetByIdAsync(Guid id)
        {
            if (!await _permissionChecker.HasPermissionAsync(AmenityPermissions.VIEW))
                return ApiResponse<AmenityResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<AmenityResponse>.FailResponse("Amenity not found");

            return ApiResponse<AmenityResponse>.SuccessResponse(_mapper.Map<AmenityResponse>(entity));
        }

        public async Task<ApiResponse<AmenityResponse>> UpdateAsync(Guid id, UpdateAmenityRequest request)
        {
            if (!await _permissionChecker.HasPermissionAsync(AmenityPermissions.UPDATE))
                return ApiResponse<AmenityResponse>.FailResponse("Unauthorized");

            var entity = await _unitOfWork.Repository<Amenity>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<AmenityResponse>.FailResponse("Amenity not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Repository<Amenity>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<AmenityResponse>.SuccessResponse(_mapper.Map<AmenityResponse>(entity));
        }
    }
}
