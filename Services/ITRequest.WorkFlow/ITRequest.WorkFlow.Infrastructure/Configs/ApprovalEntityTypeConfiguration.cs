// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Infrastructure.Configs
{
    using Fsel.Common.Helpers;
    using ITRequest.Shared.Enum;
    using ITRequest.WorkFlow.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ApprovalEntityTypeConfiguration : IEntityTypeConfiguration<Approval>
    {
        public void Configure(EntityTypeBuilder<Approval> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.HasOne(a => a.Request)
                .WithMany(b => b.Approvals)
                .HasForeignKey(b => b.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(e => e.Status)
              .HasMaxLength(100)
              .HasConversion(
                  v => v.ToString(),
                  v => v.EnumParse<EnumRequestStatus>());
        }
    }
}
