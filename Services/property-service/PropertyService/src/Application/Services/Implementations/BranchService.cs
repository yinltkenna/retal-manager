using AutoMapper;
using PropertyService.src.Application.DTOs.Requests.Branch;
using PropertyService.src.Application.DTOs.Responses.Branch;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Domain.Entities;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Common.TemplateResponses;

namespace PropertyService.src.Application.Services.Implementations
{
    public class BranchService(IUnitOfWork unitOfWork,
                               IMapper mapper) : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<BranchResponse>> CreateAsync(CreateBranchRequest request)
        {
            var entity = _mapper.Map<Branch>(request);
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Branch>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<BranchResponse>.SuccessResponse(_mapper.Map<BranchResponse>(entity));
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<bool>.FailResponse("Branch not found");

            // Soft delete, in entity have IsDeleted property, set it to true
            await _unitOfWork.Repository<Branch>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true);
        }

        public async Task<ApiResponse<List<BranchResponse>>> GetAllAsync()
        {
            var list = await _unitOfWork.Repository<Branch>().ListAsync();
            return ApiResponse<List<BranchResponse>>.SuccessResponse(_mapper.Map<List<BranchResponse>>(list));
        }

        public async Task<ApiResponse<BranchResponse>> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<BranchResponse>.FailResponse("Branch not found");

            return ApiResponse<BranchResponse>.SuccessResponse(_mapper.Map<BranchResponse>(entity));
        }

        public async Task<ApiResponse<BranchResponse>> UpdateAsync(Guid id, UpdateBranchRequest request)
        {
            var entity = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);
            if (entity == null)
                return ApiResponse<BranchResponse>.FailResponse("Branch not found");

            _mapper.Map(request, entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Repository<Branch>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<BranchResponse>.SuccessResponse(_mapper.Map<BranchResponse>(entity));
        }
    }
}
