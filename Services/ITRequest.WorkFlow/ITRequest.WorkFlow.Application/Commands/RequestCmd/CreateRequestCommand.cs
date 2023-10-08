using System.Globalization;
using AutoMapper;
using Fsel.Common.ActionResults;
using Fsel.Core.Base;
using ITRequest.Shared.Constants;
using ITRequest.Shared.Enum;
using ITRequest.Shared.Models;
using ITRequest.WorkFlow.Application.Commands.SenderCmd;
using ITRequest.WorkFlow.Application.Service.UserServices;
using ITRequest.WorkFlow.Application.Service.UserServices.Models;
using ITRequest.WorkFlow.Domain.Entities;
using ITRequest.WorkFlow.Domain.Enums;
using ITRequest.WorkFlow.Domain.IRepositories;
using ITRequest.WorkFlow.Domain.Models.Commands.Requests;
using ITRequest.WorkFlow.Domain.Models.EntityModels;
using ITRequest.WorkFlow.Infrastructure.ValueSetting;
using MediatR;
using Microsoft.AspNetCore.Http;
using IMediator = MediatR.IMediator;

namespace ITRequest.WorkFlow.Application.Commands.RequestCmd
{
    public class CreateRequestCommand : CreateRequestCommandModel, IRequest<MethodResult<RequestModel>>
    {
    }

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, MethodResult<RequestModel>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly AuthContext _authContext;
        private readonly IMediator _mediator;
        private readonly AppSetting _appSetting;
        public CreateRequestCommandHandler(IRequestRepository requestRepository, IMapper mapper, IUserService userService, AuthContext authContext, IMediator mediator, AppSetting appSetting)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _userService = userService;
            _authContext = authContext;
            _mediator = mediator;
            _appSetting = appSetting;
        }

        public async Task<MethodResult<RequestModel>> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<RequestModel> methodResult = new MethodResult<RequestModel>();
            if (request.IsApprove && !request.ApproverId.HasValue)
            {
                methodResult.AddErrorBadRequest(nameof(EnumRequestErrorCode.ApproverIdNull));
                return methodResult;
            }
            if (!request.IsApprove)
            {
                var userResult = await _userService.GetUserByRoleAsync(new GetUserByRoleQueryModel { Role = EnumRoleRequest.HR });
                if (!userResult.IsSuccessStatusCode)
                {
                    methodResult.AddError(userResult.Error);
                    return methodResult;
                }
                request.ApproverId = userResult?.Content?.Result?.Id;
            }

            await _requestRepository.ExecuteTransactionAsync(async () =>
            {
                Request requestEntity = _mapper.Map<Request>(request);
                requestEntity.Status = EnumRequestStatus.Pending;
                requestEntity.Approvals.Add(new Approval
                {
                    ApprovalLevel = 1,
                    Status = EnumRequestStatus.Pending,
                    ApproverId = request.ApproverId,
                });

                if (!requestEntity.IsValid())
                {
                    methodResult.AddErrorBadRequest(requestEntity.ErrorMessages);
                    return methodResult;
                }
                requestEntity = _requestRepository.Add(requestEntity);
                await _requestRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

                if (request.ApproverId.HasValue)
                {
                    var userResult = await _userService.GetUserByIdAsync(request.ApproverId.Value);
                    if (!userResult.IsSuccessStatusCode)
                    {
                        methodResult.AddError(userResult.Error);
                        return methodResult;
                    }
                    var userModel = userResult.Content?.Result;

                    var param = new SendOtpTemplateModel
                    {
                        RequestType = requestEntity.Type.ToString(),
                        Title = requestEntity.Title,
                        Email = _authContext.Email,
                        Content = requestEntity.Content,
                        CreatedDate = requestEntity.CreatedDate.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture),
                        AccessLink = string.Format(CultureInfo.InvariantCulture, _appSetting.ConstantUrl!.DetailRequestUrl!, requestEntity.Id)
                    };
                    var subject = string.Format(CultureInfo.InvariantCulture, SenderSettings.Subject);
                    var sendResult = new MethodResult<bool>();
                    if (!string.IsNullOrEmpty(userModel?.Email))
                    {
                        sendResult = await _mediator.Send(new SenderCommand { Email = userModel.Email, Subject = subject, Params = param, Template = EnumSenderTemplate.CreateRequest }, cancellationToken).ConfigureAwait(false);
                    }
                }

                methodResult.StatusCode = StatusCodes.Status201Created;
                methodResult.Result = _mapper.Map<RequestModel>(requestEntity);
                return methodResult;
            });
            return methodResult;
        }
    }
}
