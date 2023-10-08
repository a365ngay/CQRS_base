// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.QueryModels
{
    using System;
    using Fsel.Core.Base.BaseModels;

    public class SearchApprovalByRequestIdQueryModel : BaseQueryModel
    {
        public Guid RequestId { get; set; }
    }
}
