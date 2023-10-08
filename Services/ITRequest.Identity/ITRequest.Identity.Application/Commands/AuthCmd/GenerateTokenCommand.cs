// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Application.Commands.AuthCmd
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Constants;
    using Fsel.Common.Helpers;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.IRepositories;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using ITRequest.Identity.Infrastructure.ValueSetting;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;

    public class GenerateTokenCommand : IRequest<MethodResult<TokenModel>>
    {
        public string? Id { get; set; }
    }

    public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, MethodResult<TokenModel>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly AppSetting _appSetting;
        private readonly IMapper _mapper;

        public GenerateTokenCommandHandler(UserManager<User> userManager,
            IUserTokenRepository userTokenRepository,
            AppSetting appSetting,
            IMapper mapper)
        {
            _userManager = userManager;
            _userTokenRepository = userTokenRepository;
            _appSetting = appSetting;
            _mapper = mapper;
        }

        public async Task<MethodResult<TokenModel>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            MethodResult<TokenModel> methodResult = new MethodResult<TokenModel>();
            var user = await _userManager.FindByIdAsync(request.Id ?? string.Empty);
            if (user == null)
            {
                methodResult.StatusCode = StatusCodes.Status401Unauthorized;
                return methodResult;
            }

            var jti = Guid.NewGuid().ToString();
            var authClaims = new List<Claim>
            {
                new Claim(JwtClaimNames.UserName, user.UserName ?? string.Empty),
                new Claim(JwtClaimNames.FullName, user.FullName ?? string.Empty),
                new Claim(JwtClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtClaimNames.UserId, user.Id ?? string.Empty),
                new Claim(JwtClaimNames.Sub, _appSetting.Jwt?.Subject ?? string.Empty),
                new Claim(JwtClaimNames.Jti, jti),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            userRoles.ForEach(userRole =>
            {
                authClaims.Add(new Claim(JwtClaimNames.Role, userRole));
                authClaims.Add(new Claim(JwtClaimNames.Roles, userRole));
            });

            var secretKeyBytes = Encoding.ASCII.GetBytes(_appSetting.Jwt?.SecretKey ?? string.Empty);
            var signin = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _appSetting.Jwt?.Issuer ?? string.Empty,
                _appSetting.Jwt?.Audience ?? string.Empty,
                authClaims,
                expires: DateTime.Now.AddMinutes(_appSetting.Jwt?.TokenValidityInMinutes ?? default),
                signingCredentials: signin
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = TokenHelper.GenerateRefreshToken();
            UserToken userToken = new UserToken
            {
                Name = jti,
                Value = accessToken,
                RefreshToken = refreshToken,
                LoginProvider = JwtBearerDefaults.AuthenticationScheme,
                UserId = user.Id ?? string.Empty,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(_appSetting.Jwt?.RefreshTokenValidityInDays ?? default)
            };
            var checkLogin = _userTokenRepository.CheckFirstLogin(userToken);
            await _userTokenRepository.AddAsync(userToken);


            var tokenLogin = new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo.ConvertTimeFromUtc(TimeZoneInfo.Local),
                FullName = user.FullName,
                Roles = userRoles.ToList(),
                IsFirstLogin =  checkLogin,
            };

            methodResult.Result = tokenLogin;
            methodResult.StatusCode = StatusCodes.Status200OK;
            return methodResult;
        }
    }
}
