using AutoMapper;
using TenancyService.src.Application.DTOs.Requests.Contract;
using TenancyService.src.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.src.Application.DTOs.Requests.ContractExtension;
using TenancyService.src.Application.DTOs.Requests.ContractFile;
using TenancyService.src.Application.DTOs.Requests.ContractTermination;
using TenancyService.src.Application.DTOs.Requests.Tenant;
using TenancyService.src.Application.DTOs.Requests.TenantIdentityDocument;
using TenancyService.src.Application.DTOs.Responses.Contract;
using TenancyService.src.Application.DTOs.Responses.ContractDepositTransaction;
using TenancyService.src.Application.DTOs.Responses.ContractExtension;
using TenancyService.src.Application.DTOs.Responses.ContractFile;
using TenancyService.src.Application.DTOs.Responses.ContractTermination;
using TenancyService.src.Application.DTOs.Responses.Tenant;
using TenancyService.src.Application.DTOs.Responses.TenantIdentityDocument;
using TenancyService.src.Domain.Entities;

namespace TenancyService.src.Application.Mappings
{
    public class TenancyMappingProfile : Profile
    {
        public TenancyMappingProfile()
        {
            CreateMap<CreateTenantRequest, Tenant>();
            CreateMap<UpdateTenantRequest, Tenant>();
            CreateMap<Tenant, TenantResponse>();

            CreateMap<CreateTenantIdentityDocumentRequest, TenantIdentityDocument>();
            CreateMap<UpdateTenantIdentityDocumentRequest, TenantIdentityDocument>();
            CreateMap<TenantIdentityDocument, TenantIdentityDocumentResponse>();

            CreateMap<CreateContractRequest, Contract>();
            CreateMap<UpdateContractRequest, Contract>();
            CreateMap<Contract, ContractResponse>();

            CreateMap<AddContractFileRequest, ContractFile>();
            CreateMap<ContractFile, ContractFileResponse>();

            CreateMap<CreateContractExtensionRequest, ContractExtension>();
            CreateMap<ContractExtension, ContractExtensionResponse>();

            CreateMap<CreateContractTerminationRequest, ContractTermination>();
            CreateMap<ContractTermination, ContractTerminationResponse>();

            CreateMap<CreateContractDepositTransactionRequest, ContractDepositTransaction>();
            CreateMap<ContractDepositTransaction, ContractDepositTransactionResponse>();
        }
    }
}
