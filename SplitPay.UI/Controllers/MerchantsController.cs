﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SplitPay.DAL.Models;
using SplitPay.DAL.Repository.Interface;
using SplitPay.UI.Models;

namespace SplitPay.UI.Controllers
{
  
    public class MerchantsController : Controller
    {
        private readonly IGenericRepository<Merchant> _merchantRepository;
        private readonly UserManager<AppUser> _userManager;

		public MerchantsController(IGenericRepository<Merchant> merchantRepository,
            UserManager<AppUser> userManager)
		{
			_merchantRepository = merchantRepository;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
        {
            List<MerchantVM> models = new();
            var merchants = await _merchantRepository.GetAll();
            foreach (var merchant in merchants)
            {
                models.Add(new()
                {
                    Id = merchant.Id,
                    Name = merchant.Name,
                    Description = merchant.Description,
                });
            }
            return View(models);
        }
		[Authorize(Roles = "Admin")]
		public IActionResult Create()
        {
            string userId = _userManager.GetUserId(User);
            MerchantVM model = new()
            {
                AppUserId=userId
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(MerchantVM model)
        {
                Merchant merchant = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    TerminalNo = model.TerminalNo,
                    AppUserId = model.AppUserId,
                };
                await _merchantRepository.Add(merchant);
                await _merchantRepository.SaveAsync();
                return RedirectToAction("Index", "Home");
           
        }
        
    }
}
