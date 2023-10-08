// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Queries.User
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fsel.Common.ActionResults;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.Enums.ErrorCodes;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using ITRequest.Identity.Domain.Models.QueryModels;
    using MediatR;
    using Microsoft.AspNetCore.Identity;

    public class GetUserByRoleQuery : GetUserByRoleQueryModel, IRequest<MethodResult<UserModel>>
    {
    }

    public class GetUserByRoleQueryHandler : IRequestHandler<GetUserByRoleQuery, MethodResult<UserModel>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUserByRoleQueryHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<MethodResult<UserModel>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<UserModel>();
            var users = await _userManager.GetUsersInRoleAsync(request.Role.ToString());
            var user = users.FirstOrDefault();
            if (user == null)
            {
                methodResult.AddErrorBadRequest(nameof(EnumUserErrorCode.UserNotExist));
                return methodResult;
            }
            methodResult.Result = _mapper.Map<UserModel>(user);
            return methodResult;
        }
    }
}
