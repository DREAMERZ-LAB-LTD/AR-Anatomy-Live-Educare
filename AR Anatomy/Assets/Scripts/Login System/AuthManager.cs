using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UI;
using System.Security.Cryptography;

namespace LoginRegisterSystem
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Sever Root URL")]
        [SerializeField] protected string BaseURL = "http://18.140.117.58/dlabtrial/public/api";
        [SerializeField] private UI_Handeler ui;

        private static string TokenKey = "Token";//user token will be save into cash using this Token key

        /// <summary>
        /// return true if an user allready login
        /// </summary>
        public static bool isLogedin //=> Token != "";
        {
            get
            {

                Debug.Log("Loin = " + Token != null);
                Debug.Log("Token = " + Token);
                return Token != "";
            }
        }

        /// <summary>
        /// User Token
        /// you can change user token and save automaticly into cash;
        /// </summary>
        public static string Token
        {
            get
            {
                return PlayerPrefs.GetString(TokenKey);
            }
            private set
            {
                if (value != "")
                    PlayerPrefs.SetString(TokenKey, value);
            }
        }



        private void Start()
        {
            ui.SetRegisterPage_BtnEvent(OnClickRegister, OnClickAppleRegister, OnClickFacebookRegister);
         //   ui.SetVerificationPage_BtnEvent(OnClickVerification, OnClickRecentCode);
            ui.SetLoginPage_BtnEvent(OnClickLogin, OnClickAppleLogin, OnClickFacebookLogin);
            ui.SetResetPasswordPage_BtnEvent(OnClickResetPasswordRequest, OnclickPasswordResetToBack);
            ui.SetNewResetPasswordPage_BtnEvent(OnClickSetNewPassword);
        }

        #region User Login
        protected void OnClickLogin()
        {
            API.Login user = new API.Login();
            user.email = ui.GetLoginEmail;
            user.password = ui.GetLoginPassword;

            string json = JsonUtility.ToJson(user);
            string url = BaseURL + "/auth/login";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnSuccessLogin, OnError));
        }

        private void OnSuccessLogin(string json)
        {
            API.LoginResponse loginresponse = JsonUtility.FromJson<API.LoginResponse>(json);
            GetUserInfo(loginresponse.data.token);
            ui.ShowToast("Login Success", 2f, Color.green);
        }
        private void OnClickAppleLogin()
        {
            Show.Log("Apple Register Zone");
        }
        private void OnClickFacebookLogin()
        {
            Show.Log("Facebook Register Zone");
        }
        #endregion User Login
        private void OnClickLogOut()
        {
            PlayerPrefs.DeleteKey(TokenKey);

        }

        #region User Registration
        API.Register NewUser;//current Registred user data store for access to next step 
        private void OnClickRegister()
        {
            if (!ui.isMached_RegisterPassword())
            {
                ui.ShowWarning(new Warning("Password not mached", "please mached confirm password", null));
                return;
            }

            NewUser = new API.Register();
            NewUser.name = ui.GetRegisterName;
            NewUser.email = ui.GetRegisterEmail;
            NewUser.phone = ui.GetRegisterPhoneNumber;
            NewUser.password = ui.GetRegisterPassword;
            NewUser.confirm_password = ui.GetRegisterPassword;

            string json = JsonUtility.ToJson(NewUser);
            string url = BaseURL + "/auth/register";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnRegisterSuccess, OnError));

        }
        private void OnClickAppleRegister()
        {
            Show.Log("Apple Register Zone");
        }
        private void OnClickFacebookRegister()
        {
            Show.Log("Facebook Register Zone");
        }
        private void OnRegisterSuccess(string json)
        {
            ui.SetVerificationPage_BtnEvent(OnClickVerification, OnClickRecentCode);
            ui.ShowVerificationPage();
            ui.ShowToast("Register Success", 2f, Color.green);
            Show.Log("Register Success" + json);
        }
        #endregion User Registration

        #region Email Verification
        protected void OnClickVerification()
        {

            API.EmailVerification Verification = new API.EmailVerification();
            Verification.email = NewUser.email;
            Verification.code = ui.GetVerificationCode;

            string json = JsonUtility.ToJson(Verification);
            string url = BaseURL + "/auth/signup/verify";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnEmailVerificationSuccess, OnError));

        }
        private void OnClickRecentCode()
        {
            ui.ShowToast("Comming Soon", 2, Color.red);
        }
        private void OnEmailVerificationSuccess(string json)
        {
            ui.ShowLoginPage();
            ui.ShowToast("Verification Success ", 2f, Color.green);
        }
        #endregion Email Verification

        #region Reset Password Request
        API.PasswordResetRequest PasswordResetRequest;
        private void OnClickResetPasswordRequest()
        {
            PasswordResetRequest = new API.PasswordResetRequest();
            PasswordResetRequest.email = ui.GetResetPasswordReqEmail;
            string json = JsonUtility.ToJson(PasswordResetRequest);
            string url = BaseURL + "/password/create";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnPasswordResetSuccess, OnError));
        }

        private void OnPasswordResetSuccess(string Json)
        {
            API.PasswordResetResponse response = new API.PasswordResetResponse();

            ui.SetVerificationPage_BtnEvent(OnClickResetPasswordVerification, OnClickRecentCode);
            ui.ShowVerificationPage();
            ui.ShowToast(response.message, 2, Color.green);
        }
        private void OnclickPasswordResetToBack()
        {
            ui.ShowLoginPage();
        }

        #endregion Reset Password Request


        #region Password Reset Email Verification
        API.EmailVerification Verification;
        private void OnClickResetPasswordVerification()
        {
            Verification = new API.EmailVerification();
            Verification.email = PasswordResetRequest.email;
            Verification.code = ui.GetVerificationCode;

            string json = JsonUtility.ToJson(Verification);
            string url = BaseURL + "/password/find";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, PasswordResetVerifiSuccess, OnError));
        }
        private void PasswordResetVerifiSuccess(string json)
        {
            ui.ShowSetNewPasswordPage();
            ui.ShowToast("Verifyed", 2, Color.green);
        }
        #endregion Password Reset Email Verification


        #region Set New Password
        private void OnClickSetNewPassword()
        {
            API.SetNewPassword NewPassData = new API.SetNewPassword();
            NewPassData.code = Verification.code;
            NewPassData.password = ui.GetNewPasword;
            NewPassData.password_confirmation = ui.GetNewConfirmPasword;
            NewPassData.email = Verification.email;
            if (!ui.isMached_ResetPassword())
            {
                ui.ShowWarning(new Warning("Password Nor Mached", "Check Confirm Password", null));
                ui.ShowToast("password Not Mached", 2, Color.red);
                return;
            }

            string json = JsonUtility.ToJson(NewPassData);
            string url = BaseURL + "/password/reset";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnResetPasswordSuccess, OnError));
        }

        private void OnResetPasswordSuccess(string json)
        {
            ui.ShowLoginPage();
        }
        #endregion Set New Password


        #region Retive User Data
        public void GetUserInfo(string token)
        {
            //Saving The Token
            Token = token;

            string url = BaseURL;
            StartCoroutine(RestApiHandeler.PostData(url, token, null, OnSuccessRetriveUserdata, OnError));
        }
 
    
        private void OnSuccessRetriveUserdata(string json)
        {
            GetUserInfoStruct getUserInfoStruct = JsonUtility.FromJson<GetUserInfoStruct>(json);
            ui.ShowToast("User Retrive Success ", 2f, Color.green);
        }
        private void OnError(string message)
        {
            ui.ShowToast("Error " + message, 2f, Color.green);
        }
        #endregion Retive User Data
    }
}