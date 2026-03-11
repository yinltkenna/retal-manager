namespace IdentityService.src.Application.Interfaces
{
    public interface IPermissionChecker
    {
        Task<bool> HasPermissionAsync(string permission);
    }
}
