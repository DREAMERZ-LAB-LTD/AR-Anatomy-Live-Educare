using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using LoginRegisterSystem;
using System.Collections.Generic;

namespace UI
{
    public class UI_Handeler : MonoBehaviour
    {
        #region Property

#pragma warning disable 649

        private Activity activity;//handel ui transiction
        public WarningHaler Warning_Haler;//handel warning panel
        public static bool isBackFromARScene = false;


        [Header("Reference all of the panel's")]
        [SerializeField] private GameObject GamePanel;

        [SerializeField] private GameObject Login_Page;
        [SerializeField] private GameObject Register_Page;
        [SerializeField] private GameObject VerificationCode_Page;
        [SerializeField] private GameObject ForgotPassword_Page;
        [SerializeField] private GameObject SetNewPassword_Page;
        [SerializeField] private GameObject PasswordResetSuccess_Page;
        [SerializeField] private GameObject SavedUser_Page;
        [SerializeField] private GameObject TostMessage;

        [Header("Password visible & non visible Singn")]
        [SerializeField] private Sprite PasswordVisbleSign;
        [SerializeField] private Sprite PasswordNonVisbleSign;

        [Header("Login Panel's Property")]
        //input fields
        [SerializeField] private TMP_InputField Login_EmailField;//use for Login_Page
        [SerializeField] private TMP_InputField Login_PasswordField;//use for Login_Page
        [Space]
        //buttons
        [SerializeField] private Button Login_Btn;
        [SerializeField] private Button AppleLogin_Btn;
        [SerializeField] private Button FacebookLogin_Btn;
        [SerializeField] private Button ForgotPassword_Btn;
        [SerializeField] private Button GotoRegisterPage_Btn;
        [SerializeField] private Button LoginPasswordVisible_Btn;

        [Header("Registration Panel's Property ")]
        //input fields
        [SerializeField] private TMP_InputField Register_NameField;//use for Register_Page
        [SerializeField] private TMP_InputField Register_EmailField;//use for Register_Page
        [SerializeField] private TMP_InputField Register_PhoneNoField;//use for Register_Page
        [SerializeField] private TMP_InputField Register_PasswordField;//use for Register_Page
        [SerializeField] private TMP_InputField Register_ConfirmPasswordField;//use for Register_Page

        [Space]
        //buttons
        [SerializeField] private Button RegisterToLogin_Btn;//use for Register_Page
        [SerializeField] private Button Register_Btn;//use for Register_Page
        [SerializeField] private Button AppleRegister_Btn;//use for Register_Page
        [SerializeField] private Button FacebookRegister_Btn;//use for Register_Page
        [SerializeField] private Button RegisterToLoginNow_Btn;//use for Register_Page
        [SerializeField] private Button RegisterPasswordVisible_Btn;//use for Register_Page
        [SerializeField] private Button RegisterConfirmPasswordVisible_Btn;//use for Register_Page
        [Space]
        [SerializeField] private TMP_Dropdown Register_CountryCodeField;//use for Register_Page


        [Header("Verification Panel's Property ")]
        //input field
        [SerializeField] private TMP_InputField VerificationCodeField;//use for Verification Page
        [Space]
        //buttons
        [SerializeField] private Button VerifiToLogin_Btn;//use for Verification Page
        [SerializeField] private Button Verify_Btn;//use for Verification Page
        [SerializeField] private Button RecntVerificationCode_Btn;//use for Verification Page


        [Header("ForgotPassword_Page Panel's Property ")]
        //input fileds
        [SerializeField] private TMP_InputField ResetEmail_Field;//use for ForgotPassword_Page
        [Space]
        //buttons
        [SerializeField] private Button ForgotPasswordToBack_Btn;//use for ForgotPassword_Page
        [SerializeField] private Button SentRequest_Btn;//use for ForgotPassword_Page
        [SerializeField] private Button ForgotPasswordGoBack_Btn;//use for ForgotPassword_Page


        [Header("Set New Password Panel's Property ")]
        //input fields
        [SerializeField] private TMP_InputField ResetPassword_Field;//Set New Password Panel
        [SerializeField] private TMP_InputField ConfirmResetPassword_Field;//Set New Password Panel
        [Space]
        //buttons
        [SerializeField] private Button NewPasswordToBack_Btn;//Set New Password Panel
        [SerializeField] private Button ResetPassword_Btn;//Set New Password Panel
        [SerializeField] private Button ResetPasswordVisible_Btn;//Set New Password Panel
        [SerializeField] private Button ResetConfirmPasswordVisible_Btn;//Set New Password Panel

