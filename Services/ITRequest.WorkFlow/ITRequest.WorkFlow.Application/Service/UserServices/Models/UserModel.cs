// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Application.Service.UserServices.Models
{
    using Fsel.Core.Base.BaseModels;
    using ITRequest.Shared.Enum;

    public class UserModel : BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public string? FullName
        {
            get { return $"{FirstName} {LastName}".Trim(); }
        }

        public string? Code { get; set; }
        public string? AvatarPath { get; set; }

        public EnumRequestType Type { get; set; }
        public string? Department { get; set; }
        public string? Position { get; set; }
        public string? Role { get; set; }
    }
}
