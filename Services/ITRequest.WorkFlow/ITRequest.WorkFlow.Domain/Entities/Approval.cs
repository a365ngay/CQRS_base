// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Fsel.Common.Enums.ErrorCodes;
    using Fsel.Core.Entities;
    using ITRequest.Shared.Enum;

    public class Approval : Entity
    {
        public Guid RequestId { get; set; }
        public int ApprovalLevel { get; set; }
        public Guid? ApproverId { get; set; }
        public Guid? ApprovalDate { get; set; }

        [MaxLength(500, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Note { get; set; }

        public EnumRequestStatus Status { get; set; }
        public Request? Request { get; set; }
    }
}
