// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Domain.Enums.ErrorCodes
{
    public enum EnumAuthErrorCode
    {
        /// <summary>
        /// Old Password cannot empty
        /// </summary>
        OldPassWordNotEmpty,

        /// <summary>
        /// Password cannot empty
        /// </summary>
        PasswordNotEmpty,

        /// <summary>
        /// Confirm Password cannot empty
        /// </summary>
        ConfirmPasswordNotEmpty,

        /// <summary>
        /// ConfirmPassword Wrong
        /// </summary>
        ConfirmPasswordWrong,

        /// <summary>
        /// Email does not exist
        /// </summary>
        EmailNotExist,

        /// <summary>
        /// Old password is incorrect
        /// </summary>
        OldPasswordIncorrect,

        /// <summary>
        /// Duplicate email, please select another email
        /// </summary>
        DuplicateEmail,

        /// <summary>
        /// Duplicate PhoneNumber, please select another PhoneNumber
        /// </summary>
        DuplicatePhoneNumber,

        /// <summary>
        /// Email verified fail
        /// </summary>
        EmailVerifiedFail,

        /// <summary>
        /// Error while generating reset token
        /// </summary>
        ErrorResetToken,

        /// <summary>
        /// Error while generating reset Password
        /// </summary>
        ErrorResetPassword,

        /// <summary>
        /// Username and Password cannot empty
        /// </summary>
        UserNameAndPasswordNotEmpty,

        /// <summary>
        /// Username and Password  is incorrect
        /// </summary>
        UserNameAndPasswordIncorrect,

        /// <summary>
        /// Invalid token
        /// </summary>
        InvalidToken,

        /// <summary>
        /// Access token has not yet expired
        /// </summary>
        AccessTokenNotYetExpired,

        /// <summary>
        /// Refresh token has expired
        /// </summary>
        RefreshTokenExpired,

        /// <summary>
        /// Sign up fail
        /// </summary>
        SignUpFail,

        /// <summary>
        /// User Failed to Create
        /// </summary>
        UserFailToCreate,

        /// <summary>
        /// Send Auth erorr
        /// </summary>
        SendAuthErorr,

        /// <summary>
        /// User was active
        /// </summary>
        UserActive,

        /// <summary>
        /// Error Sending Email
        /// </summary>
        ErrorSendEmail,

        /// <summary>
        /// Invalid OTP, please try again
        /// </summary>
        InvalidOTP,

        /// <summary>
        /// OTP has expired, please click on the Resend code button below to receive a new OTP
        /// </summary>
        OTPExpired,

        /// <summary>
        /// Current password is incorrect
        /// </summary>
        CurrentPasswordIsIncorrect,

        /// <summary>
        /// Change password , Current password is incorrect
        /// </summary>
        ErrorCurrentPassword,

        /// <summary>
        /// Invalid Password
        /// </summary>
        InvalidPassword,

        /// <summary>
        /// User Not Exist
        /// </summary>
        UserNotExist,

        /// <summary>
        ///  NewPassword Must be Different From CurrentPassword
        /// </summary>
        NewPasswordMustBeDiffirent


    }
}
