// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Sender.Domain.Models.CommandModels
{
    using ITRequest.Shared.Enum;

    public class SendEmailByTemplateCommandModel : SendEmailCommandModel
    {
        public EnumSenderTemplate? Template { get; set; }
        public object? Params { get; set; }
    }
}
