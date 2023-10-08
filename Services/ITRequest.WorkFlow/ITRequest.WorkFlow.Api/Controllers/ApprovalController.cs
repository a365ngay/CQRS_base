// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Api.Controllers
{
    using System.Net;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Constants;
    using Fsel.Core.Base.BaseModels;
    using ITRequest.WorkFlow.Application.Queries.Approvals;
    using ITRequest.WorkFlow.Application.Queries.Requests;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/approval")]
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprovalController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// search requests by approver
        /// </summary>
        [HttpGet("search-by-requestId")]
        [ProducesResponseType(typeof(MethodResult<PagingItemsModel<ApprovalModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchRequestsByApprover([FromQuery] SearchApprovalByRequestIdQuery query)
        {
            MethodResult<PagingItemsModel<ApprovalModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }
    }
}
