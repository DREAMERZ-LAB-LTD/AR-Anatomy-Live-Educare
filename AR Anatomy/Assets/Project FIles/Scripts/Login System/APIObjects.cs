using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
namespace API
{

    #region Register Object
    /// <summary>
    /// Register / Sign Up Object
    /// </summary>
    [System.Serializable]
    public struct Register//use wnen new user register to server
    {
        public string email;
        public string phone;
        public string password;
        public string confirm_password;
        public string name;
    }

    public class EmailVerificationResponse
    {
        public bool success { get; set; }
        public API.Error error { get; set; }
    }
    public struct Error
    {
        public string token { get; set; }
    }
    #endregion Register Object

    #region Email Verification
    [System.Serializable]
    public struct EmailVerification //use after complete registrarion
    {
        public string email;
        public string code;
    }

    




    #endregion Email Verification

    #region User Login 
    [System.Serializable]
    public struct Login//use when user login
    {
        public string email;
        public string password;
    }
    [System.Serializable]
    public struct LoginResponse
    {
        public bool success;
        public UserAccess data;
    }
    
    [System.Serializable]
    public struct UserAccess
    {
        public string token;
    }
    #endregion User Login 

    #region Password Reset
    [System.Serializable]
    public struct PasswordResetRequest
    {
        public string email;
    }
    [System.Serializable]
    public struct PasswordResetResponse
    {
        public bool success;
        public string message;
    }

    [System.Serializable]
    public struct SetNewPassword
    {
        public string email;
        public string password;
        public string password_confirmation;
        public string code;
    }
    #endregion Password Reset

    #region User Retrive
    [System.Serializable]
    public struct User
    {
        public bool success;
        public UserInformation data;
    }
    [System.Serializable]
    public struct UserInformation
    {
        public int id;
        public string name;
        public string email;
        public string email_verified_at;
        public string created_at;
        public string updated_at;
    }
    #endregion User Retrive

}
*/