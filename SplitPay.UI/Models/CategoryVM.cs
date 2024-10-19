using SplitPay.DAL.Models;

namespace SplitPay.UI.Models
{
	public class CategoryVM
	{
        public int Id { get; set; }
        public string Name { get; set; }
		public int ParentId { get; set; }
		public string ParentCategory { get; set; }
        public List<CategoryVM> Categories { get; set; }
        public int Level { get; set; }
		public int BranchId { get; set; }
		public string BranchName { get; set; }
		public List<BranchVM> Branchs { get; set; }
		public int EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public List<Employee> Employees { get; set; }
        public string SearchCategory { get; set; }
    }
}
