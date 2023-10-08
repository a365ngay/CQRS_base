// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Shared.Constants
{
    public static class EmailContents
    {
        public const string CreateRequest = @"Bạn đã nhận được môt yêu cầu xử lý {0} từ tài khoản {1}";
        public const string RejectRequest = @"Yêu cầu xử lý {0}, tên yêu cầu {1} đã bị từ chối
Lý do: {2}";
        public const string DoneRequest = @"Yêu cầu xử lý {0}, tên yêu cầu {1} đã được xử lý xong";
    }
}
