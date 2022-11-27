using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Services.Abstract
{
    public interface IAdminAccountService
    {
        Task<bool> LoginAsync(AccountLoginVM model);
        Task LogOutAsync();
    }
}
