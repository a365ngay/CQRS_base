// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Queries.Requests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using ITRequest.Shared.Enum;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetSynthesisReportQuery : IRequest<MethodResult<IList<SynthesisReportModel>>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetSynthesisReportQueryHandler : IRequestHandler<GetSynthesisReportQuery, MethodResult<IList<SynthesisReportModel>>>
    {
        private readonly IRequestRepository _requestRepository;

        public GetSynthesisReportQueryHandler(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        public async Task<MethodResult<IList<SynthesisReportModel>>> Handle(GetSynthesisReportQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<IList<SynthesisReportModel>> methodResult = new MethodResult<IList<SynthesisReportModel>>();

            var synthesisReportModel = await _requestRepository.Queryable.Where(n => !request.StartDate.HasValue || !request.EndDate.HasValue || (request.StartDate.Value.Date <= n.CreatedDate && n.CreatedDate.Date <= request.EndDate.Value.Date)).GroupBy(p => p.Type).Select(x => new SynthesisReportModel
            {
                Type = x.Key,
                TotalRequest = x.Count(),
                NumberRequestPending = x.Where(i => i.Status == EnumRequestStatus.Pending).Count(),
                NumberRequestDoing = x.Where(i => i.Status == EnumRequestStatus.Doing).Count(),
                NumberRequestDone = x.Where(i => i.Status == EnumRequestStatus.Done).Count(),
                NumberRequestReject = x.Where(i => i.Status == EnumRequestStatus.Reject).Count(),
            }).ToListAsync(cancellationToken);

            methodResult.Result = synthesisReportModel;
            return methodResult;
        }
    }
}
