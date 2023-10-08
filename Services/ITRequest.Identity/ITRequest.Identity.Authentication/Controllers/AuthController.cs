// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Authentication.Controllers
{
    using Fsel.Common.ActionResults;
    using System.Net;
    using Fsel.Common.Constants;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using ITRequest.Identity.Application.Commands.AuthCmd;

    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/auth")]
    [ApiController]
    public class AuthController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Login
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(MethodResult<TokenModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            MethodResult<TokenModel> commandResult = await _mediator.Send(command).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(MethodResult<TokenModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            MethodResult<TokenModel> commandResult = await _mediator.Send(command).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }
    }
}
