using Fsel.Common.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using Fsel.Core.Entities;
using ITRequest.Shared.Enum;
using Fsel.Common.Enums.ErrorCodes;
using System.ComponentModel.DataAnnotations;

namespace ITRequest.WorkFlow.Domain.Entities
{
    public class Request : Entity
    {
        public EnumRequestType Type { get; set; }
        public bool IsApprove { get; set; }
        public EnumPriority Priority { get; set; }

        [MaxLength(500, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Title { get; set; }

        [MaxLength(500, ErrorMessage = nameof(EnumSystemErrorCode.MaxLength))]
        public string? Content { get; set; }
        public string? FilePathsStr { get; set; }

        [NotMapped]
        public IList<string>? FilePaths
        {
            get { return ConvertHelper.Deserialize<IList<string>?>(FilePathsStr); }
            set { FilePathsStr = value == null ? null : ConvertHelper.Serialize(value); }
        }
        public EnumRequestStatus Status { get; set; }
        public ICollection<Approval> Approvals { get; set; } = new List<Approval>();
    }
}