        [Header("Reset Password Success Panel's Property ")]
        //buttons
        [SerializeField] private Button ResetPasswordSuccessToBack_Btn;
        [SerializeField] private Button ResetPasswordSuccessToHome_Btn;


        [Header("Saved User Panel's Property ")]
        //buttons
        [SerializeField] private Button SavedUserLogin;
        [SerializeField] private Button LoginAnother;
        [Space]
        //texts
        [SerializeField] private TMP_Text SavedUserName;
        [SerializeField] private TMP_Text SavedUserEmail;

        [Header("Game Panel's Property ")]
        [SerializeField] private Button LogOutBtn;

        [Header("Loading Panel")]
        [SerializeField] private GameObject LoadingPanel;

#pragma warning restore 649

        #endregion Property


        #region Initilize Zone

        private void Awake()
        {
            ShowLoadingPage(false);
            InitilizePanel();
            ClearAllInputField();
            SetPasswordVisibleBtnEvent();
            LoadCountryList();
            InitilizeDefaultBtnEvent();
        }

        private void InitilizePanel()
        {
            //it will controll all of ui transitions
            activity = new Activity(TostMessage);

            //initialy deactive all of the pages
            GamePanel.SetActive(false);
            Login_Page.SetActive(false);
            Register_Page.SetActive(false);
            VerificationCode_Page.SetActive(false);
            ForgotPassword_Page.SetActive(false);
            SetNewPassword_Page.SetActive(false);
            PasswordResetSuccess_Page.SetActive(false);
            SavedUser_Page.SetActive(false);
            TostMessage.SetActive(false);

            if (AuthManager.isLogedin)
            {
                if (isBackFromARScene)
                {
                    //when user back from AR scene, we need to show game menu panel in run time because an user allready login
                    ShowGamePage();
                }
                else
                {
                    //when app is opening we need to show saved uesr information on saved user panel first time only
                    ShowSavedUserPage();
                }
            }
            else
            {
                ShowLoginPage();
            }
        }

        public void ClearAllInputField()
        {
            Login_EmailField.text = string.Empty;
            Login_PasswordField.text = string.Empty;
            Register_NameField.text = string.Empty;
            Register_EmailField.text = string.Empty;
            Register_PhoneNoField.text = string.Empty;
            Register_PasswordField.text = string.Empty;
            Register_ConfirmPasswordField.text = string.Empty;
            VerificationCodeField.text = string.Empty;
            ResetEmail_Field.text = string.Empty;
            ResetPassword_Field.text = string.Empty;
            ConfirmResetPassword_Field.text = string.Empty;

        }

        private void LoadCountryList()
        {
            List<string> DailCodes = new List<string>();
            foreach (ISO3166Country country in CountryCode.GetALLCountry())
            {
                DailCodes.Add(country.DialCodes[0]);
            }

            Register_CountryCodeField.ClearOptions();
            Register_CountryCodeField.AddOptions(DailCodes);

            StartCoroutine(CountryCode.GetCurentountryJson(Onsuccess, OnError));

            void Onsuccess(string countryJson)
            {
                ISO3166Country country = CountryCode.JsonToObject(countryJson);
                Register_CountryCodeField.value = DailCodes.IndexOf(country.DialCodes[0]);
                Debug.Log("Name " + country.Name + " Alphe2 " + country.Alpha2 + " dail code " + country.DialCodes);

            }
            void OnError(string message)
            {
                Show.Log("Auto Country select Error");
            }


        }

        #endregion Initilize Zone

        #region Buttons Event Setup Zone 

