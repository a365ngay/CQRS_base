// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Api.Controllers
{
    using System.Net;
    using Fsel.Common.ActionResults;
    using Fsel.Common.Constants;
    using Fsel.Core.Base.BaseModels;
    using ITRequest.WorkFlow.Application.Commands.RequestCmd;
    using ITRequest.WorkFlow.Application.Queries.Requests;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// create request
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MethodResult<RequestModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateRequestCommand command)
        {
            MethodResult<RequestModel> commandResult = await _mediator.Send(command).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// search requests by user
        /// </summary>
        [Authorize]
        [HttpGet("search-by-user")]
        [ProducesResponseType(typeof(MethodResult<PagingItemsModel<SearchRequestsByUserModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchRequestsByUser([FromQuery] SearchRequestsByUserQuery query)
        {
            MethodResult<PagingItemsModel<SearchRequestsByUserModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// search requests by approver
        /// </summary>
        [Authorize]
        [HttpGet("search-by-approver")]
        [ProducesResponseType(typeof(MethodResult<PagingItemsModel<SearchRequestsByApproverQueryModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchRequestsByApprover([FromQuery] SearchRequestsByApproverQuery query)
        {
            MethodResult<PagingItemsModel<SearchRequestsByApproverQueryModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// change status request
        /// </summary>
        [HttpPut("change-status")]
        [ProducesResponseType(typeof(MethodResult<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ChangeStatusRequest([FromBody] ChangeStatusRequestCommand command)
        {
            MethodResult<bool> commandResult = await _mediator.Send(command).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// Get request
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MethodResult<RequestModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            MethodResult<RequestModel> commandResult = await _mediator.Send(new GetRequestQuery { Id = id }).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// Get Synthesis Report
        /// </summary>
        [HttpGet("get-synthesis-report")]
        [ProducesResponseType(typeof(MethodResult<IList<SynthesisReportModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSynthesisReport([FromQuery] GetSynthesisReportQuery query)
        {
            MethodResult<IList<SynthesisReportModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// search request status done
        /// </summary>
        [HttpGet("search-request-status-done")]
        [ProducesResponseType(typeof(MethodResult<PagingItemsModel<SearchRequestStatusDoneQueryModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchRequestStatusDone([FromQuery] SearchRequestStatusDoneQuery query)
        {
            MethodResult<PagingItemsModel<SearchRequestStatusDoneQueryModel>> commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }

        /// <summary>
        /// get number of request by status by user
        /// </summary>
        [HttpGet("get-number-of-request-by-status-by-user")]
        [ProducesResponseType(typeof(MethodResult<GetTheNumberOfRequestByStateByUserModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetNumberOfRequestByBtatusByUser([FromQuery] GetTheNumberOfRequestByStateByUserQuery query)
        {
            var commandResult = await _mediator.Send(query).ConfigureAwait(false);
            return commandResult.GetActionResult();
        }
    }
}
