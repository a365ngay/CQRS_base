// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Shared.Constants
{
    public static class SenderSettings
    {
        public const string HostName = "FSEL";
        public const string SendOtpSubject = "[LMS - FSEL] Thông báo mã OTP";
        public const string Subject = "Bạn vừa nhận được một yêu cầu mới";

        public const string TemplateFileName = "Resources//{0}.html";

        public const string DoneRequest = "Yêu cầu: {0} của bạn đã được xử lý xong";
        public const string RejectRequest = "Yêu cầu: {0} của bạn đã bị từ chối xử lý";
        public const string DoingRequest = "Yêu cầu: {0} của bạn đang được xử lý";
    }
}
