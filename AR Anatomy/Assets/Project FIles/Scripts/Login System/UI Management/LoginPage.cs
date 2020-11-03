using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LoginPage : MonoBehaviour
    {

        #region Prpoerty
#pragma warning disable 649
        [Header("Input Field's")]
        [SerializeField] private TMP_InputField Login_EmailField;
        [SerializeField] private TMP_InputField Login_PasswordField;

        [Header("Button's")]
        [SerializeField] private Button Login_Btn;
 //       [SerializeField] private Button AppleLogin_Btn;
        [SerializeField] private Button FacebookLogin_Btn;
        [SerializeField] private Button ForgotPassword_Btn;
        [SerializeField] private Button GotoRegisterPage_Btn;
        [SerializeField] private Button LoginPasswordVisible_Btn;

        [Header("Passwor visile & Hide sign")]
        [SerializeField] private Sprite VisileSign;
        [SerializeField] private Sprite NonVisileSign;
#pragma warning restore 649
        #endregion Prpoerty

        private void OnEnable()
        {
            SetPasswordVisileBtnEvent();
            ClearFields();
        }

        //clear all input field
        private void ClearFields()
        {
            Login_EmailField.text = string.Empty;
            Login_PasswordField.text = string.Empty;
        }

        //set button event that control password content mode like password to text view
        private void SetPasswordVisileBtnEvent()
        {
            LoginPasswordVisible_Btn.onClick.RemoveAllListeners();
            LoginPasswordVisible_Btn.onClick.AddListener(delegate { ChangePasswordVisileMode(LoginPasswordVisible_Btn.GetComponent<Image>(), Login_PasswordField); });
        }
        private void ChangePasswordVisileMode(Image buttonTexture, TMP_InputField passwordField)
        {
            bool isPasswordMode = passwordField.contentType == TMP_InputField.ContentType.Password;
            buttonTexture.sprite = isPasswordMode ? NonVisileSign : VisileSign;
            passwordField.contentType = isPasswordMode ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            passwordField.ForceLabelUpdate();
        }


        /// <summary>
        /// return this panel gameojbect
        /// </summary>
        public GameObject Activity => this.gameObject;


        /// <summary>
        /// set default buttons event that control only screen transiction
        /// </summary>
        /// <param name="OnClickForgotPassword"></param>
        /// <param name="OnClickRegister"></param>
        public void SetButtonEvents(UnityAction OnClickForgotPassword, UnityAction OnClickRegister)
        {
            //remove event from button 
            ForgotPassword_Btn.onClick.RemoveAllListeners();
            GotoRegisterPage_Btn.onClick.RemoveAllListeners();

            //set button's event
            ForgotPassword_Btn.onClick.AddListener(OnClickForgotPassword);
            GotoRegisterPage_Btn.onClick.AddListener(OnClickRegister);
        }

        /// <summary>
        /// this buttons event control all of the backend execution process
        /// </summary>
        /// <param name="OnClickLogin"></param>
        /// <param name="OnClickAppleLogin"></param>
        /// <param name="OnClickFaceBookLogin"></param>
        public void SetButtonEvents(UnityAction OnClickLogin, UnityAction OnClickAppleLogin, UnityAction OnClickFaceBookLogin)
        {
            //remove event from button 
            Login_Btn.onClick.RemoveAllListeners();
 //           AppleLogin_Btn.onClick.RemoveAllListeners();
            FacebookLogin_Btn.onClick.RemoveAllListeners();

            //set button's event
            Login_Btn.onClick.AddListener(OnClickLogin);
 //           AppleLogin_Btn.onClick.AddListener(OnClickAppleLogin);
            FacebookLogin_Btn.onClick.AddListener(OnClickFaceBookLogin);
        }

        /// <summary>
        /// return true if user login page all of input field is not null
        /// </summary>
        public bool isValidLoginInfo => GetUserEmail != "" && GetUserPassword != "";

        /// <summary>
        /// return uesr email from login page
        /// </summary>
        public string GetUserEmail => Login_EmailField.text;
        /// <summary>
        /// return user password from login page
        /// </summary>
        public string GetUserPassword => Login_PasswordField.text;
    }
}