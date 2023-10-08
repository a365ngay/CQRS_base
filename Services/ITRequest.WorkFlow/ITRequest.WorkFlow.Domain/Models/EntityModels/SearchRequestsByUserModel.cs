// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class SearchRequestsByUserModel : BaseModel
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EnumRequestStatus Status { get; set; }
        public EnumPriority Priority { get; set; }
    }
}