        /// <summary>
        /// initilize all of the default button event as default
        ///default ui only control ui page trasition No other Logic execute with this buttons
        /// </summary>
        private void InitilizeDefaultBtnEvent()
        {
            //remove button's event
            ForgotPassword_Btn.onClick.RemoveAllListeners();//From Login page
            GotoRegisterPage_Btn.onClick.RemoveAllListeners();//From Login page

            RegisterToLogin_Btn.onClick.RemoveAllListeners();//From Register page
            RegisterToLoginNow_Btn.onClick.RemoveAllListeners();//From Register page

            VerifiToLogin_Btn.onClick.RemoveAllListeners();//From Verification page

            ForgotPasswordToBack_Btn.onClick.RemoveAllListeners();//From Password Reset page
            ForgotPasswordGoBack_Btn.onClick.RemoveAllListeners();//From Password Reset page

            NewPasswordToBack_Btn.onClick.RemoveAllListeners();//From Set New Password page

            ResetPasswordSuccessToBack_Btn.onClick.RemoveAllListeners(); //From Reset Password Success page
            ResetPasswordSuccessToHome_Btn.onClick.RemoveAllListeners(); //From Reset Password Success page

            LoginAnother.onClick.RemoveAllListeners(); //From saved user page
            SavedUserLogin.onClick.RemoveAllListeners(); //From saved user page

            //set event into the buttons
            ForgotPassword_Btn.onClick.AddListener(ShowForgotPasswordPage);//From Login page
            GotoRegisterPage_Btn.onClick.AddListener(ShowRegisterPage);//From Login page

            RegisterToLogin_Btn.onClick.AddListener(ShowLoginPage);//From Register page
            RegisterToLoginNow_Btn.onClick.AddListener(ShowLoginPage);//From Register page


            VerifiToLogin_Btn.onClick.AddListener(ShowLoginPage);//From Verification page

            ForgotPasswordToBack_Btn.onClick.AddListener(ShowLoginPage);//From Password Reset page
            ForgotPasswordGoBack_Btn.onClick.AddListener(ShowLoginPage);//From Password Reset page

            NewPasswordToBack_Btn.onClick.AddListener(ShowForgotPasswordPage);//From Set New Password page


            ResetPasswordSuccessToBack_Btn.onClick.AddListener(ShowLoginPage); //From Reset Password Success page
            ResetPasswordSuccessToHome_Btn.onClick.AddListener(ShowLoginPage); //From Reset Password Success page

            LoginAnother.onClick.AddListener(ShowLoginPage); //From saved user page

            //remove all of the password input fields value changed event
            Register_PasswordField.onValueChanged.RemoveAllListeners();
            Register_ConfirmPasswordField.onValueChanged.RemoveAllListeners();
            ResetPassword_Field.onValueChanged.RemoveAllListeners();
            ConfirmResetPassword_Field.onValueChanged.RemoveAllListeners();

            //set all of the password input fields value changed event
            //it will handel input field color when uer enter diffrent password into password field and confirm password field
            Register_PasswordField.onValueChanged.AddListener(OnTypeRegisterPassword);
            Register_ConfirmPasswordField.onValueChanged.AddListener(OnTypeRegisterPassword);
            ResetPassword_Field.onValueChanged.AddListener(OnTypeResetPassword);
            ConfirmResetPassword_Field.onValueChanged.AddListener(OnTypeResetPassword);

        }

        /// <summary>
        /// set password visible buttons event that will control password field content mode
        /// </summary>
        private void SetPasswordVisibleBtnEvent()
        {
            //From Login page
            LoginPasswordVisible_Btn.onClick.RemoveAllListeners();
            //control password input field content mode in login page 
            LoginPasswordVisible_Btn.onClick.AddListener(delegate { ChangePasswordFieldMode(
                                                        LoginPasswordVisible_Btn.GetComponent<Image>(),
                                                        Login_PasswordField); });//From Login page

            //From Register page
            RegisterPasswordVisible_Btn.onClick.RemoveAllListeners();
            //control new password input field content mode in register page 
            RegisterPasswordVisible_Btn.onClick.AddListener(delegate { ChangePasswordFieldMode(
                                                            RegisterPasswordVisible_Btn.GetComponent<Image>(),
                                                            Register_PasswordField); });//From Login page
                                                                                        //From Register page
            RegisterConfirmPasswordVisible_Btn.onClick.RemoveAllListeners();
            //control confirm password input field content mode in register page 
            RegisterConfirmPasswordVisible_Btn.onClick.AddListener(delegate { ChangePasswordFieldMode(
                                                                   RegisterConfirmPasswordVisible_Btn.GetComponent<Image>(),
                                                                   Register_ConfirmPasswordField); });

            //From Set New Password page
            ResetPasswordVisible_Btn.onClick.RemoveAllListeners();
            //control new password input field content mode in Set New Password page 
            ResetPasswordVisible_Btn.onClick.AddListener(delegate { ChangePasswordFieldMode(
                                                                   ResetPasswordVisible_Btn.GetComponent<Image>(),
                                                                   ResetPassword_Field); });
            //From Set New Password page
            ResetConfirmPasswordVisible_Btn.onClick.RemoveAllListeners();
            //control confirm password input field content mode in Set New Password page 
            ResetConfirmPasswordVisible_Btn.onClick.AddListener(delegate { ChangePasswordFieldMode(
                                                                ResetConfirmPasswordVisible_Btn.GetComponent<Image>(),
                                                                ConfirmResetPassword_Field); });

        }

