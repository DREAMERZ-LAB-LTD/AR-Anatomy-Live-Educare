using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace UI
{
    public class UI_Handeler : MonoBehaviour
    {
        #region Property

#pragma warning disable 649


        private Activity activity;//handel ui transiction
        [SerializeField] private WarningHaler Warning_Haler;//handel warning panel

        [Header("Reference all of the panel's")]
        [SerializeField] private GameObject Login_Page;
        [SerializeField] private GameObject Register_Page;
        [SerializeField] private GameObject VerificationCode_Page;
        [SerializeField] private GameObject ForgotPassword_Page;
        [SerializeField] private GameObject SetNewPassword_Page;
        [SerializeField] private GameObject PasswordResetSuccess_Page;
        [SerializeField] private GameObject TostMessage;


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

        [Header("Reset Password Success Panel's Property ")]
        //buttons
        [SerializeField] private Button ResetPasswordSuccessToBack_Btn;
        [SerializeField] private Button ResetPasswordSuccessToHome_Btn;

        [Header("Warning Panel's Property ")]
        [SerializeField] private Button Okay_Btn;

      
#pragma warning restore 649

        #endregion Property

        [System.Obsolete]
        void Awake()
        {
            Initilize();
            
        }

        //initilize all of the property as default
        [System.Obsolete]
        private void Initilize()
        { 
            activity = new Activity(TostMessage);

            //initialy deactive all of the pages
            Login_Page.SetActive(false); ;
            Register_Page.SetActive(false); ;
            VerificationCode_Page.SetActive(false); ;
            ForgotPassword_Page.SetActive(false); ;
            SetNewPassword_Page.SetActive(false); ;
            PasswordResetSuccess_Page.SetActive(false); 
            TostMessage.SetActive(false);




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

            Okay_Btn.onClick.RemoveAllListeners(); //From warning page




            //set event into the buttons
            ForgotPassword_Btn.onClick.AddListener(ShowForgotPasswordPage);//From Login page
            GotoRegisterPage_Btn.onClick.AddListener(ShowRegisterPage);//From Login page

            RegisterToLogin_Btn.onClick.AddListener(ShowLoginPage);//From Register page
            RegisterToLoginNow_Btn.onClick.AddListener(ShowLoginPage);//From Register page

            VerifiToLogin_Btn.onClick.AddListener(ShowLoginPage);//From Verification page

            ForgotPasswordToBack_Btn.onClick.AddListener(ShowLoginPage);//From Password Reset page
            ForgotPasswordGoBack_Btn.onClick.AddListener(ShowLoginPage);//From Password Reset page

            NewPasswordToBack_Btn.onClick.AddListener(ShowForgotPasswordPage);//From Set New Password page

 //           ResetPasswordSuccessToBack_Btn.onClick.AddListener(); //From Reset Password Success page
//            ResetPasswordSuccessToHome_Btn.onClick.AddListener(); //From Reset Password Success page

            Okay_Btn.onClick.AddListener(Warning_Haler.Disable); //From warning page

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



            ShowLoginPage(); //set active default Login Page
        }

        //change confirm password color when user tying
        private void OnTypeRegisterPassword(string confirmPass) => isMached_RegisterPassword();
        private void OnTypeResetPassword(string confirmPass) => isMached_ResetPassword();
        private bool isMached_RegisterPassword()
        {
            bool isMatched = Register_PasswordField.text == Register_ConfirmPasswordField.text;
            Register_ConfirmPasswordField.textComponent.color = isMatched ? Color.black : Color.red;

            return isMatched;
        }

        private bool isMached_ResetPassword()
        {
            bool isMatched = ResetPassword_Field.text == ConfirmResetPassword_Field.text;
            ConfirmResetPassword_Field.textComponent.color = isMatched ? Color.black : Color.red;

            return isMatched;

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
        public void SetVerificationPage_BtnEvent(UnityAction OnClickVerify,UnityAction OnClickRecentCode)
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
        /// returnverification code from ui screen W.R.To user input
        /// </summary>
        public int GetVerificationode => System.Int32.Parse( VerificationCodeField.text);
        /// <summary>
        /// retun Login Email from ui screen
        /// </summary>
        public string GetLoginEmail => Login_EmailField.text;
        /// <summary>
        /// retun Login password from ui screen
        /// </summary>
        public string GetLoginPassword => Login_EmailField.text;

        public void ShowLoginPage()=> activity.Show(Login_Page);
        public void ShowRegisterPage()=>activity.Show(Register_Page);
        public void ShowVerificationPage()=> activity.Show(VerificationCode_Page);
        public void ShowForgotPasswordPage()=> activity.Show(ForgotPassword_Page);
        public void ShowSetNewPasswordPage()=> activity.Show(SetNewPassword_Page);
        public void ShowPasswordResetSuccessPage()=> activity.Show(PasswordResetSuccess_Page);
        public void ShowWarningPage(Warning warning) => Warning_Haler.ShowWarning(warning);
        public void ShowToast(string message, float duration, Color color) => activity.ShowToast(message, duration, color);

   
    }
}
