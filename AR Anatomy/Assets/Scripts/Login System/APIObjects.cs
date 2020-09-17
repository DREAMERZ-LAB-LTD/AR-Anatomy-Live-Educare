using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
    /// <summary>
    /// Register / Sign Up Object
    /// </summary>
    public struct Register//use wnen new user register to server
    {
        public string email;
        public string phone;
        public string password;
        public string confirm_password;
        public string name;
    }

    public struct EmailVerification //use after complete registrarion
    {
        public string email;
        public string code;
    }

    public struct Login//use when user login
    {
        public string email;
        public string password;
    }

    public struct LoginResponse
    {
        public bool success;
        public UsertAccess data;
    }
    public struct UsertAccess
    {
        public string token { get; set; }
    }


    public struct PasswordResetRequest
    {
        public string email;
    }
    public struct PasswordResetResponse
    {
        public bool success;
        public string message;
    }

    public struct SetNewPassword
    {
        public string email;
        public string password;
        public string password_confirmation;
        public string code;
    }
}
