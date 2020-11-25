using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace UI
{
    public class VerificationPage : MonoBehaviour
    {

        #region Prpoerty
#pragma warning disable 649
        [Header("input Field's")]
        [SerializeField] private TMP_InputField VerificationCodeField;
        [Header("Button's")]
        [SerializeField] private Button VerifiToLogin_Btn;
        [SerializeField] private Button Verify_Btn;
        [SerializeField] private Button RecntVerificationCode_Btn;
        [Header("Sent Email Shower Text")]
        [SerializeField] private TMP_Text SendingEmail;
#pragma warning restore 649
        #endregion Prpoerty

        private void OnEnable()
        {
            ClearFields();
        }
        //clear input field
        private void ClearFields()
        {
            VerificationCodeField.text = string.Empty;
        }


        /// <summary>
        /// return this panel gameojbect
        /// </summary>
        public GameObject Activity => this.gameObject;
        /// <summary>
        /// return verification code from verification page
        /// </summary>
        public string GetVerificationCode => VerificationCodeField.text;


        /// <summary>
        ///  set default button event that control only screen transiction
        /// </summary>
        /// <param name="OnClickBackToLogin"></param>
        public void SetButtonEvents(UnityAction OnClickBackToLogin)
        {
            VerifiToLogin_Btn.onClick.RemoveAllListeners();
            VerifiToLogin_Btn.onClick.AddListener(OnClickBackToLogin);
        }

        /// <summary>
        /// this buttons event control all of the backend execution process
        /// </summary>
        /// <param name="OnClickResetPasswordEmailVerify">Reset verificatoin code</param>
        /// <param name="OnClickRecentCode">Reset password recent verification code</param>
        public void SetBtnEvents(UnityAction OnClickVerify, UnityAction OnClickRecentCode)
        {
            Verify_Btn.onClick.RemoveAllListeners();
            RecntVerificationCode_Btn.onClick.RemoveAllListeners();

            Verify_Btn.onClick.AddListener(OnClickVerify);
            RecntVerificationCode_Btn.onClick.AddListener(OnClickRecentCode);
        }

        /// <summary>
        /// show email on ui screen which email use for verification
        /// </summary>
        public string SetVerificationEmail
        {
            set
            {
                string email = value != null && value != "" ? value : "Example@Email.com";
                SendingEmail.text = "An email with verifiation code has been sent to " + email + ". Please enter verification code below to proceed";

            }
        }
    }
}