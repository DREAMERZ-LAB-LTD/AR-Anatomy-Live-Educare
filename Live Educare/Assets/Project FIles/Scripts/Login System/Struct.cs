using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Registation Struct
[System.Serializable]
public struct Registation
{
    public string name;
    public string email;
    public string phone;
    public string password;
    public string confirm_password;
}
[System.Serializable]
public struct User
{
    public string name;
    public string email;
    public string updated_at;
    public string created_at;
    public int id;
}
[System.Serializable]
public struct Data
{
    public string token;
    public User user;
}
[System.Serializable]
public struct SuccessRegistation
{
    public bool success;
    public Data data;
}
[System.Serializable]
public struct ErrorRegistation
{
    public bool success;
    public Error error;
}
[System.Serializable]
public struct Error
{
    public List<string> email;
    public List<string> phone;
    public List<string> confirm_password;
}


#region Verify Code

[System.Serializable]
public struct VeryficationCode
{
    //public int code;//it was original variable
    public string code;
    public string email;
}
[System.Serializable]
public struct ErrorVerify
{
    public string token;
}
[System.Serializable]
public struct ErrorVeryficationCode
{
    public bool success;
    public ErrorVerify error;
}

#endregion

#endregion

#region Login Struct
[System.Serializable]
public struct LogInStruct
{
    public string email;
    public string password;
}

[System.Serializable]
public struct LoginData
{
    public string token;
}

[System.Serializable]
public struct SuccessLogIn
{
    public bool success;
    public LoginData data;
}
[System.Serializable]
public struct ErrorLogIn
{
    public bool success;
    public string error;
}

#endregion


#region Reset Password

[System.Serializable]
public struct ResetPasswordEmail
{
    public string email;
}

[System.Serializable]
public struct ErrorRestEmail
{
    public string email;
}  
[System.Serializable]
public struct ErrorResetPasswordEmail
{
    public bool success;
    public ErrorRestEmail error;
}
    [System.Serializable]
public struct ResetPasswordVerifyCode
{
    //public int code; //it was original type
    public string code;
    public string email;
}

    [System.Serializable]
public struct Message
{
    public string token;
}

[System.Serializable]
public struct ErrorResetPasswordVerifyCode
{
    public bool success;
    public Message message;
}




[System.Serializable]
public struct ResetPasswordNewPassword
{
    public string email;
    public string password;
    public string password_confirmation;
    public string code;
    //public int code;//it was original value
}

[System.Serializable]
public struct ErrorNewPassword
{
    public List<string> password;
}
[System.Serializable]
public struct ErrorResetPasswordNewPassword
{
    public string message;
    public ErrorNewPassword errors;
}

#endregion

#region GetUserInfo

[System.Serializable]
public struct GetUserInfoData
{
    public int id;
    public string name;
    public string email;
    public string email_verified_at;
    public string created_at;
    public string updated_at;

}

[System.Serializable]
public struct GetUserInfoStruct
{
    public bool success;
    public GetUserInfoData data;
}


#endregion


#region Facebook

[System.Serializable]
public struct FacebookStruct
{
    public string provider;
    public string access_token;
}

[System.Serializable]
public struct SuccessFacebookStruct
{
    public string token_type;
    public string access_token;
}

#endregion

#region Apple
[System.Serializable]
public struct AppleLoginFirstTime
{
    public string provider;
    public string email;
    public string name;
    public string token;
}
[System.Serializable]
public struct AppleLogin
{
    public string provider;
    public string token;
}
#endregion



#region ReAuthentication

[System.Serializable]
public struct RegistationResetVerifyEmail
{
    public string email;
}

[System.Serializable]
public class SuccessfullVerifyTokenHolder
{
    public string token;
}
[System.Serializable]
public class SuccessVerifyEmail
{
    public bool success;
    public string message;
    public SuccessfullVerifyTokenHolder data;
}

#endregion ReAuthentication