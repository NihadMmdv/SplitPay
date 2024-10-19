using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitPay.DAL.Models;
using SplitPay.DAL.Repository.Interface;
using SplitPay.UI.Models;

namespace OMMS.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IGenericRepository<Payment> _paymentRepository;

		public PaymentController(IGenericRepository<Payment> paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}

		public async Task<IActionResult> Create(PaymentVM model)
		{
			if (ModelState.IsValid)
			{
				Payment payment = new()
				{
					
				};
				await _paymentRepository.Add(payment);
				await _paymentRepository.SaveAsync();
			}
			return Ok();
		}
	}
}
