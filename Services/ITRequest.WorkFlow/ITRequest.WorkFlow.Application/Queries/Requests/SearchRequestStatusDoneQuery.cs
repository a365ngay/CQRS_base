// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Queries.Requests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Core.Base.BaseModels;
    using Fsel.Core.Extensions;
    using ITRequest.Shared.Enum;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class SearchRequestStatusDoneQuery : BaseQueryModel, IRequest<MethodResult<PagingItemsModel<SearchRequestStatusDoneQueryModel>>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class SearchRequestStatusDoneQueryHandler : IRequestHandler<SearchRequestStatusDoneQuery, MethodResult<PagingItemsModel<SearchRequestStatusDoneQueryModel>>>
    {
        private readonly IRequestRepository _requestRepository;

        public SearchRequestStatusDoneQueryHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<MethodResult<PagingItemsModel<SearchRequestStatusDoneQueryModel>>> Handle(SearchRequestStatusDoneQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<PagingItemsModel<SearchRequestStatusDoneQueryModel>>();

            if (request.PageSize > 100)
            {
                methodResult.StatusCode = StatusCodes.Status400BadRequest;
                return methodResult;
            }

            var requestsDone = _requestRepository.Queryable.Where(n => !request.StartDate.HasValue || !request.EndDate.HasValue || (request.StartDate.Value.Date <= n.CreatedDate && n.CreatedDate.Date <= request.EndDate.Value.Date)).Where(p => p.Status == EnumRequestStatus.Done).Select(x => new SearchRequestStatusDoneQueryModel
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Priority = x.Priority,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                Type = x.Type,
            });
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                requestsDone = requestsDone.Where(p => !string.IsNullOrEmpty(p.Title) && p.Title.Contains(request.Keyword));
            }
            int totalItem = await requestsDone.CountAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            var lists = await requestsDone
                    .ApplySortAndPaging(request)
                    .OrderByDescending(x => x.CreatedDate)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            foreach (var item in lists)
            {
                if (item.UpdatedDate.HasValue)
                {
                    TimeSpan timeDifference = item.UpdatedDate.Value - item.CreatedDate!.Value;
                    item.ProcessingTime = (long)timeDifference.TotalMinutes;
                }
            }
            methodResult.Result = new PagingItemsModel<SearchRequestStatusDoneQueryModel>(lists, request, totalItem);
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