        /// <summary>
        /// Set login page button event where from control login systm
        /// </summary>
        /// <param name="OnClickLogin">From Login Page Login Action Button</param>
        /// <param name="OnClickAppleLogin">From Login Page Apple Login Action Button</param>
        /// <param name="OnClickFaceBookLogin">From Login Page FaceBook Login Action Button</param>
        public void SetLoginPage_BtnEvent(UnityAction OnClickLogin,
                                              UnityAction OnClickAppleLogin,
                                              UnityAction OnClickFaceBookLogin)
        {
            //remove event from button 
            Login_Btn.onClick.RemoveAllListeners();
            AppleLogin_Btn.onClick.RemoveAllListeners();
            FacebookLogin_Btn.onClick.RemoveAllListeners();

            //set button's event
            Login_Btn.onClick.AddListener(OnClickLogin);
            AppleLogin_Btn.onClick.AddListener(OnClickAppleLogin);
            FacebookLogin_Btn.onClick.AddListener(OnClickFaceBookLogin);
        }

        /// <summary>
        /// Set Register page button event where from control register systm
        /// </summary>
        /// <param name="OnClickRegister">From Register Page Register Action Button</param>
        /// <param name="OnClickAppleRegister">From Register Page Apple Register Action Button</param>
        /// <param name="OnClickFacebookRegister">From Register Page Facebook Register Action Button</param>
        public void SetRegisterPage_BtnEvent(UnityAction OnClickRegister,
                                                 UnityAction OnClickAppleRegister,
                                                 UnityAction OnClickFacebookRegister)
        {
            //remove event from button 
            Register_Btn.onClick.RemoveAllListeners();
            AppleRegister_Btn.onClick.RemoveAllListeners();
            FacebookRegister_Btn.onClick.RemoveAllListeners();

            //set button's event
            Register_Btn.onClick.AddListener(OnClickRegister);
            AppleRegister_Btn.onClick.AddListener(OnClickAppleRegister);
            FacebookRegister_Btn.onClick.AddListener(OnClickFacebookRegister);
        }

        /// <summary>
        /// Set Verification page button event where from control Verification systm
        /// </summary>
        /// <param name="OnClickVerify">From Verification Page Verification Action Button</param>
        /// <param name="OnClickRecentCode"></param>
        public void SetVerificationPage_BtnEvent(UnityAction OnClickVerify, UnityAction OnClickRecentCode)
        {
            //remove event from button 
            Verify_Btn.onClick.RemoveAllListeners();
            RecntVerificationCode_Btn.onClick.RemoveAllListeners();

            //set button's event
            Verify_Btn.onClick.AddListener(OnClickVerify);
            RecntVerificationCode_Btn.onClick.AddListener(OnClickRecentCode);
        }

        /// <summary>
        /// Set Reset Password page button event where from control Reset Password systm
        /// </summary>
        /// <param name="OnClickRequest">From Reset Password Page Password Reset Request Action Button</param>
        /// <param name="OnClickGoBack">From Reset Password Page Password Reset page to back Action Button</param>
        public void SetResetPasswordPage_BtnEvent(UnityAction OnClickRequest, UnityAction OnClickGoBack)
        {
            //remove event from button 
            SentRequest_Btn.onClick.RemoveAllListeners();
            ForgotPasswordGoBack_Btn.onClick.RemoveAllListeners();

            //set button's event
            SentRequest_Btn.onClick.AddListener(OnClickRequest);
            ForgotPasswordGoBack_Btn.onClick.AddListener(OnClickGoBack);
        }

        /// <summary>
        /// Set New Reset Password page button event where from control Set New Password systm
        /// </summary>
        /// <param name="OnClickResetPassword">From New Reset Password Page New Password Set Action Button</param>
        public void SetNewResetPasswordPage_BtnEvent(UnityAction OnClickResetPassword)
        {
            //remove event from button 
            ResetPassword_Btn.onClick.RemoveAllListeners();

            //set button's event
            ResetPassword_Btn.onClick.AddListener(OnClickResetPassword);
        }

        /// <summary>
        /// Set Logout button event where from control Logout systm
        /// </summary>
        /// <param name="OnClickLogOut">Logout action Method</param>
        public void SetLogOutBtnEvent(UnityAction OnClickLogOut)
        {
            //remove event from button 
            LogOutBtn.onClick.RemoveAllListeners();

            //set button's event
            LogOutBtn.onClick.AddListener(OnClickLogOut);
        }


        public void SetSavedUserLoginEvent(UnityAction GetSavedUser)
        {
            SavedUserLogin.onClick.AddListener(GetSavedUser); //From saved user page
        }
#endregion Buttons Event Setup Zone 


#region Information Checker Zone

