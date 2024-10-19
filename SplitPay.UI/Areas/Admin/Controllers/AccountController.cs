using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SplitPay.DAL.Models;
using SplitPay.UI.Models;

namespace SplitPay.UI.Areas.Admin.Controllers
{
	[Area("admin")]
	[Authorize(Roles ="Admin,Employee")]
	public class AccountController : Controller
	{
		private readonly SignInManager<AppUser> _signInmanagerManager;
		private readonly UserManager<AppUser> _userManager;
		public AccountController(SignInManager<AppUser> signInmanagerManager, UserManager<AppUser> userManager)
		{
			_signInmanagerManager = signInmanagerManager;
			_userManager = userManager;
		}

		public IActionResult LogIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> LogIn(LogInVM model)
		{

			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					var signInResult = await _signInmanagerManager.PasswordSignInAsync(user, model.Password, false, false);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Email or Password wrong");
				}
			}
			return View(model);
		}
	}
}
