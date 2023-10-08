// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class ApprovalModel : BaseModel
    {
        public Guid RequestId { get; set; }
        public int ApprovalLevel { get; set; }
        public Guid ApproverId { get; set; }
        public Guid? ApprovalDate { get; set; }
        public string? Note { get; set; }
        public EnumRequestStatus Status { get; set; }
        public string? ApproverName { get; set; }
        public string? ApproverDepartment { get; set; }
        public string? ApproverPosition { get; set; }
    }
}
