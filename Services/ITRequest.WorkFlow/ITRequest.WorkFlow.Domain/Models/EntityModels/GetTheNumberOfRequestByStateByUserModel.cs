// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Domain.Models.EntityModels
{
    public class GetTheNumberOfRequestByStateByUserModel
    {
        public int NumberRequestPending { get; set; }
        public int NumberRequestDoing { get; set; }
        public int NumberRequestDone { get; set; }
        public int NumberApprovalPending { get; set; }
    }
}
