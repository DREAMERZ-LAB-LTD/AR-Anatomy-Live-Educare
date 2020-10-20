using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using LoginRegisterSystem;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace UI
{
    public class UI_Handeler : MonoBehaviour
    {
        #region Property
#pragma warning disable 649

        public static bool isBackFromARScene = false;

        private Activity activity;//handel all of ui transiction system


        [Header("Rerence All Of The Panel's")]
        public LoginPage LoginPanel;
        public RegisterPage RegisterPanel;
        public VerificationPage VerificationPage;
        public ForgotPasswordPage ForgotPasswordPanel;
        public SetNewPasswordPage NewPasswordPanel;
        public ResetPasswordSuccessPage ResetPasswordSuccessPanel;
        public SavedUserPage SaveduserPanel;
        [SerializeField] private GameObject GamePanel;

        [Header("Reference all of the Sub panel's")]
        public WarningHaler Warning_Haler;//handel warning panel
        [SerializeField] private GameObject TostMessage;
        [SerializeField] private GameObject LoadingPanel;

        [Header("Game Menu Panel's Property ")]
        [SerializeField] private Button LogOutBtn;

        
#pragma warning restore 649
        #endregion Property


        #region Initilize Zone

        private void Awake()
        {
            ShowLoadingPage(false);
            InitilizePanel();
            InitilizeDefaultBtnEvent();
        }

        private void InitilizePanel()
        {
            //it will controll all of ui transitions
            activity = new Activity(TostMessage);

            //initialy deactive all of the panel
            LoginPanel.Activity.SetActive(false);
            RegisterPanel.Activity.SetActive(false);
            VerificationPage.Activity.SetActive(false);
            ForgotPasswordPanel.Activity.SetActive(false);
            NewPasswordPanel.Activity.SetActive(false);
            ResetPasswordSuccessPanel.Activity.SetActive(false);
            SaveduserPanel.Activity.SetActive(false);
            TostMessage.SetActive(false);
            GamePanel.SetActive(false);


            if (!AuthManager.isLoggedin)
            {
                ShowLoginPage();
                return;
            }

            if (isBackFromARScene)
            {
                //when user back from AR scene, we need to show game menu panel in run time because an user allready login
                ShowGamePage();
            }
            else
            {
                //when app is opening we need to show saved uesr information on saved user panel first time only
                activity.Show(SaveduserPanel.Activity);
            }

        }

        /// <summary>
        /// initilize all of the default button event as default
        ///default ui only control ui page trasition No other Logic execute with this buttons
        /// </summary>
        private void InitilizeDefaultBtnEvent()
        {
            //remove button's event
            LoginPanel.SetButtonEvents(ShowForgotPasswordPage, ShowRegisterPage);
            RegisterPanel.SetButtonEvents(ShowLoginPage);
            VerificationPage.SetButtonEvents(ShowLoginPage);
            ForgotPasswordPanel.SetButtonEvents(ShowLoginPage, ShowLoginPage);
            ResetPasswordSuccessPanel.SetButtonEvents(ShowLoginPage, ShowLoginPage);
            SaveduserPanel.SetAnotherButtonEvents(ShowLoginPage);
            NewPasswordPanel.SetBackuBtnEvent(ShowForgotPasswordPage);//From Set New Password page
        }


        /// <summary>
        /// Set Logout button event where from control Logout systm
        /// </summary>
        /// <param name="OnClickLogOut">Logout action method</param>
        public void SetLogOutBtnEvent(UnityAction OnClickLogOut)
        {
            LogOutBtn.onClick.RemoveAllListeners();
            LogOutBtn.onClick.AddListener(OnClickLogOut);
        }

        #endregion Initilize Zone


        #region Activity Panel Transiction
        public void ShowLoginPage()=>activity.Show(LoginPanel.Activity);
        
        public void ShowRegisterPage()=>activity.Show(RegisterPanel.Activity);
        
        public void ShowVerificationPage()=>activity.Show(VerificationPage.Activity);
        
        public void ShowForgotPasswordPage()=>activity.Show(ForgotPasswordPanel.Activity);
        
        public void ShowSetNewPasswordPage()=> activity.Show(NewPasswordPanel.Activity);
        
        public void ShowPasswordResetSuccessPage()=>activity.Show(ResetPasswordSuccessPanel.Activity);
        
        public void ShowGamePage()=> activity.Show(GamePanel);
        
        public void ShowLoadingPage(bool show) => LoadingPanel.SetActive(show); //Popup Loadin Page On Ui Screen

#pragma warning disable 4014
        public void ShowToast(string message, float duration, Color color)
        { 
            //activity.ShowToast(message, duration, color);
        }
#pragma warning restore 4014
#endregion Activity Panel Transiction

    }
}
