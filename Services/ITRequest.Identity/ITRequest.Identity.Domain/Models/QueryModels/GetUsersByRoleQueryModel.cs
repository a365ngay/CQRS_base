// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.QueryModels
{
    using ITRequest.Shared.Enum;

    public class GetUsersByRoleQueryModel
    {
        public EnumRole Role { get; set; }
    }
}
