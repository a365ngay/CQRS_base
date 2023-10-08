// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Queries.Approvals
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Core.Base;
    using Fsel.Core.Base.BaseModels;
    using Fsel.Core.Extensions;
    using ITRequest.WorkFlow.Application.Service.UserServices;
    using ITRequest.WorkFlow.Application.Service.UserServices.Models;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using ITRequest.WorkFlow.Domain.Models.QueryModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class SearchApprovalByRequestIdQuery : SearchApprovalByRequestIdQueryModel, IRequest<MethodResult<PagingItemsModel<ApprovalModel>>>
    {
    }

    public class SearchApprovalByRequestIdQueryHandler : IRequestHandler<SearchApprovalByRequestIdQuery, MethodResult<PagingItemsModel<ApprovalModel>>>
    {
        private readonly IApprovalRepository _approvalRepository;
        private readonly AuthContext _authContext;
        private readonly IUserService _userService;

        public SearchApprovalByRequestIdQueryHandler(IApprovalRepository approvalRepository, AuthContext authContext, IUserService userService)
        {
            _approvalRepository = approvalRepository;
            _authContext = authContext;
            _userService = userService;
        }

        public async Task<MethodResult<PagingItemsModel<ApprovalModel>>> Handle(SearchApprovalByRequestIdQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<PagingItemsModel<ApprovalModel>>();

            if (request.PageSize > 100)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            var approvals = _approvalRepository.Queryable
                        .Where(x => x.RequestId == request.RequestId)
                        .Select(x => new ApprovalModel
                        {
                            RequestId = x.RequestId,
                            Id = x.Id,
                            ApprovalDate = x.ApprovalDate,
                            ApprovalLevel = x.ApprovalLevel,
                            CreatedDate = x.CreatedDate,
                            Note = x.Note,
                            Status = x.Status,
                            CreatedUserId = x.CreatedUserId,
                            CreatedFullName = x.CreatedFullName,
                            ApproverId = x.ApproverId ?? default,
                        });

            int totalItem = await approvals.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            var lists = await approvals
                    .ApplySortAndPaging(request)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

            var userIds = new List<string>();
            userIds.AddRange(lists.Select(p => p.ApproverId.ToString()).ToList());
            userIds.AddRange(lists.Select(p => p.CreatedUserId.ToString()).ToList());

            var userResult = await _userService.GetUsersByIdsAsync(new GetUsersByIdsQueryModel
            {
                UserIds = userIds
            });

            foreach (var item in lists)
            {
                item.ApproverName = userResult.Content?.Result?.FirstOrDefault(p => p.Id == item.ApproverId)?.FullName;
                item.CreatedFullName = userResult.Content?.Result?.FirstOrDefault(p => p.Id == item.CreatedUserId)?.FullName;
                item.ApproverDepartment = userResult.Content?.Result?.FirstOrDefault(p => p.Id == item.ApproverId)?.Department;
                item.ApproverPosition = userResult.Content?.Result?.FirstOrDefault(p => p.Id == item.ApproverId)?.Position;
            }
            methodResult.Result = new PagingItemsModel<ApprovalModel>(lists, request, totalItem);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
