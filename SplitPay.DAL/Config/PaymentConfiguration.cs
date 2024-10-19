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
	public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{
			builder.HasOne(x => x.Loan).WithMany(x => x.Payments).HasForeignKey(x => x.LoanId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.Customer).WithMany(x => x.Payments).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
