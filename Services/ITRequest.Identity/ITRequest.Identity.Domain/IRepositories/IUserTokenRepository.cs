// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.IRepositories
{
    using System.Threading.Tasks;
    using ITRequest.Identity.Domain.Entities;

    public interface IUserTokenRepository
    {
        Task<UserToken?> GetByRefreshTokenAsync(string? refreshToken);

        Task<UserToken> AddAsync(UserToken userToken);

        Task<UserToken> Remove(UserToken userToken);

        public bool CheckFirstLogin(UserToken userToken);

    }
}
