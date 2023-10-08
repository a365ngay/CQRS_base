using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fsel.Common.Enums.ErrorCodes;
using ITRequest.Shared.Enum;
using Microsoft.AspNetCore.Identity;

namespace ITRequest.Identity.Domain.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(250, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(250, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? LastName { get; set; }

        [NotMapped]
        public string? FullName
        {
            get { return $"{FirstName} {LastName}".Trim(); }
        }

        [Required]
        [MaxLength(250, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Code { get; set; }

        [MaxLength(500, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? AvatarPath { get; set; }

        [MaxLength(500, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Department { get; set; }

        [MaxLength(500, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Position { get; set; }

    }
}
