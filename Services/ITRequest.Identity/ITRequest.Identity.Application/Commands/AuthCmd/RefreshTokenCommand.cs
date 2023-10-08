// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Commands.AuthCmd
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Helpers;
    using ITRequest.Identity.Domain.Enums.ErrorCodes;
    using ITRequest.Identity.Domain.IRepositories;
    using ITRequest.Identity.Domain.Models.CommandModels;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using ITRequest.Identity.Infrastructure.ValueSetting;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;

    public class RefreshTokenCommand : RefreshTokenCommandModel, IRequest<MethodResult<TokenModel>>
    {
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, MethodResult<TokenModel>>
    {
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly AppSetting _appSetting;
        private readonly IMediator _mediator;

        public RefreshTokenCommandHandler(
            IUserTokenRepository userTokenRepository,
            AppSetting appSetting,
            IMediator mediator)
        {
            _userTokenRepository = userTokenRepository;
            _mediator = mediator;
            _appSetting = appSetting;
        }

        public async Task<MethodResult<TokenModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            MethodResult<TokenModel> methodResult = new MethodResult<TokenModel>();
            ArgumentNullException.ThrowIfNull(request);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.ASCII.GetBytes(_appSetting.Jwt?.SecretKey ?? string.Empty);
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = _appSetting?.Jwt?.Audience,
                ValidIssuer = _appSetting?.Jwt?.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
            };
            var tokenValidationResult = await jwtTokenHandler.ValidateTokenAsync(request.AccessToken, tokenValidateParam);

            if (tokenValidationResult.SecurityToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase);
                if (!result)
                {
                    methodResult.AddError(StatusCodes.Status401Unauthorized, nameof(EnumAuthErrorCode.InvalidToken));
                    return methodResult;
                }
            }

            var checkExpireDate = long.TryParse(tokenValidationResult.ClaimsIdentity.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value, out long utcExpireDate);

            var expireDate = utcExpireDate.ConvertUnixTimeStampToDateTime();
            if (!checkExpireDate || expireDate < DateTime.Now)
            {
                methodResult.AddError(StatusCodes.Status401Unauthorized, nameof(EnumAuthErrorCode.AccessTokenNotYetExpired));
                return methodResult;
            }

            var refreshToken = await _userTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (refreshToken == null)
            {
                methodResult.AddError(StatusCodes.Status401Unauthorized, nameof(EnumAuthErrorCode.InvalidToken),
                    nameof(request.RefreshToken), request.RefreshToken);
                return methodResult;
            }
            else if (refreshToken.RefreshTokenExpiryTime == null || refreshToken.RefreshTokenExpiryTime.Value <= DateTime.Now)
            {
                methodResult.AddError(StatusCodes.Status401Unauthorized, nameof(EnumAuthErrorCode.RefreshTokenExpired));
            }

            await _userTokenRepository.Remove(refreshToken);

            methodResult = await _mediator.Send(new GenerateTokenCommand { Id = refreshToken.UserId }, cancellationToken).ConfigureAwait(false);
            return methodResult;
        }
    }
}
