using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitPay.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SplitPay.DAL.Config
{
	public class LoanItemConfiguration : IEntityTypeConfiguration<LoanItem>
	{
		public void Configure(EntityTypeBuilder<LoanItem> builder)
		{
			builder.HasOne(li => li.Product).WithMany(p => p.LoanItems).HasForeignKey(li => li.ProductId).OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(li => li.Product).WithMany(p => p.LoanItems).HasForeignKey(li => li.ProductId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
