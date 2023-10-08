// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Queries.User
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Enums.ErrorCodes;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class GetUserByIdQuery : IRequest<MethodResult<UserModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, MethodResult<UserModel>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetUserByIdQueryHandler(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<MethodResult<UserModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<UserModel>();

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                methodResult.AddErrorBadRequest(nameof(EnumSystemErrorCode.DataNotExist), nameof(request.Id));
                return methodResult;
            }
            var userModel = _mapper.Map<UserModel>(user);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                methodResult.AddErrorBadRequest(nameof(EnumSystemErrorCode.DataNotExist), nameof(request.Id));
                return methodResult;
            }
            userModel!.Role = roles.FirstOrDefault();

            methodResult.Result = userModel;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
