using EventContracts.Authorization.Permissions.TenancyService;

namespace EventContracts.Authorization.Definitions.PermissionDefinitions
{
    public static class TenancyPermissionDefinitions
    {
        public static IEnumerable<BasePermissionDefinition> Get()
        {
            // Tenant Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantViewId,
                Code = TenantPermissions.VIEW,
                Name = "View Tenant",
                Group = "Tenant",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantCreateId,
                Code = TenantPermissions.CREATE,
                Name = "Create Tenant",
                Group = "Tenant",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantUpdateId,
                Code = TenantPermissions.UPDATE,
                Name = "Update Tenant",
                Group = "Tenant",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantDeleteId,
                Code = TenantPermissions.DELETE,
                Name = "Delete Tenant",
                Group = "Tenant",
                SortOrder = 4
            };

            // Tenant Document
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantDocumentViewId,
                Code = TenantIdentityDocumentsPermissions.VIEW,
                Name = "View Tenant Identity Document",
                Group = "Tenant Document",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantDocumentCreateId,
                Code = TenantIdentityDocumentsPermissions.CREATE,
                Name = "Create Tenant Identity Document",
                Group = "Tenant Document",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantDocumentUpdateId,
                Code = TenantIdentityDocumentsPermissions.UPDATE,
                Name = "Update Tenant Identity Document",
                Group = "Tenant Document",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.TenantDocumentDeleteId,
                Code = TenantIdentityDocumentsPermissions.DELETE,
                Name = "Delete Tenant Identity Document",
                Group = "Tenant Document",
                SortOrder = 4
            };

            // Contract
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractViewId,
                Code = TenantContractPermissions.VIEW,
                Name = "View Tenant Contract",
                Group = "Tenant Contract",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractCreateId,
                Code = TenantContractPermissions.CREATE,
                Name = "Create Tenant Contract",
                Group = "Tenant Contract",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractUpdateId,
                Code = TenantContractPermissions.UPDATE,
                Name = "Update Tenant Contract",
                Group = "Tenant Contract",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractDeleteId,
                Code = TenantContractPermissions.DELETE,
                Name = "Delete Tenant Contract",
                Group = "Tenant Contract",
                SortOrder = 4
            };

            // Contract File
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractFileViewId,
                Code = TenantContractFilePermissions.VIEW,
                Name = "View Tenant Contract File",
                Group = "Tenant Contract",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractFileCreateId,
                Code = TenantContractFilePermissions.CREATE,
                Name = "Create Tenant Contract File",
                Group = "Tenant Contract File",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractFileDeleteId,
                Code = TenantContractFilePermissions.DELETE,
                Name = "Delete Tenant Contract File",
                Group = "Tenant Contract File",
                SortOrder = 3
            };

            // Contract Member
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractMemberViewId,
                Code = TenantContractMemberPermissions.VIEW,
                Name = "View Tenant Contract Number",
                Group = "Tenant Contract Number",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractMemberCreateId,
                Code = TenantContractMemberPermissions.CREATE,
                Name = "Create Tenant Contract Number",
                Group = "Tenant Contract Number",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractMemberDeleteId,
                Code = TenantContractMemberPermissions.DELETE,
                Name = "Delete Tenant Contract Number",
                Group = "Tenant Contract Number",
                SortOrder = 4
            };

            // Contract Extension
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractExtensionViewId,
                Code = TenantContractExtensionPermissions.VIEW,
                Name = "View Contract Extension",
                Group = "Tenant Contract Extension",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractExtensionCreateId,
                Code = TenantContractExtensionPermissions.CREATE,
                Name = "Create Contract Extension",
                Group = "Tenant Contract Extension",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractExtensionUpdateId,
                Code = TenantContractExtensionPermissions.UPDATE,
                Name = "Update Contract Extension",
                Group = "Tenant Contract Extension",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractExtensionDeleteId,
                Code = TenantContractExtensionPermissions.DELETE,
                Name = "Delete Contract Extension",
                Group = "Tenant Contract Extension",
                SortOrder = 4
            };

            // Contract Termination
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractTerminationViewId,
                Code = TenantTerminationPermissions.VIEW,
                Name = "View Contract Termination",
                Group = "Tenant Contract Termination",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractTerminationCreateId,
                Code = TenantTerminationPermissions.CREATE,
                Name = "Create Contract Termination",
                Group = "Tenant Contract Termination",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractTerminationUpdateId,
                Code = TenantTerminationPermissions.UPDATE,
                Name = "Update Contract Termination",
                Group = "Tenant Contract Termination",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractTerminationDeleteId,
                Code = TenantTerminationPermissions.DELETE,
                Name = "Delete Contract Termination",
                Group = "Tenant Contract Termination",
                SortOrder = 4
            };

            // Contract Deposit Transaction
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractDepositTransactionViewId,
                Code = TenantDepositTransactionPermission.VIEW,
                Name = "View Contract Deposit Transaction",
                Group = "Tenant Contract Deposit Transaction",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractDepositTransactionCreateId,
                Code = TenantDepositTransactionPermission.CREATE,
                Name = "Create Contract Deposit Transaction",
                Group = "Tenant Contract Deposit Transaction",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractDepositTransactionUpdateId,
                Code = TenantDepositTransactionPermission.UPDATE,
                Name = "Update Contract Deposit Transaction",
                Group = "Tenant Contract Deposit Transaction",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ContractDepositTransactionDeleteId,
                Code = TenantDepositTransactionPermission.DELETE,
                Name = "Delete Contract Deposit Transaction",
                Group = "Tenant Contract Deposit Transaction",
                SortOrder = 4
            };
        }   
    }
}
