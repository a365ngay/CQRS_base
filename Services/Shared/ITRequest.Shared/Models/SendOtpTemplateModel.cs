// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Shared.Models
{
    public class SendOtpTemplateModel
    {
        public string? RequestType { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? CreatedDate { get; set; }
        public string? Content { get; set; }
        public string? Note { get; set; }
        public string? AccessLink { get; set; }
    }
}
