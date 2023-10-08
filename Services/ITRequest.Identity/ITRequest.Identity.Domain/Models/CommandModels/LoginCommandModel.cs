// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.CommandModels
{
    using System.ComponentModel.DataAnnotations;

    public class LoginCommandModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
