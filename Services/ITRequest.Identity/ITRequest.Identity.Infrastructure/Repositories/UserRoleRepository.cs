// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Infrastructure.Repositories
{
    using ITRequest.Identity.Domain.IRepositories;
    using Microsoft.AspNetCore.Identity;

    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserRoleRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public virtual IQueryable<IdentityUserRole<string>> GetQuery()
        {
            try
            {
                return _userDbContext.UserRoles.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
