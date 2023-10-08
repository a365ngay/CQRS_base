// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Queries.Requests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Core.Base;
    using Fsel.Core.Base.BaseModels;
    using Fsel.Core.Extensions;
    using ITRequest.Shared.Enum;
    using ITRequest.WorkFlow.Application.Service.UserServices;
    using ITRequest.WorkFlow.Application.Service.UserServices.Models;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class SearchRequestsByApproverQuery : BaseQueryModel, IRequest<MethodResult<PagingItemsModel<SearchRequestsByApproverQueryModel>>>
    {
    }

    public class SearchRequestsByApproverQueryHandler : IRequestHandler<SearchRequestsByApproverQuery, MethodResult<PagingItemsModel<SearchRequestsByApproverQueryModel>>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly AuthContext _authContext;
        private readonly IUserService _userService;

        public SearchRequestsByApproverQueryHandler(IRequestRepository requestRepository, AuthContext authContext, IUserService userService)
        {
            _requestRepository = requestRepository;
            _authContext = authContext;
            _userService = userService;
        }

        public async Task<MethodResult<PagingItemsModel<SearchRequestsByApproverQueryModel>>> Handle(SearchRequestsByApproverQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<PagingItemsModel<SearchRequestsByApproverQueryModel>>();

            if (request.PageSize > 100)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            var currentUserId = _authContext.CurrentUserId;
            var requests = _requestRepository.Queryable.Include(p => p.Approvals).Where(o => o.Status != EnumRequestStatus.Done && o.Status != EnumRequestStatus.Reject).Select(x => new SearchRequestsByApproverQueryModel
            {
                Id = x.Id,
                IsApprove = x.IsApprove,
                Content = x.Content,
                Title = x.Title,
                CreatedDate = x.CreatedDate,
                Status = x.Status,
                CreatedUserId = x.CreatedUserId,
                ApproverId = x.Approvals.OrderByDescending(p => p.CreatedDate).Where(a => a.Status == EnumRequestStatus.Pending || a.Status == EnumRequestStatus.Doing).Select(n => n.ApproverId).FirstOrDefault(),
                Priority = x.Priority,
            });

            requests = requests.Where(p => p.ApproverId == currentUserId);

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                requests = requests.Where(m => !string.IsNullOrEmpty(m.Title) && m.Title.Contains(request.Keyword));
            }
            int totalItem = await requests.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            var lists = await requests
                    .ApplySortAndPaging(request)
                    .OrderByDescending(x => x.CreatedDate)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            if (lists.Any())
            {
                var userResult = await _userService.GetUsersByIdsAsync(new GetUsersByIdsQueryModel
                {
                    UserIds = lists.Select(p => p.CreatedUserId.ToString()).ToList()
                });
                if (!userResult.IsSuccessStatusCode)
                {
                    methodResult.AddError(userResult.Error);
                    return methodResult;
                }
                foreach (var item in lists)
                {
                    item.CreatedFullName = userResult.Content?.Result?.FirstOrDefault(p => p.Id == item.CreatedUserId)?.FullName;
                }
            }
            methodResult.Result = new PagingItemsModel<SearchRequestsByApproverQueryModel>(lists, request, totalItem);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
