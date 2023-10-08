// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Queries.User
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fsel.Common.ActionResults;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.Enums.ErrorCodes;
    using ITRequest.Identity.Domain.IRepositories;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using ITRequest.Identity.Domain.Models.QueryModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class GetUsersByIdsQuery : GetUsersByIdsQueryModel, IRequest<MethodResult<List<UserModel>>>
    {
    }

    public class GetUsersByIdsQueryHandler : IRequestHandler<GetUsersByIdsQuery, MethodResult<List<UserModel>>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserRoleRepository _userRoleRepository;

        public GetUsersByIdsQueryHandler(IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager, IUserRoleRepository userRoleRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<MethodResult<List<UserModel>>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<List<UserModel>>();

            if (request.UserIds == null || request.UserIds.Count == 0)
            {
                methodResult.AddErrorBadRequest(nameof(EnumUserErrorCode.IdsNull), nameof(request.UserIds), request.UserIds);
                return methodResult;
            }

            var query = from u in _userManager.Users
                        join ur in _userRoleRepository.GetQuery() on u.Id equals ur.UserId
                        join r in _roleManager.Roles on ur.RoleId equals r.Id
                        where request.UserIds.Contains(u.Id)
                        select new { u, r };

            var queryResult = await query.ToListAsync(cancellationToken);
            var users = queryResult.Select(x =>
            {
                var item = _mapper.Map<UserModel>(x.u);
                item.Role = x.r.Name;
                return item;
            }).ToList();

            methodResult.Result = users;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
