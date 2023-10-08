// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Commands.RequestCmd
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Core.Base;
    using ITRequest.Shared.Constants;
    using ITRequest.Shared.Enum;
    using ITRequest.Shared.Models;
    using ITRequest.WorkFlow.Application.Commands.SenderCmd;
    using ITRequest.WorkFlow.Application.Service.SenderServices;
    using ITRequest.WorkFlow.Application.Service.UserServices;
    using ITRequest.WorkFlow.Application.Service.UserServices.Models;
    using ITRequest.WorkFlow.Domain.Entities;
    using ITRequest.WorkFlow.Domain.Enums;
    using ITRequest.WorkFlow.Domain.IRepositories;
    using ITRequest.WorkFlow.Domain.Models.Commands.Requests;
    using ITRequest.WorkFlow.Infrastructure.ValueSetting;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using IMediator = MediatR.IMediator;

    public class ChangeStatusRequestCommand : ChangeStatusRequestCommandModel, IRequest<MethodResult<bool>>
    {
    }

    public class ChangeStatusRequestCommandHandler : IRequestHandler<ChangeStatusRequestCommand, MethodResult<bool>>
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserService _userService;
        private readonly AuthContext _authContext;
        private readonly IApprovalRepository _approvalRepository;
        private readonly ISenderService _senderService;
        private readonly AppSetting _appSetting;
        private readonly IMediator _mediator;

        public ChangeStatusRequestCommandHandler(IRequestRepository requestRepository, IUserService userService, AuthContext authContext, IApprovalRepository approvalRepository, ISenderService senderService, AppSetting appSetting, IMediator mediator)
        {
            _requestRepository = requestRepository;
            _userService = userService;
            _authContext = authContext;
            _approvalRepository = approvalRepository;
            _senderService = senderService;
            _appSetting = appSetting;
            _mediator = mediator;
        }

        public async Task<MethodResult<bool>> Handle(ChangeStatusRequestCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<bool> methodResult = new MethodResult<bool>();
            var requestEntity = await _requestRepository.GetByIdAsync(request.RequestId);
            if (requestEntity == null)
            {
                methodResult.AddErrorBadRequest(nameof(EnumRequestErrorCode.RequestNotExist));
                return methodResult;
            }
            UserModel? approverModel = new UserModel();
            UserModel? createdUserModel = new UserModel();
            if (request.Role.HasValue)
            {
                var approverResult = await _userService.GetUserByRoleAsync(new GetUserByRoleQueryModel
                {
                    Role = request.Role.Value
                });
                if (!approverResult.IsSuccessStatusCode)
                {
                    methodResult.AddError(approverResult.Error);
                    return methodResult;
                }
                approverModel = approverResult.Content?.Result;
            }
            var lastLevel = await _approvalRepository.Queryable.Where(p => p.RequestId == requestEntity.Id).Select(x => x.ApprovalLevel).OrderByDescending(x => x).FirstOrDefaultAsync(cancellationToken);
            await _requestRepository.ExecuteTransactionAsync(async () =>
            {
                requestEntity.Approvals.Add(new Approval
                {
                    Status = request.Status,
                    ApproverId = _authContext.CurrentUserId,
                    Note = request.Note,
                    ApprovalLevel = lastLevel
                });
                if (request.Role.HasValue)
                {
                    requestEntity.Approvals.Add(new Approval
                    {
                        Status = EnumRequestStatus.Pending,
                        ApproverId = approverModel?.Id,
                        ApprovalLevel = lastLevel + 1
                    });
                    requestEntity.Status = EnumRequestStatus.Pending;
                }
                else
                {
                    requestEntity.Status = request.Status;
                }
                if (!requestEntity.IsValid())
                {
                    methodResult.AddErrorBadRequest(requestEntity.ErrorMessages);
                    return methodResult;
                }
                _requestRepository.Update(requestEntity);
                await _requestRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).ConfigureAwait(false);

                var param = new SendOtpTemplateModel
                {
                    RequestType = requestEntity.Type.ToString(),
                    Title = requestEntity.Title,
                    Email = _authContext.Email,
                    Content = requestEntity.Content,
                    CreatedDate = requestEntity.CreatedDate.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture),
                    AccessLink = string.Format(CultureInfo.InvariantCulture, _appSetting.ConstantUrl!.DetailRequestUrl!, requestEntity.Id),
                    Note = request.Note
                };
                var createdUserResult = await _userService.GetUserByIdAsync(requestEntity.CreatedUserId);
                if (!createdUserResult.IsSuccessStatusCode)
                {
                    methodResult.AddError(createdUserResult.Error);
                    return methodResult;
                }
                createdUserModel = createdUserResult.Content?.Result;
                if (requestEntity.Status == EnumRequestStatus.Reject || requestEntity.Status == EnumRequestStatus.Done)
                {
                    var sendCommandModel = new SendEmailCommandModel
                    {
                        ToEmails = new List<string> { $"{createdUserModel?.Email}" }
                    };
                    if (requestEntity.Status == EnumRequestStatus.Reject)
                    {
                        await SendMail(requestEntity, param, createdUserModel?.Email, SenderSettings.RejectRequest, EnumSenderTemplate.RejectRequest, cancellationToken);
                    }
                    else if (requestEntity.Status == EnumRequestStatus.Done)
                    {
                        await SendMail(requestEntity, param, createdUserModel?.Email, SenderSettings.DoneRequest, EnumSenderTemplate.DoneRequest, cancellationToken);
                    }
                }
                else if (requestEntity.Status == EnumRequestStatus.Pending)
                {
                    await SendMail(requestEntity, param, approverModel?.Email, SenderSettings.Subject, EnumSenderTemplate.RedirectRequest, cancellationToken);
                }
                else if (requestEntity.Status == EnumRequestStatus.Doing)
                {
                    await SendMail(requestEntity, param, createdUserModel?.Email, SenderSettings.DoingRequest, EnumSenderTemplate.DoingRequest, cancellationToken);
                }

                methodResult.StatusCode = StatusCodes.Status200OK;
                methodResult.Result = true;
                return methodResult;
            });
            return methodResult;
        }

        public async Task SendMail(Request requestEntity, SendOtpTemplateModel param, string? email, string emailSubject, EnumSenderTemplate enumSenderTemplate, CancellationToken cancellationToken)
        {
            var subject = string.Format(CultureInfo.InvariantCulture, emailSubject, requestEntity?.Title);
            if (!string.IsNullOrEmpty(email))
            {
                await _mediator.Send(new SenderCommand { Email = email, Subject = subject, Params = param, Template = enumSenderTemplate }, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
