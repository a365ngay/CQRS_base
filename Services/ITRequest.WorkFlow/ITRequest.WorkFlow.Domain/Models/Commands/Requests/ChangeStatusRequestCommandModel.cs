// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.Commands.Requests
{
    using ITRequest.Shared.Enum;

    public class ChangeStatusRequestCommandModel
    {
        public Guid RequestId { get; set; }
        public EnumRequestStatus Status { get; set; }
        public EnumRoleRequest? Role { get; set; }
        public string? Note { get; set; }
    }
}
