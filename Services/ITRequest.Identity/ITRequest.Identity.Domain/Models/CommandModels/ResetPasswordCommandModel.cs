// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.CommandModels
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordCommandModel
    {
        public string? Email { get; set; }

        [Required]
        public string? OldPassword { get; set; }

        [Required]
        [Compare(nameof(ConfirmPassword))]
        public string? Password { get; set; }

        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
