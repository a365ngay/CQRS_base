// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.QueryModels
{
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class SearchRequestsByUserQueryModel : BaseQueryModel
    {
        public EnumRequestStatus Status { get; set; }
    }
}
