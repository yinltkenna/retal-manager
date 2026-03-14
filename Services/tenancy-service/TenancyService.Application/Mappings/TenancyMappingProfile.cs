using AutoMapper;
using TenancyService.Application.DTOs.Requests.Contract;
using TenancyService.Application.DTOs.Requests.ContractDepositTransaction;
using TenancyService.Application.DTOs.Requests.ContractExtension;
using TenancyService.Application.DTOs.Requests.ContractFile;
using TenancyService.Application.DTOs.Requests.ContractTermination;
using TenancyService.Application.DTOs.Requests.Tenant;
using TenancyService.Application.DTOs.Requests.TenantIdentityDocument;
using TenancyService.Application.DTOs.Responses.Contract;
using TenancyService.Application.DTOs.Responses.ContractDepositTransaction;
using TenancyService.Application.DTOs.Responses.ContractExtension;
using TenancyService.Application.DTOs.Responses.ContractFile;
using TenancyService.Application.DTOs.Responses.ContractTermination;
using TenancyService.Application.DTOs.Responses.Tenant;
using TenancyService.Application.DTOs.Responses.TenantIdentityDocument;
using TenancyService.Domain.Entities;

namespace TenancyService.Application.Mappings
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
