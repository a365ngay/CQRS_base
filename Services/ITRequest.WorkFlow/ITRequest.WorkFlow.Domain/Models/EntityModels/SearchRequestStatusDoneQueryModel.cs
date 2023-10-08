// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    using System;
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class SearchRequestStatusDoneQueryModel : BaseModel
    {
        public EnumRequestType Type { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public EnumPriority Priority { get; set; }
        public long ProcessingTime { get; set; }
    }
}
