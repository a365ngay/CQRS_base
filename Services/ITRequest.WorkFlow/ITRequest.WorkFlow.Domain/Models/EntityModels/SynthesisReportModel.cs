// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    using ITRequest.Shared.Enum;

    public class SynthesisReportModel
    {
        public EnumRequestType Type { get; set; }
        public int TotalRequest { get; set; }
        public int NumberRequestPending { get; set; }
        public int NumberRequestDoing { get; set; }
        public int NumberRequestReject { get; set; }
        public int NumberRequestDone { get; set; }
    }
}
