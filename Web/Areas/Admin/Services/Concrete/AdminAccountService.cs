using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Areas.Admin.Services.Abstract;
using Web.Areas.Admin.ViewModels;

namespace Web.Areas.Admin.Services.Concrete
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ModelStateDictionary _modelState;
        public AdminAccountService(SignInManager<User> signInManager,
          UserManager<User> userManager, IActionContextAccessor actionContextAccessor
          )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }
        public async Task<bool> LoginAsync(AccountLoginVM model)
        {
            if (!_modelState.IsValid) return false;
            var userAdmin = await _userManager.FindByNameAsync(model.Username);
            if (userAdmin == null)
            {
                _modelState.AddModelError(string.Empty, "Username or password was wrong");
                return false;
            }
            if (!await _userManager.IsInRoleAsync(userAdmin, UserRoles.Admin.ToString()))
            {
                _modelState.AddModelError(string.Empty, "Username or password was wrong");
                return false;
            }
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!result.Succeeded)
            {
                _modelState.AddModelError(string.Empty, "Username or password was wrong");
                return false;
            }
            return true;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
