using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Security.Cryptography;
using UnityEngine;
using Facebook.Unity;
using UI;

namespace LoginRegisterSystem
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Sever Root URL")]
        [SerializeField] protected string BaseURL = "http://18.140.117.58/live-educare/public/api";
        [SerializeField] private UI_Handeler ui;

        #region Initilize
        private void Start()
        {
            ui.SetRegisterPage_BtnEvent(OnClickRegister, OnClickAppleRegister, OnClickFacebookLogin);
            ui.SetLoginPage_BtnEvent(OnClickLogin, OnClickAppleLogin, OnClickFacebookLogin);
            ui.SetResetPasswordPage_BtnEvent(OnClickResetPasswordRequest, OnclickPasswordResetToBack);
            ui.SetNewResetPasswordPage_BtnEvent(OnClickSetNewPassword);
            ui.SetLogOutBtnEvent(OnClickLogOut);
            ui.SetSavedUserLoginEvent(GetUserInfo);
            InitilizeFaceook();
        }
        #endregion Initilize


        #region Saved User
        /// <summary>
        /// return true if an user allready login
        /// </summary>
        public static bool isLogedin //=> Token != "";
        {
            get
            {
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
                return PlayerPrefs.GetString("Token");
            }
            private set
            {
                PlayerPrefs.SetString("Token", value);
            }
        }

        /// <summary>
        /// return Last Login User that was save in cash
        /// </summary>
        public static GetUserInfoStruct SavedUser
        {
            set 
            {
                GetUserInfoStruct NewUser = value;
                PlayerPrefs.SetString("Name", NewUser.data.name);
                PlayerPrefs.SetString("Email", NewUser.data.email);

            }
            get 
            {
                GetUserInfoStruct LastUser = new GetUserInfoStruct();
                LastUser.data.name = PlayerPrefs.GetString("Name");
                LastUser.data.email = PlayerPrefs.GetString("Email");
                return LastUser;
            }
          
        
        }

        #endregion Saved User

        #region User Login
        [SerializeField]
        SuccessLogIn loginresponse;
        protected void OnClickLogin()
        {
            if (!ui.isValidLoginInfo)
            {
                return;
            }

            OnClickLogOut();
            LogInStruct user = new LogInStruct();
            user.email = ui.GetLoginEmail;
            user.password = ui.GetLoginPassword;

            string Json = JsonUtility.ToJson(user);
            string url = BaseURL + "/auth/login";
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnLoginSuccess, OnLoginError));
           
            //Login Success Callack
            void OnLoginSuccess(string json)
            {
                ui.ShowLoadingPage(false);
                
                loginresponse = JsonUtility.FromJson<SuccessLogIn>(json);
                Token = loginresponse.data.token;
                GetUserInfo();
            

                ui.ShowGamePage();
                ui.ShowToast("Login Success", 2f, Color.green);
            }

            //Login Error Callack
            void OnLoginError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                    return;
                }

                ErrorLogIn errorLogIn = JsonUtility.FromJson<ErrorLogIn>(json);
                ui.ShowToast(errorLogIn.error, 2f, Color.red);
            }
        }
   
        #endregion User Login

        #region User LogOut
        private void OnClickLogOut()
        {
            Token = "";
            ui.ShowLoginPage();
        }
        #endregion User LogOut

        #region User Registration

        Registation NewUser;//current Registred user data store for access to next step 
        private void OnClickRegister()
        {
            if (!ui.isValidRegisterInfo)
            {
                return;
            }

            if (!ui.isMached_RegisterPassword())
            {
                ui.Warning_Haler.PasswordError();
                return;
            }


            NewUser = new Registation();
            NewUser.name = ui.GetRegisterName;
            NewUser.email = ui.GetRegisterEmail;
            NewUser.phone = ui.GetRegisterPhoneNumber;
            NewUser.password = ui.GetRegisterPassword;
            NewUser.confirm_password = ui.GetRegisterPassword;

            string Json = JsonUtility.ToJson(NewUser);
            string url = BaseURL + "/auth/register";
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnRegisterSuccess, OnRegisterError));

            //register success callack
            void OnRegisterSuccess(string json)
            {
                SuccessRegistation successRegistation = JsonUtility.FromJson<SuccessRegistation>(json);
               
                ui.SetVerifyEmail = NewUser.email;
                ui.ShowLoadingPage(false);
                ui.SetVerificationPage_BtnEvent(OnClickVerification, OnClickRecentCode);
                ui.ShowVerificationPage();
                ui.ShowToast("Register Success" , 2f, Color.green);
             
            }
            //register error callack
            void OnRegisterError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                    return;
                }

                ErrorRegistation errorRegistation = JsonUtility.FromJson<ErrorRegistation>(json);
                string blackstring = "";
                foreach (string str in errorRegistation.error.email)
                {
                    blackstring += str + "\n";
                }
                foreach (string str in errorRegistation.error.phone)
                {
                    blackstring += str + "\n";
                }
                foreach (string str in errorRegistation.error.confirm_password)
                {
                    blackstring += str + "\n";
                }
                ui.ShowToast(blackstring, 2f, Color.red);
            }
        }
        private void OnClickAppleRegister()
        {
            ui.ShowToast("Coming soon", 2, Color.yellow);
        }
    
        #endregion User Registration

        #region Email Verification
        protected void OnClickVerification()
        {

            VeryficationCode Verification = new VeryficationCode();
            Verification.email = NewUser.email;
            Verification.code = ui.GetVerificationCode;

            string Json = JsonUtility.ToJson(Verification);
            string url = BaseURL + "/auth/signup/verify";
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnEmailVerificationSuccess, OnEmailVerificationError));

        void OnEmailVerificationSuccess(string json)
        {
                ui.ShowLoadingPage(false);
                ui.ShowLoginPage();
            ui.ShowToast("Verification Success ", 2f, Color.green);
        } 
        void OnEmailVerificationError(string json)
        {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                }
                ui.ShowToast("Email verification fail", 2f, Color.red);
        }
        }
        private void OnClickRecentCode()
        {
            ui.ShowToast("Comming Soon", 2, Color.red);
        }

        #endregion Email Verification

        #region Reset Password Request

        ResetPasswordEmail PasswordResetRequest;
        private void OnClickResetPasswordRequest()
        {
            PasswordResetRequest = new ResetPasswordEmail();
            PasswordResetRequest.email = ui.GetResetPasswordReqEmail;
           
            string Json = JsonUtility.ToJson(PasswordResetRequest);
            string url = BaseURL + "/password/create";
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnPasswordResetRequestSuccess, OnPasswordResetRequestError));
          
            void OnPasswordResetRequestSuccess(string json)
            {
                //ErrorResetPasswordVerifyCode response = new ErrorResetPasswordVerifyCode();
                ui.ShowLoadingPage(false);
                ui.SetVerificationPage_BtnEvent(OnClickResetPasswordVerification, OnClickRecentCode);
                ui.ShowVerificationPage();
                ui.ShowToast("Check email", 2, Color.green);
            }

            void OnPasswordResetRequestError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                    return;
                }

            
                ErrorResetPasswordEmail errorResetPasswordEmail = JsonUtility.FromJson<ErrorResetPasswordEmail>(json);
                ui.ShowToast("Check this email" + errorResetPasswordEmail.error.email, 2f, Color.red);
            }
        }

        private void OnclickPasswordResetToBack()
        {
            ui.ShowLoginPage();
        }

        #endregion Reset Password Request

        #region Reset Password Email Verification

        ResetPasswordVerifyCode Verification;
        private void OnClickResetPasswordVerification()
        {
            Verification = new ResetPasswordVerifyCode();
            Verification.email = PasswordResetRequest.email;
            Verification.code = ui.GetVerificationCode;

            string Json = JsonUtility.ToJson(Verification);
            string url = BaseURL + "/password/find";
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, PasswordResetVerifiSuccess, PasswordResetVerifiError));
         
            void PasswordResetVerifiSuccess(string json)
            {
                ui.ShowLoadingPage(false);
                ui.ShowSetNewPasswordPage();
                ui.ShowToast("Verifyed", 2, Color.green);
            }

            void PasswordResetVerifiError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                    return;
                }

              //  ErrorResetPasswordVerifyCode errorResetPasswordVerifyCode = JsonUtility.FromJson<ErrorResetPasswordVerifyCode>(json);
              
                ui.ShowToast("Password reset fail", 2f, Color.red);
            }
        }
        #endregion Reset Password Email Verification

        #region Set New Password
        private void OnClickSetNewPassword()
        {
            if (!ui.isMached_ResetPassword())
            {
                ui.Warning_Haler.PasswordError();
                ui.ShowToast("password Not Mached", 2, Color.red);
                return;
            }

            ResetPasswordNewPassword NewPassData = new ResetPasswordNewPassword();
            NewPassData.code = Verification.code;
            NewPassData.password = ui.GetNewPasword;
            NewPassData.password_confirmation = ui.GetNewPasword;
            NewPassData.email = Verification.email;
           

            string Json = JsonUtility.ToJson(NewPassData);
            string url = BaseURL + "/password/reset";
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnResetPasswordSuccess, OnResetPasswordError));
          
            //new Password success callack
            void OnResetPasswordSuccess(string json)
            {
                ui.ShowLoadingPage(false);
                ui.ShowLoginPage();
            }
            //new Password error callack
            void OnResetPasswordError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                }
                ui.ShowToast("Email verification fail", 2f, Color.red);
            }
        }

        #endregion Set New Password

        #region Retive User Data
        public void GetUserInfo()
        {
            ui.ShowLoadingPage(true);

            //if saved user was loagged in from facebook then no need to retrive from our akend server, Couse user will be exist in faceook server
            if (SavedUser.data.email == "Facebook")
            {
                ui.ShowLoadingPage(false);
                ui.ShowGamePage();
                return;
            }

            string url = BaseURL + "/details";
            StartCoroutine(RestApiHandeler.PostData(url, Token, null, OnSuccessRetriveUserdata, OnUserRetriveError));
       
            void OnSuccessRetriveUserdata(string json)
            {
                GetUserInfoStruct user = JsonUtility.FromJson<GetUserInfoStruct>(json);
                SavedUser = user;//save user info into cash

                ui.ShowLoadingPage(false);
                ui.ShowGamePage();
            }
            void OnUserRetriveError(string message)
            {
                ui.ShowLoadingPage(false);
                if (message == RestApiHandeler.InternetError)
                    {
                        ui.Warning_Haler.ConnnectionError();
                    return;
                    }
                    ui.ShowToast("User Data Not Found", 2f, Color.red);
            }
        }


        #endregion Retive User Data



        #region FacebookLogin
        private void InitilizeFaceook()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(() =>
                {
                    if (FB.IsInitialized)
                    {
                        Show.Log("IsInitialized");
                        FB.ActivateApp();
                    }
                    else
                    {
                        Show.Log("Couldn't initialize");
                    }
                },
                isGameShown =>
                {
                    if (!isGameShown)
                    {
                        Show.Log("IS not Game Shown");
                    }
                    else
                        Show.Log("IS Game Shown");
                });
            }
            else
            {
                Show.Log("IsInitialized");
                FB.ActivateApp();
            }
        }

        private void OnClickFacebookLogin()
        {
            ui.ShowLoadingPage(true);

            IEnumerable<string> Permissions = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(Permissions, Auth_Callback);

            void Auth_Callback(ILoginResult result)
            {
                if (FB.IsLoggedIn)
                {
                    FB.API("/me?fields=id,name,email", HttpMethod.GET, GetUserInfo);//sent request to faceook for retrive user data
                }
                else
                {
                    ui.ShowToast("Login cancelled ", 2, Color.red);
                }
            }

            void GetUserInfo(IResult userInfo)
            {
                ui.ShowLoadingPage(false);

                if (userInfo.Error != null)
                {
                    ui.ShowToast(userInfo.Error, 2, Color.red);//pass error message
                    return;
                }

                GetUserInfoStruct facebookUser = new GetUserInfoStruct();
                facebookUser.data.name = userInfo.ResultDictionary["name"].ToString();
                //facebookUser.data.id = userInfo.ResultDictionary["id"].ToString();
                //facebookUser.data.email = userInfo.ResultDictionary["email"].ToString();
                facebookUser.data.email = "Facebook";

                SavedUser = facebookUser;
                Token = Facebook.Unity.AccessToken.CurrentAccessToken.ToString(); // Get access token from faceook
                ui.ShowGamePage();
                ui.ShowToast("Facebook Login Success", 2, Color.green);//pass error message
                Show.Log("Token" + Token);
            }
        }
        #endregion FacebookLogin



        #region AppleLogin
        private void OnClickAppleLogin()
        {
            ui.ShowToast("Coming soon", 2, Color.yellow);
        }
        #endregion AppleLogin


    }
}