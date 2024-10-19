using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitPay.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitPay.DAL.Config
{
	public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.HasOne(e => e.Branch).WithMany(b => b.Employees).HasForeignKey(e => e.BranchId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
