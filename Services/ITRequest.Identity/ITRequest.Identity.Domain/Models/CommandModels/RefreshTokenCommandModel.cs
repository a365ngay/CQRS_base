// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.CommandModels
{
    using System.ComponentModel.DataAnnotations;

    public class RefreshTokenCommandModel
    {
        [Required]
        public string? AccessToken { get; set; }

        [Required]
        public string? RefreshToken { get; set; }
    }
}
