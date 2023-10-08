// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Commands.AuthCmd
{
    using System;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Core.Base;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.Enums.ErrorCodes;
    using ITRequest.Identity.Domain.Models.CommandModels;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class ResetPasswordCommand : ResetPasswordCommandModel, IRequest<MethodResult<bool>>
    {
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, MethodResult<bool>>
    {
        private readonly UserManager<User> _userManager;

        private readonly AuthContext _authContext;
        private readonly SignInManager<User> _signInManager;

        public ResetPasswordCommandHandler(UserManager<User> userManager,
            AuthContext authContext,
            SignInManager<User> signInManager)
        {
            _authContext = authContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<MethodResult<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<bool> methodResult = new MethodResult<bool>();
            if (request.OldPassword == null)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.OldPassWordNotEmpty),
                    nameof(request.OldPassword), request.OldPassword);
                return methodResult;
            }
            if (request.Password == null)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.PasswordNotEmpty),
                    nameof(request.Password), request.Password);
                return methodResult;
            }
            if (request.ConfirmPassword == null)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.ConfirmPasswordNotEmpty),
                    nameof(request.ConfirmPassword), request.ConfirmPassword);
                return methodResult;
            }

            if (request.ConfirmPassword != request.Password)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.ConfirmPasswordWrong),
                    nameof(request.ConfirmPassword), request.ConfirmPassword);
                return methodResult;
            }

            User? user;
            if (string.IsNullOrEmpty(request.Email))
            {
                user = await _userManager.FindByIdAsync(_authContext.CurrentUserId.ToString());
            }
            else
            {
                user = await _userManager.FindByEmailAsync(request.Email);
            }

            if (user == null)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.EmailNotExist),
                    nameof(request.Email), request.Email);
                return methodResult;
            }

            var checkOldPassword = await _signInManager.PasswordSignInAsync(user.UserName ?? string.Empty, request.OldPassword, false, false);
            if (!checkOldPassword.Succeeded)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.OldPasswordIncorrect),
                    nameof(request.OldPassword), request.OldPassword);
                return methodResult;
            }

            var hashPassword = _userManager.PasswordHasher.HashPassword(user, request.Password);
            user.PasswordHash = hashPassword;
            await _userManager.UpdateAsync(user);

            methodResult.StatusCode = StatusCodes.Status200OK;
            methodResult.Result = true;
            return methodResult;
        }
    }
}
