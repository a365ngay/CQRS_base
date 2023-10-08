// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Commands.AuthCmd
{
    using System;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Helpers;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.Enums.ErrorCodes;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class ForgotPasswordCommand : IRequest<MethodResult<bool>>
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, MethodResult<bool>>
    {
        private readonly UserManager<User> _userManager;

        public ForgotPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<MethodResult<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<bool> methodResult = new MethodResult<bool>();
            var user = await _userManager.FindByEmailAsync(request.Email ?? string.Empty);
            if (user == null)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.EmailNotExist),
                    nameof(request.Email), request.Email);
                return methodResult;
            }

            var newPassword = new PasswordGeneratorHelper(8, 10).Generate();

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(resetToken))
            {
                methodResult.StatusCode = StatusCodes.Status500InternalServerError;
                methodResult.AddError(nameof(EnumAuthErrorCode.ErrorResetToken));
                return methodResult;
            }
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
            {
                methodResult.StatusCode = StatusCodes.Status500InternalServerError;
                methodResult.AddError(nameof(EnumAuthErrorCode.ErrorResetPassword));
                return methodResult;
            }

            methodResult.Result = true;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
