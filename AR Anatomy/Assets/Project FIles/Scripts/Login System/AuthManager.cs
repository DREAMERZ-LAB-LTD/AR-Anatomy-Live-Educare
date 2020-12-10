using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Facebook.Unity;
using UI;
using Facebook.Unity;

namespace LoginRegisterSystem
{
    public class AuthManager : MonoBehaviour
    {
      //  [SerializeField] protected string BaseURL = "http://54.254.22.111/live-educare/public/api";

#pragma warning disable 649
        [SerializeField] private UI_Handeler ui;
    

       private appAttributes URLData => RemoteConfig.URLData;
#pragma warning restore 649

        #region Initilize
        private void Start()
        {
            ui.RegisterPanel.SetButtonEvents(OnClickRegister, OnClickAppleRegister, OnClickFacebookLogin);
            ui.LoginPanel.SetButtonEvents(OnClickLogin, OnClickAppleLogin, OnClickFacebookLogin);
            ui.ForgotPasswordPanel.SetButtonEvents(OnClickResetPasswordRequest);
            ui.NewPasswordPanel.SetResetBtnEvent(OnClickSetNewPassword);
            ui.SaveduserPanel.SetButtonEvents(delegate { GetUserInfo(false); });
            ui.SetLogOutBtnEvent(OnClickLogOut);
            InitilizeFaceook();
        }
        #endregion Initilize

        #region Saved User
        /// <summary>
        /// return true if an user allready loggedin
        /// </summary>
        public static bool isLoggedin //=> Token != "";
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
        SuccessLogIn loginresponse;
        protected void OnClickLogin()
        {
            if (!ui.LoginPanel.isValidLoginInfo)
            {
                ui.ShowToast("Enter email and password", 2,  Color.red);
                return;
            }

            LogInStruct user = new LogInStruct();
            user.email = ui.LoginPanel.GetUserEmail;
            user.password = ui.LoginPanel.GetUserPassword;

            UnAuthorizedmail = user.email;//store email for unauthorized verification

            ui.ShowLoadingPage(true);

            string Json = JsonUtility.ToJson(user);
            //string url = BaseURL + "/auth/login";
            string url = URLData.BaseURL + URLData.Login;
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnLoginSuccess, OnLoginError));
           
            //Login Success Callack
            void OnLoginSuccess(string json)
            {
                OnClickLogOut();
                ui.ShowLoadingPage(false);
                
                loginresponse = JsonUtility.FromJson<SuccessLogIn>(json);
                Token = loginresponse.data.token;
                GetUserInfo();
            
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
                else if (RestApiHandeler.isUnAuthError(json))
                {
                    UserReAuthorize();
                    ui.VerificationPage.SetBtnEvents(OnClickVerification, UserReAuthorize);
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
            SavedUser = new GetUserInfoStruct();
            ui.ShowToast("Logout success", 2, Color.green);
        }
        #endregion User LogOut

        #region User Registration

        Registation NewUser;//current Registred user data store for access to next step 
        private void OnClickRegister()
        {
            if (!ui.RegisterPanel.isValidinInfo)
            {
                ui.Warning_Haler.RegisterError("Please complete the form with valid information & submit again");
                return;
            } else if (!ui.RegisterPanel.isMached_Password())
            {
                ui.Warning_Haler.PasswordError();
                return;
            }
            else if (!ui.RegisterPanel.isValidPassword)
            {
                ui.ShowToast("Your password must be at last 8 digits", 2, Color.red);
                return;
            }
            

            NewUser = new Registation();
            NewUser.name = ui.RegisterPanel.GetUerName;
            NewUser.email = ui.RegisterPanel.GetUserEmail;
            NewUser.phone = ui.RegisterPanel.GetUserPhoneNumber;
            NewUser.password = ui.RegisterPanel.GetUserPassword;
            NewUser.confirm_password = ui.RegisterPanel.GetUserPassword;

            UnAuthorizedmail = NewUser.email;//store email for unauthorized verification

            string Json = JsonUtility.ToJson(NewUser);
            //string url = BaseURL + "/auth/register";
            string url = URLData.BaseURL + URLData.SignUp;
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnRegisterSuccess, OnRegisterError));

