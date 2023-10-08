// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Queries.Requests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Core.Base;
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class GetTheNumberOfRequestByStateByUserQuery : IRequest<MethodResult<GetTheNumberOfRequestByStateByUserModel>>
    {

    }
    public class GetTheNumberOfRequestByStateByUserQueryHandler : IRequestHandler<GetTheNumberOfRequestByStateByUserQuery, MethodResult<GetTheNumberOfRequestByStateByUserModel>>
    {
        private readonly AuthContext _authContext;
        private readonly IRequestRepository _requestRepository;

        public GetTheNumberOfRequestByStateByUserQueryHandler(AuthContext authContext, IRequestRepository requestRepository)
        {
            _authContext = authContext;
            _requestRepository = requestRepository;
        }

        public async Task<MethodResult<GetTheNumberOfRequestByStateByUserModel>> Handle(GetTheNumberOfRequestByStateByUserQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var methodResult = new MethodResult<GetTheNumberOfRequestByStateByUserModel>();

            var result = new GetTheNumberOfRequestByStateByUserModel();
            var requestQuery = _requestRepository.Queryable.Where(p => p.CreatedUserId == _authContext.CurrentUserId);
            result.NumberRequestPending = await requestQuery.Where(x => x.Status == EnumRequestStatus.Pending).CountAsync(cancellationToken);
            result.NumberRequestDoing = await requestQuery.Where(x => x.Status == EnumRequestStatus.Doing).CountAsync(cancellationToken);
            result.NumberRequestDone = await requestQuery.Where(x => x.Status == EnumRequestStatus.Done || x.Status == EnumRequestStatus.Reject).CountAsync(cancellationToken);

            result.NumberApprovalPending = await _requestRepository.Queryable.Include(p => p.Approvals)
                .Where(o => o.Status != EnumRequestStatus.Done && o.Status != EnumRequestStatus.Reject)
                .Select(x => new
                {
                    ApproverId = x.Approvals.OrderByDescending(p => p.CreatedDate).Where(a => a.Status == EnumRequestStatus.Pending || a.Status == EnumRequestStatus.Doing).Select(x => x.ApproverId).FirstOrDefault(),
                })
                .Where(p => p.ApproverId == _authContext.CurrentUserId)
                .CountAsync(cancellationToken);

            methodResult.Result = result;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
