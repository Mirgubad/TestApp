﻿using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Services.Abstract
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(AccountLoginVM model);
        Task LogOutAsync();
    }
}
