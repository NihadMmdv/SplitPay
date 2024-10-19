using SplitPay.DAL.Models;

namespace SplitPay.UI.Models
{
	public class LoanDetailVM
	{
        public int Id { get; set; }
        public int LoanId { get; set; }
        public Loan Loan { get; set; }
        public decimal CurrentAmount { get; set; }
    }
}
