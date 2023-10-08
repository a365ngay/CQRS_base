// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Infrastructure.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.IRepositories;
    using Microsoft.EntityFrameworkCore;

    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly UserDbContext _userDbContext;

        public UserTokenRepository(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public virtual async Task<UserToken?> GetByRefreshTokenAsync(string? refreshToken)
        {
            try
            {
                return await _userDbContext.UserTokens.SingleOrDefaultAsync(x => x.RefreshToken == refreshToken).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<UserToken> AddAsync(UserToken userToken)
        {
            try
            {
                userToken = (await _userDbContext.UserTokens.AddAsync(userToken)).Entity;
                await _userDbContext.SaveChangesAsync();
                return userToken;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<UserToken> Remove(UserToken userToken)
        {
            try
            {
                _userDbContext.UserTokens.Remove(userToken);
                await _userDbContext.SaveChangesAsync();
                return userToken;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool CheckFirstLogin(UserToken userToken)
        {
            if (userToken == null)
            {
                return false;
            }
            try
            {
                return _userDbContext.UserTokens.Any(x => x.UserId == userToken.UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
