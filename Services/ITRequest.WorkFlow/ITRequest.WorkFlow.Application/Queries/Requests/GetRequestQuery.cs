// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Queries.Requests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Enums.ErrorCodes;
    using ITRequest.WorkFlow.Application.Service.UserServices;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class GetRequestQuery : IRequest<MethodResult<RequestModel>>
    {
        public Guid Id { get; set; }
    }

    public class GetRequestQueryHandler : IRequestHandler<GetRequestQuery, MethodResult<RequestModel>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetRequestQueryHandler(IRequestRepository requestRepository, IMapper mapper, IUserService userService)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<MethodResult<RequestModel>> Handle(GetRequestQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<RequestModel> methodResult = new MethodResult<RequestModel>();
            var requestModel = await _requestRepository.Queryable
                            .Include(x => x.Approvals)
                            .Select(x => new RequestModel
                            {
                                Id = x.Id,
                                Content = x.Content,
                                CreatedFullName = x.CreatedFullName,
                                CreatedUserId = x.CreatedUserId,
                                FilePathsStr = x.FilePathsStr,
                                FilePaths = x.FilePaths,
                                IsApprove = x.IsApprove,
                                Priority = x.Priority,
                                Status = x.Status,
                                Title = x.Title,
                                Type = x.Type,
                                CreatedDate = x.CreatedDate,
                                ApproverId = x.Approvals.OrderBy(n => n.CreatedDate).Select(n => n.ApproverId).FirstOrDefault()
                            }).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (requestModel == null)
            {
                methodResult.AddErrorBadRequest(nameof(EnumSystemErrorCode.DataNotExist), nameof(request.Id));
                return methodResult;
            }

            methodResult.Result = requestModel;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
