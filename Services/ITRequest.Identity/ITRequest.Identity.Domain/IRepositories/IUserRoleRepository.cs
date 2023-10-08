// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.IRepositories
{
    using System.Linq;
    using Microsoft.AspNetCore.Identity;

    public interface IUserRoleRepository
    {
        IQueryable<IdentityUserRole<string>> GetQuery();
    }
}
