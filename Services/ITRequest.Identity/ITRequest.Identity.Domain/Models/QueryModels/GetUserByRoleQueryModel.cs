// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.QueryModels
{
    using ITRequest.Shared.Enum;

    public class GetUserByRoleQueryModel
    {
        public EnumRoleRequest Role { get; set; }
    }
}
