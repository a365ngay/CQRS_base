// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class SearchRequestsByApproverQueryModel : BaseModel
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EnumRequestStatus? Status { get; set; }
        public EnumPriority Priority { get; set; }
        public bool IsApprove { get; set; }
        public Guid? ApproverId { get; set; }
    }
}
