// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Models.QueryModels
{
    public class GetUsersByIdsQueryModel
    {
        public IList<string>? UserIds { get; set; }
    }
}
