﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SplitPay.DAL.Models;
using SplitPay.DAL.Repository.Interface;
using SplitPay.UI.Models;

namespace SplitPay.UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin,Employee")]
	public class CategoriesController : Controller
	{
		private readonly IGenericRepository<Category> _categoryRepository;
		private readonly IGenericRepository<Branch> _branchRepository;
		private readonly IGenericRepository<Employee> _employeeRepository;

		public CategoriesController(IGenericRepository<Branch> branchRepository,
			UserManager<AppUser> userManager,
			IGenericRepository<Employee> employeeRepository,
			IGenericRepository<Category> categoryRepository)
		{
			_branchRepository = branchRepository;
			_employeeRepository = employeeRepository;
			_categoryRepository = categoryRepository;
		}

		public async Task<IActionResult> Index()
		{
			var categories = (await _categoryRepository.GetAll()).ToList();
			List<CategoryVM> models = new();
			foreach (var model in categories)
			{
				var branch = await _branchRepository.Get(model.BranchId);
				var employee = await _employeeRepository.Get(model.EmployeeId);
				var parentCategory = await _categoryRepository.Get(model.ParentId);
				models.Add(new()
				{
					Id = model.Id,
					Name = model.Name,
					BranchName = branch.Name,
					EmployeeName = employee.Name,
					Level = model.Level,
					ParentCategory = model.ParentId != 0 ? parentCategory.Name : "No parent category",

				});
			}
			return View(models);
		}
		public async Task<IActionResult> Create()
		{
			var branchs = await _branchRepository.GetAll();
			var categories = await _categoryRepository.GetAll();
			var employees = await _employeeRepository.GetAll();
			CategoryVM model = new()
			{
				Branchs = branchs.ToList(),
				Employees = employees.ToList(),
				Categories = categories.ToList(),
			};
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Create(CategoryVM model)
		{
			Category category = new()
			{
				Name = model.Name,
				Level = model.Level,
				ParentId = model.ParentId,
				BranchId = model.BranchId,
				EmployeeId = model.EmployeeId,
				UpdatedDate = DateTime.Now,
			};
			await _categoryRepository.Add(category);
			await _categoryRepository.SaveAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int Id)
		{
			var branchs = await _branchRepository.GetAll();
			var categories = await _categoryRepository.GetAll();
			var employees = await _employeeRepository.GetAll();
			var category = await _categoryRepository.Get(Id);
			CategoryVM model = new()
			{
				Name = category.Name,
				Level = category.Level,
				ParentId = category.ParentId,
				BranchId = category.BranchId,
				EmployeeId = category.EmployeeId,
				Branchs = branchs.ToList(),
				Employees = employees.ToList(),
				Categories = categories.ToList(),
			};
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(CategoryVM model, int id)
		{
			var category = await _categoryRepository.Get(id);
			category.Name = model.Name;
			category.Level = model.Level;
			category.ParentId = model.ParentId;
			category.BranchId = model.BranchId;
			category.EmployeeId = model.EmployeeId;
			_categoryRepository.Update(category);
			_categoryRepository.Save();
			return RedirectToAction("Index");
		}
	}
}