        private void ChangePasswordFieldMode(Image buttonTexture, TMP_InputField passwordField)
        {
            bool isPasswordMode = passwordField.contentType == TMP_InputField.ContentType.Password;
            buttonTexture.sprite = isPasswordMode ? PasswordNonVisbleSign : PasswordVisbleSign;
            passwordField.contentType = isPasswordMode ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            passwordField.ForceLabelUpdate();
            Debug.Log("MOde Changed");
        }

        /// <summary>
        /// return true when user full fill up register form in register panel
        /// </summary>
        public bool isValidRegisterInfo => GetRegisterName != "" && GetRegisterEmail != "" && GetRegisterPassword != ""&& Register_PhoneNoField.text != "";
       
        //Check  confirm password and change color in runtime when user tying on input field in Register Pagee
        private void OnTypeRegisterPassword(string confirmPass) => isMached_RegisterPassword();

        /// <summary>
        /// return true if password and confirm password are mached in user register page
        /// </summary>
        /// <returns></returns>
        public bool isMached_RegisterPassword()
        {
            bool isMatched = Register_PasswordField.text !="" && Register_PasswordField.text == Register_ConfirmPasswordField.text;
            Register_ConfirmPasswordField.textComponent.color = isMatched ? Color.black : Color.red;

            return isMatched;
        }

        //Check  confirm password and change color in runtime when user tying on input field in Reset Pawwword Pagee
        private void OnTypeResetPassword(string confirmPass) => isMached_ResetPassword();

        /// <summary>
        /// return true if password and confirm password are mached in Reset password page
        /// return true if password and confirm password are mached in Reset password page
        /// </summary>
        /// <returns></returns>
        public bool isMached_ResetPassword()
        {
            bool isMatched = ResetPassword_Field.text!="" && ResetPassword_Field.text == ConfirmResetPassword_Field.text;
            ConfirmResetPassword_Field.textComponent.color = isMatched ? Color.black : Color.red;

            return isMatched;

        }
#endregion Information Checker Zone


#region Get Access To Authentication Manager
        /// <summary>
        /// return Login email from login page email field
        /// </summary>
        public string GetLoginEmail => Login_EmailField.text;
        /// <summary>
        /// return Login password from login page password field
        /// </summary>
        public string GetLoginPassword => Login_PasswordField.text;


        /// <summary>
        /// return user name from register page name field
        /// </summary>
        public string GetRegisterName => Register_NameField.text;
        /// <summary>
        /// return email from register page email field
        /// </summary>
        public string GetRegisterEmail => Register_EmailField.text;
        /// <summary>
        ///  return phone numer from register page phone numer field
        /// </summary>
        public string GetRegisterPhoneNumber => Register_CountryCodeField.options[Register_CountryCodeField.value].text + Register_PhoneNoField.text;
        /// <summary>
        ///  return password from register page password field
        /// </summary>
        public string GetRegisterPassword => Register_PasswordField.text;



        /// <summary>
        ///  return email from reset password request page email field
        /// </summary>
        public string GetResetPasswordReqEmail => ResetEmail_Field.text;

        /// <summary>
        ///  return new password from set new password page new password field
        /// </summary>
        public string GetNewPasword => ResetPassword_Field.text;
     
        /// <summary>
        /// return verification code from Verification page Verification code field
        /// </summary>
        public string GetVerificationCode =>  VerificationCodeField.text;
#endregion Get Access To Authentication Manager

#region Activity Panel Transiction
        public void ShowLoginPage()=> activity.Show(Login_Page);
        public void ShowRegisterPage()=>activity.Show(Register_Page);
        public void ShowVerificationPage()=> activity.Show(VerificationCode_Page);
        public void ShowForgotPasswordPage()=> activity.Show(ForgotPassword_Page);
        public void ShowSetNewPasswordPage()=> activity.Show(SetNewPassword_Page);
        public void ShowPasswordResetSuccessPage()=> activity.Show(PasswordResetSuccess_Page);
        public void ShowGamePage()=> activity.Show(GamePanel);
        public void ShowLoadingPage(bool show) => LoadingPanel.SetActive(show);
        public void ShowSavedUserPage()
        {
            GetUserInfoStruct LastSavedUer = AuthManager.SavedUser;
            SavedUserName.text = LastSavedUer.data.name;
            SavedUserEmail.text = LastSavedUer.data.email;
            activity.Show(SavedUser_Page);

        }
        public void ShowToast(string message, float duration, Color color) => activity.ShowToast(message, duration, color);
#endregion Activity Panel Transiction

    }
}