            //register success callack
            void OnRegisterSuccess(string json)
            {
                ui.VerificationPage.SetBtnEvents(OnClickVerification, UserReAuthorize);
                SuccessRegistation successRegistation = JsonUtility.FromJson<SuccessRegistation>(json);
               
                ui.VerificationPage.SetVerificationEmail = NewUser.email;
                ui.ShowLoadingPage(false);
                ui.ShowVerificationPage();
                ui.ShowToast("Registration Success", 2f, Color.green);
             
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
                else if (RestApiHandeler.isUnAuthError(json))
                {
                    UserReAuthorize();
                    ui.VerificationPage.SetBtnEvents(OnClickVerification, UserReAuthorize);
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

        #region ReAuthentication
        private static string UnAuthorizedmail;//store UnAuthorized user email
        
        private void UserReAuthorize()
        {
            ui.ShowLoadingPage(true);
           
            RegistationResetVerifyEmail UnAuthorizedUser = new RegistationResetVerifyEmail();
            UnAuthorizedUser.email = UnAuthorizedmail;
            string Json = JsonUtility.ToJson(UnAuthorizedUser);
            //string url = BaseURL + "/auth/resend/otp";
            string url = URLData.BaseURL + URLData.ResedOTP;
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnRecendCodeSuccess, OnRecendCodeError));

            void OnRecendCodeSuccess(string json)
            { 
                ui.ShowLoadingPage(false);
                ui.ShowVerificationPage();
                ui.VerificationPage.SetVerificationEmail = UnAuthorizedUser.email;
                ui.ShowToast("Verifiation code has been sent", 2,Color.green);
            }
            void OnRecendCodeError(string json)
            {
                ui.ShowLoadingPage(false);
            }
        }

        #endregion ReAuthentication

