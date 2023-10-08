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
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using ITRequest.WorkFlow.Domain.Models.QueryModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class SearchRequestsByUserQuery : SearchRequestsByUserQueryModel, IRequest<MethodResult<PagingItemsModel<SearchRequestsByUserModel>>>
    {
    }

    public class SearchRequestsByUserQueryHandler : IRequestHandler<SearchRequestsByUserQuery, MethodResult<PagingItemsModel<SearchRequestsByUserModel>>>
    {
        private readonly AuthContext _authContext;
        private readonly IRequestRepository _requestRepository;

        public SearchRequestsByUserQueryHandler(AuthContext authContext, IRequestRepository requestRepository)
        {
            _authContext = authContext;
            _requestRepository = requestRepository;
        }

        public async Task<MethodResult<PagingItemsModel<SearchRequestsByUserModel>>> Handle(SearchRequestsByUserQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<PagingItemsModel<SearchRequestsByUserModel>>();

            if (request.PageSize > 100)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }
            var currentUserId = _authContext.CurrentUserId;
            var requests = _requestRepository.Queryable.Include(a => a.Approvals)
            .Where(p => p.CreatedUserId == currentUserId).Select(x => new SearchRequestsByUserModel
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedDate = x.CreatedDate,
                Status = x.Status,
                Priority = x.Priority,
            });

            if (request.Status == EnumRequestStatus.Done || request.Status == EnumRequestStatus.Reject)
            {
                requests = requests.Where(p => p.Status == EnumRequestStatus.Done || p.Status == EnumRequestStatus.Reject);
            }
            else
            {
                requests = requests.Where(p => p.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                requests = requests.Where(p => !string.IsNullOrEmpty(p.Title) && p.Title.Contains(request.Keyword));
            }

            int totalItem = await requests.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            var lists = await requests
                    .ApplySortAndPaging(request)
                    .OrderByDescending(x => x.CreatedDate)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

            methodResult.Result = new PagingItemsModel<SearchRequestsByUserModel>(lists, request, totalItem);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
