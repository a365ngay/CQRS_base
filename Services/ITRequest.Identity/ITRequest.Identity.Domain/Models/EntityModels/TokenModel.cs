// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.EntityModels
{
    using System;
    using System.Collections.Generic;

    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? FullName { get; set; }
        public DateTime? Expiration { get; set; }
        public IList<string>? Roles { get; set; }
        public bool IsFirstLogin { get; set;}
    }
}
