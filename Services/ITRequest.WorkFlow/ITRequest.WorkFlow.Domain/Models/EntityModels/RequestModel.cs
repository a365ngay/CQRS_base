// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class RequestModel : BaseModel
    {
        public EnumRequestType Type { get; set; }
        public bool IsApprove { get; set; }
        public Guid? ApproverId { get; set; }
        public EnumPriority Priority { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? FilePathsStr { get; set; }
        public IList<string>? FilePaths { get; set; }
        public EnumRequestStatus Status { get; set; }
    }
}
