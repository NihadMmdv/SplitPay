﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SplitPay.DAL.Models;
using SplitPay.DAL.Enums;
using SplitPay.DAL.Repository.Interface;
using SplitPay.UI.Enums;
using SplitPay.UI.Models;
using SplitPay.UI.Services.Interfaces;
using System.Linq;

namespace SplitPay.UI.Controllers
{
	public class LoansController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IGenericRepository<Loan> _loanRepository;
		private readonly IGenericRepository<LoanItem> _loanItemRepository;
		private readonly IGenericRepository<LoanDetail> _loanDetailRepository;
		private readonly IGenericRepository<Customer> _customerRepository;
		private readonly IGenericRepository<Product> _productRepository;
		private readonly IEmailService _emailService;
		public LoansController(IGenericRepository<Loan> loanRepository,
			UserManager<AppUser> userManager,
			IGenericRepository<Customer> customerRepository,
			IGenericRepository<Product> productRepository,
			IGenericRepository<LoanItem> loanItemRepository,
			IEmailService emailService,
			IGenericRepository<LoanDetail> loanDetailRepository)
		{
			_loanRepository = loanRepository;
			_userManager = userManager;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
			_loanItemRepository = loanItemRepository;
			_emailService = emailService;
			_loanDetailRepository = loanDetailRepository;
		}

		public async Task<IActionResult> Index()
		{
			List<LoanVM> models = new();
			string userId = _userManager.GetUserId(User);
			var customers = await _customerRepository.GetAll();
			var customer = customers.ToList().FirstOrDefault(c => c.AppUserId == userId);
			var loans = await _loanRepository.GetAll();
			var pendingLoans = loans.Where(l => l.CustomerId == customer.Id && l.Status == Status.Pending);
			var loanItems = await _loanItemRepository.GetAll();
			foreach (var loan in pendingLoans.ToList())
			{
				var loanItem = loanItems.FirstOrDefault(l => l.LoanId == loan.Id);
				models.Add(new()
				{
					Title = loan.Title,
					MonthlyPrice = loan.MonthlyPrice,
					TotalPrice = loan.TotalPrice,
					Terms = loan.Terms,
					Customer = customer,
					Status = loan.Status,
					Count = loanItem.Count,
					Price = loanItem.Price
				});
			}
			return View(models);
		}
		public async Task<IActionResult> Create(int? productId)
		{
			var userId = _userManager.GetUserId(User);
			var customer = (await _customerRepository.GetAll()).FirstOrDefault(c => c.AppUserId == userId);
			if (customer == null)
			{
				return RedirectToAction("Create", "Customers");
			}
			var product = await _productRepository.Get(productId ?? 0);
			LoanVM model = new()
			{
				CustomerId = customer.Id,
				Product = product,
				ProductId = product.Id,
				TermList = new()
			};
			foreach (var item in Enum.GetValues(typeof(Terms)))
			{
				int term = (int)item;
				model.TermList.Add(term);
			}
			ViewBag.BackUrl = Request.Headers["Referer"].ToString();
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Create(LoanVM model)
		{
			var userId = _userManager.GetUserId(User);
			var product = await _productRepository.Get(model.ProductId);
			decimal monthlyPrice = (product.Price / model.Terms) * (decimal)1.05;
			decimal totalPrice = monthlyPrice * model.Terms;
		
			Loan loan = new()
			{
				Title = $"{product.Name}-{product.Model}-{product.Brand}-{model.Terms}months-{model.CustomerId}",
				Terms = model.Terms,
				CustomerId = model.CustomerId,
				TotalPrice = totalPrice,
				MonthlyPrice = monthlyPrice,
				Status = Status.Pending
			};
			await _loanRepository.Add(loan);
			await _loanRepository.SaveAsync();
			int count = model.Count;
			decimal price = count * totalPrice;
			LoanItem loanItem = new()
			{
				LoanId = loan.Id,
				ProductId = model.ProductId,
				Count = count,
				Price = price
			};
			LoanDetail loanDetail = new()
			{
				LoanId = loan.Id,
				CurrentAmount = totalPrice,
			};
		
			var user = await _userManager.GetUserAsync(User);
			string url = Url.Action("Create", "Payments", new { userId = userId, loanId = loan.Id }, HttpContext.Request.Scheme);
			await _emailService.SendPaymentEmail(url, user.Email, loan);
			await _loanItemRepository.Add(loanItem);
			await _loanItemRepository.SaveAsync();
			await _loanDetailRepository.Add(loanDetail);
			await _loanDetailRepository.SaveAsync();
		
			return RedirectToAction("Details", "Products", new { Id = product.Id });
		}
		public async Task<IActionResult> Details(int Id)
		{
			var loan = await _loanRepository.Get(Id);
			var loanDetails = await _loanDetailRepository.GetAll();
			var loanDetail = loanDetails.FirstOrDefault(ld => ld.LoanId == loan.Id);
			LoanDetailVM model = new()
			{
				Loan = loan,
				CurrentAmount = loanDetail.CurrentAmount
			};
			return PartialView("Details", model);
		}
	}
}
