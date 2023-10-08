// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Infrastructure.Configs
{
    using ITRequest.Identity.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserEnityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
        }
    }
}
