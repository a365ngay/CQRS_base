// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Service.UserServices
{
    using Fsel.Common.ActionResults;
    using ITRequest.WorkFlow.Application.Service.UserServices.Models;
    using Microsoft.AspNetCore.Mvc;
    using Refit;

    public interface IUserService
    {
        [Post("/user/get-by-ids")]
        Task<IApiResponse<MethodResult<List<UserModel>>>> GetUsersByIdsAsync([Body] GetUsersByIdsQueryModel model);

        [Post("/user/get-user-by-role")]
        Task<IApiResponse<MethodResult<UserModel>>> GetUserByRoleAsync([Body] GetUserByRoleQueryModel role);

        [Get("/user/get-by-id/{id}")]
        Task<IApiResponse<MethodResult<UserModel>>> GetUserByIdAsync(Guid id);
    }
}
