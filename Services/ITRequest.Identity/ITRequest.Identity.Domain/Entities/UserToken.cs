// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Entities
{
    using Fsel.Common.Enums.ErrorCodes;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    public class UserToken : IdentityUserToken<string>
    {
        [MaxLength(1000, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
