using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitPay.DAL.Models;

namespace SplitPay.DAL.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasOne(x => x.Branch).WithMany(x => x.Categories).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Employee).WithMany(x => x.Categories).HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
