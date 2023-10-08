// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.Commands.Requests
{
    using ITRequest.Shared.Enum;

    public class CreateRequestCommandModel
    {
        public EnumRequestType Type { get; set; }
        public bool IsApprove { get; set; }
        public EnumPriority Priority { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IList<string>? FilePaths { get; set; }
        public Guid? ApproverId { get; set; }
    }
}
