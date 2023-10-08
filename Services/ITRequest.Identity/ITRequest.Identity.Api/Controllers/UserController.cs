// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Api.Controllers
{
    using System.Net;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Constants;
    using ITRequest.Identity.Application.Queries.User;
    using ITRequest.Identity.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get Users
        /// </summary>
        [HttpPost("get-by-ids")]
        [ProducesResponseType(typeof(MethodResult<List<UserModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsersByIds([FromBody] GetUsersByIdsQuery query)
        {
            MethodResult<List<UserModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// Get user by role
        /// </summary>
        [HttpPost("get-user-by-role")]
        [ProducesResponseType(typeof(MethodResult<UserModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsersByIds([FromBody] GetUserByRoleQuery query)
        {
            MethodResult<UserModel> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }
        /// <summary>
        /// Get User 
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(typeof(MethodResult<UserModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            MethodResult<UserModel> commandResult = await _mediator.Send(new GetUserByIdQuery { Id = id }).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// Get users by role
        /// </summary>
        [HttpPost("get-users-by-role")]
        [ProducesResponseType(typeof(MethodResult<IList<UserModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUsersByRole([FromBody] GetUsersByRoleQuery query)
        {
            MethodResult<IList<UserModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }
    }
}
