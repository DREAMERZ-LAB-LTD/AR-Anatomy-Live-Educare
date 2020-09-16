using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace LoginRegisterSystem
{
    public class UI_Handeller : MonoBehaviour
    {
        #region Property

#pragma warning disable 649
        [Header("Reference all of the panel's")]
        [SerializeField] private GameObject Login_Page;
        [SerializeField] private GameObject Register_Page;
        [SerializeField] private GameObject VerificationCode_Page;
        [SerializeField] private GameObject TostMessage;

        [Header("Reference all of the button's")]
        [SerializeField] private Button Login_Btn;
        [SerializeField] private Button Register_Btn;
        [SerializeField] private Button RegisterPage_Btn;
        [SerializeField] private Button Verify_Btn;
        [SerializeField] private Button BackFromRegister;
        [SerializeField] private Button BackFromVerification;

        [Space,Header("Reference all of the inputfields's")]
        [SerializeField] private InputField Login_EmailField;//use for Login_Page
        [SerializeField] private InputField Login_PasswordField;//use for Login_Page
        [Space]
        [SerializeField] private InputField Register_EmailField;//use for Register_Page
        [SerializeField] private InputField Register_NameField;//use for Register_Page
        [SerializeField] private InputField Register_PasswordField;//use for Register_Page
        [SerializeField] private InputField Register_ConfirmPasswordField;//use for Register_Page
        [Space]
        [SerializeField] private InputField VerificationCodeField;//use for Verification page
#pragma warning restore 649
        private Activity activity;//handel ui transiction

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

            //initialy deactive all of pages
            Login_Page.SetActive(false);
            Register_Page.SetActive(false);
            VerificationCode_Page.SetActive(false);
            TostMessage.SetActive(false);
            
            if (AuthManager.isLogedin)
            {
                SceneManager.LoadScene(0);
                return;
            }

            //remove button's event
            RegisterPage_Btn.onClick.RemoveAllListeners();
            BackFromRegister.onClick.RemoveAllListeners();
            BackFromVerification.onClick.RemoveAllListeners();

            //set event into the buttons
            RegisterPage_Btn.onClick.AddListener(ShowRegisterPage); 
            BackFromRegister.onClick.AddListener(ShowLoginPage);
            BackFromVerification.onClick.AddListener(ShowRegisterPage);


            //set passwod typeing event
            Register_PasswordField.onValueChange.RemoveAllListeners();
            Register_ConfirmPasswordField.onValueChange.RemoveAllListeners();
            Register_PasswordField.onValueChange.AddListener(OnTypePassword);
            Register_ConfirmPasswordField.onValueChange.AddListener(OnTypePassword);



            ShowLoginPage(); //set active default Login Page
        }

        //change confirm password color when user tying
        private void OnTypePassword(string confirmPass)
        {
            bool PasswordisMatched = Register_PasswordField.text == Register_ConfirmPasswordField.text;

            Register_ConfirmPasswordField.textComponent.color = PasswordisMatched ? Color.black : Color.red;
        } 

        /// <summary>
        /// set button event where from control login and register systm
        /// </summary>
        /// <param name="OnClickLogin"></param>
        /// <param name="OnClickRegister"></param>
        /// <param name="OnClickVerify"></param>
        public void SetupButtonEvent(UnityAction OnClickLogin, UnityAction OnClickRegister, UnityAction OnClickVerify)
        {
            //remove event from button 
            Login_Btn.onClick.RemoveAllListeners();
            Register_Btn.onClick.RemoveAllListeners();
            Verify_Btn.onClick.RemoveAllListeners();

            //set button's event
            Login_Btn.onClick.AddListener(OnClickLogin);
            Register_Btn.onClick.AddListener(OnClickRegister);
            Verify_Btn.onClick.AddListener(OnClickVerify);
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

        public void ShowToast(string message, float duration, Color color) => activity.ShowToast(message, duration, color);
        public void ShowLoginPage()=> activity.Show(Login_Page);
        public void ShowRegisterPage()=>activity.Show(Register_Page);
        public void ShowVerificationPage()=> activity.Show(VerificationCode_Page);

   
    }
}