        #region Email Verification
        private void OnClickVerification()
        {

            VeryficationCode Verification = new VeryficationCode();
            Verification.email = UnAuthorizedmail;
            Verification.code = ui.VerificationPage.GetVerificationCode;

            string Json = JsonUtility.ToJson(Verification);
           // string url = BaseURL + "/auth/signup/verify";
            string url = URLData.BaseURL + URLData.EmailVerification;
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
                    return;
                }
                ui.ShowToast("verification fail", 2f, Color.red);
            }
        }

        #endregion Email Verification

        #region Reset Password Request

        ResetPasswordEmail PasswordResetRequest;
        private void OnClickResetPasswordRequest()
        {
            PasswordResetRequest = new ResetPasswordEmail();
            PasswordResetRequest.email = ui.ForgotPasswordPanel.GetUserEmail;
            SentPasswordResetRequest();
        }
        private void SentPasswordResetRequest()
        {
            string Json = JsonUtility.ToJson(PasswordResetRequest);
            //string url = BaseURL + "/password/create";
            string url = URLData.BaseURL + URLData.PasswordResetRequest;
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnPasswordResetRequestSuccess, OnPasswordResetRequestError));
          
            void OnPasswordResetRequestSuccess(string json)
            {
                //ErrorResetPasswordVerifyCode response = new ErrorResetPasswordVerifyCode();
                ui.ShowLoadingPage(false);
                ui.VerificationPage.SetBtnEvents(OnClickResetPasswordVerification, SentPasswordResetRequest);
                ui.VerificationPage.SetVerificationEmail = PasswordResetRequest.email;
                ui.ShowVerificationPage();
                ui.ShowToast("Verifiation code has been sent", 2, Color.green);
            }

            void OnPasswordResetRequestError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                    return;
                }

            
                //ErrorResetPasswordEmail errorResetPasswordEmail = JsonUtility.FromJson<ErrorResetPasswordEmail>(json);
                ui.ShowToast("User not found" , 2f, Color.red);
            }
        }

  

        #endregion Reset Password Request

        #region Reset Password Email Verification

        ResetPasswordVerifyCode Verification;
        private void OnClickResetPasswordVerification()
        {
            Verification = new ResetPasswordVerifyCode();
            Verification.email = PasswordResetRequest.email;
            Verification.code = ui.VerificationPage.GetVerificationCode;

            string Json = JsonUtility.ToJson(Verification);
            //string url = BaseURL + "/password/find";
            string url = URLData.BaseURL + URLData.PasswordResetRequestVerication;
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, PasswordResetVerifiSuccess, PasswordResetVerifiError));
         
            void PasswordResetVerifiSuccess(string json)
            {
                ui.ShowLoadingPage(false);
                ui.ShowSetNewPasswordPage();
                ui.ShowToast("Verification Success ", 2f, Color.green);
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

                ui.ShowToast("verification fail", 2f, Color.red);
            }
        }
        #endregion Reset Password Email Verification

        #region Set New Password
        private void OnClickSetNewPassword()
        {
            if (!ui.NewPasswordPanel.isMached_Password())
            {
                ui.Warning_Haler.PasswordError();
                ui.ShowToast("Your password must be at last 8 digits", 2, Color.red);
                return;
            }
            else if (!ui.NewPasswordPanel.isValidPassword)
            {
                ui.ShowToast("Your password must be at last 8 digits", 2, Color.red);
                return;
            }

            ResetPasswordNewPassword NewPassData = new ResetPasswordNewPassword();
            NewPassData.code = Verification.code;
            NewPassData.password = ui.NewPasswordPanel.GetNewPasword;
            NewPassData.password_confirmation = ui.NewPasswordPanel.GetNewPasword;
            NewPassData.email = Verification.email;
           

            string Json = JsonUtility.ToJson(NewPassData);
           // string url = BaseURL + "/password/reset";
            string url = URLData.BaseURL + URLData.PasswordReset;
            ui.ShowLoadingPage(true);
            StartCoroutine(RestApiHandeler.PostData(url, null, Json, OnResetPasswordSuccess, OnResetPasswordError));
          
            //new Password success callack
            void OnResetPasswordSuccess(string json)
            {
                ui.ShowLoadingPage(false);
                ui.ShowPasswordResetSuccessPage();
            }
            //new Password error callack
            void OnResetPasswordError(string json)
            {
                ui.ShowLoadingPage(false);
                if (json == RestApiHandeler.InternetError)
                {
                    ui.Warning_Haler.ConnnectionError();
                }
                ui.ShowToast("Verification fail", 2f, Color.red);
            }
        }

        #endregion Set New Password

        #region Retive User Data
        public void GetUserInfo(bool showSavedUser = true)
        {
            ui.ShowLoadingPage(true);

          /*  //if saved user was loagged in from facebook then no need to retrive from our akend server, Couse user will be exist in faceook server
            if (SavedUser.data.email == "Facebook")
            {
                ui.ShowLoadingPage(false);
                ui.ShowGamePage();
                return;
            }
          */
            string url = URLData.BaseURL + URLData.GetUserInfo;
            StartCoroutine(RestApiHandeler.PostData(url, Token, null, OnSuccessRetriveUserdata, OnUserRetriveError));
       
            void OnSuccessRetriveUserdata(string json)
            {
                GetUserInfoStruct user = JsonUtility.FromJson<GetUserInfoStruct>(json);
                SavedUser = user;//save user info into cash
                ui.ShowLoadingPage(false);

              
                if (showSavedUser)
                {
                    ui.ShowSavedUser();
                }
                else
                { 
                    ui.ShowGamePage();
                }
            }
            void OnUserRetriveError(string message)
            {
                ui.ShowLoadingPage(false);
                ui.ShowToast("Fail", 2f, Color.red);

                if (message == RestApiHandeler.InternetError)
                {
                   ui.Warning_Haler.ConnnectionError();
                }
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
                    ui.ShowLoadingPage(false);
                }
            }

            void GetUserInfo(IResult userInfo)
            {

                if (userInfo.Error != null)
                {
                    ui.ShowLoadingPage(false);
                    ui.ShowToast(userInfo.Error, 2, Color.red);//pass error message
                    Show.Log("an Error");
                    return;
                }
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                Token = aToken.TokenString;
                FaceBookApi();

;
                GetUserInfoStruct facebookUser = new GetUserInfoStruct();
                facebookUser.data.name = userInfo.ResultDictionary["name"].ToString();
                facebookUser.data.email = "Facebook";
                //facebookUser.data.id = userInfo.ResultDictionary["id"].ToString();
                //facebookUser.data.email = userInfo.ResultDictionary["email"].ToString();
               // Token = Facebook.Unity.AccessToken.CurrentAccessToken.ToString(); // Get access token from faceook

                SavedUser = facebookUser;
                ui.ShowSavedUser();
                ui.ShowToast("Facebook Login Success", 2, Color.green);//pass error message
            }
        }

        
        private void FaceBookApi()
        {
            FacebookStruct facebookStruct;
            facebookStruct.provider = "facebook";
            facebookStruct.access_token = Token;

            ui.ShowLoadingPage(true);

            string json = JsonUtility.ToJson(facebookStruct);
            string url = RemoteConfig.URLData.SocialLogin;
            StartCoroutine(RestApiHandeler.PostData(url, null, json, FacebookLoginSuccessCallBack, FacebookLoginErrorCallBack));

            Show.Log(json);
        }

        [SerializeField] SuccessFacebookStruct successFacebookStruct;
        private void FacebookLoginSuccessCallBack(string val)
        {
            ui.ShowLoadingPage(false);
            successFacebookStruct = JsonUtility.FromJson<SuccessFacebookStruct>(val);
            Token = successFacebookStruct.access_token;
            GetUserInfo();
        }
        private void FacebookLoginErrorCallBack(string val)
        {
            ui.ShowLoadingPage(false);
            // calling the popupwith message
            if (val == "0")
            {
                string redtext = "No internet connection";
                string blacktext = "Please make sure that Wi-Fi or mobile data is turned on, then try again";
                ui.Warning_Haler.ConnnectionError();
                Show.Log(val);
            }
            else
            {
                Warning warniing = new Warning("Can not login", "Please try again", null);
                
                ui.Warning_Haler.ShowWarning(warniing);
                //_logInSystemUIHolder.OpenPoPup(1, redtext, blacktext);
                Show.Log(val);
            }

            //_logInSystemUIHolder.LogIn();
           // PlayerPrefs.DeleteKey("token");
           // Show.Log("Delete Player Prefs " + PlayerPrefs.GetString("token", ""));
           // Show.Log(val);
        }

        
        #endregion FacebookLogin

        #region AppleLogin
        private void OnClickAppleLogin()
        {
           // ui.ShowToast("Coming soon", 2, Color.yellow);
        }

        string backupJSON;
        public void AppleSignUP(AppleSignUpStruct appleStruct)
        {
            ui.ShowLoadingPage(true);
            appleStruct.provider = "Apple";
            string json = JsonUtility.ToJson(appleStruct);
            backupJSON = json;
            string url = URLData.BaseURL + "/auth/apple-login";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, AppleLoginSuccessCallBack, AppleLoginErrorCallBack));

            Show.Log(json);
        }


        public void AppleLogin(AppleLoginStruct appleStruct)
        {
            ui.ShowLoadingPage(true);
            appleStruct.provider = "Apple";
            string json = JsonUtility.ToJson(appleStruct);
            backupJSON = json;
            string url = URLData.BaseURL + "/auth/apple-login";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, AppleLoginSuccessCallBack, AppleLoginErrorCallBack));

            Show.Log(json);
        }



        // [SerializeField] SuccessFacebookStruct successFacebookStruct;
        public void AppleLoginSuccessCallBack(string val)
        {
            ui.ShowLoadingPage(false);
            Show.Log(val);

            successFacebookStruct = JsonUtility.FromJson<SuccessFacebookStruct>(val);
            Token = successFacebookStruct.access_token;
            GetUserInfo();
        }
        public void AppleLoginErrorCallBack(string val)
        {
            ui.ShowLoadingPage(false);
            // calling the popupwith message
            if (val == "0")
            {
                ui.Warning_Haler.ConnnectionError();
                Show.Log(val);
            }
            else
            {
                ui.Warning_Haler.PasswordError();/*********************************it will change******************************/
                Show.Log(val);
            }

            ui.ShowLoginPage();
            Token = "";
            Show.Log(val);
        }

        #endregion AppleLogin

    }
}