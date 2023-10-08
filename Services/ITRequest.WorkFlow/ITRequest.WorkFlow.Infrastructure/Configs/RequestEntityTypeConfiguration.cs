// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Infrastructure.Configs
{
    using Fsel.Common.Helpers;
    using ITRequest.Shared.Enum;
    using ITRequest.WorkFlow.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RequestEntityTypeConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.Property(e => e.Type)
               .HasMaxLength(100)
               .HasConversion(
                   v => v.ToString(),
                   v => v.EnumParse<EnumRequestType>());
            builder.Property(e => e.Status)
             .HasMaxLength(100)
             .HasConversion(
                 v => v.ToString(),
                 v => v.EnumParse<EnumRequestStatus>());
            builder.Property(e => e.Priority)
            .HasMaxLength(100)
            .HasConversion(
                v => v.ToString(),
                v => v.EnumParse<EnumPriority>());
        }
    }
}
