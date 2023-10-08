
using Fsel.Common.ActionResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ITRequest.Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;
using ITRequest.Identity.Domain.Enums.ErrorCodes;
using Microsoft.EntityFrameworkCore;
using Fsel.Core.Base;
using System.Text.RegularExpressions;
using ITRequest.Shared.Enum;

namespace ITRequest.Identity.Application.Commands.AuthCmd
{
    public class ChangePasswordCommand : IRequest<MethodResult<bool>>
    {
        public String NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, MethodResult<bool>>
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthContext _authContext;
        private readonly SignInManager<User> _signInManager;


        public ChangePasswordCommandHandler(UserManager<User> userManager, AuthContext authContext, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _authContext = authContext;
            _signInManager = signInManager;
        }
        public async Task<MethodResult<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<bool> methodResult = new MethodResult<bool>();
            // check mật khẩu mới phải ít nhất 8 kí tự, có 1 chữ viết hoa , 1 kí tự đặc biệt
            string pattern = @"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+])[A-Za-z\d!@#$%^&*()_+]{8,}$";
            if (!Regex.IsMatch(request.NewPassword, pattern))
            {
                methodResult.AddError(nameof(EnumAuthErrorCode.InvalidPassword));
                return methodResult;
            }

            User? user = await _userManager.FindByIdAsync(_authContext.CurrentUserId.ToString());


            if (user == null)
            {
                methodResult.AddErrorBadRequest(
                    nameof(EnumAuthErrorCode.UserNotExist),
                    nameof(request.CurrentPassword), request.CurrentPassword);
                return methodResult;
            }
            if (request.CurrentPassword == request.NewPassword)
            {
                methodResult.AddError(nameof(EnumAuthErrorCode.NewPasswordMustBeDiffirent));
                return methodResult;
            }

            var checkOldPassword = await _signInManager.PasswordSignInAsync(user.UserName ?? string.Empty, request.CurrentPassword, false, false);

            if (!checkOldPassword.Succeeded)
            {
                methodResult.AddError(nameof(EnumAuthErrorCode.ErrorCurrentPassword));
                return methodResult;
            }

            await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            methodResult.Result = true;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
