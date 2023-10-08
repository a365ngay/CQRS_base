// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using Fsel.Common.Enums.ErrorCodes;
    using Microsoft.AspNetCore.Identity;

    public class Role : IdentityRole
    {
        [MaxLength(250, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Name { get; set; }
        [MaxLength(250, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Discription { get; set; }
    }
}
