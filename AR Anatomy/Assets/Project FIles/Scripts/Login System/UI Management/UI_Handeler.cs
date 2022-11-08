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


        [Header("Rerence All Of The Panel's")]
        public LoginPage LoginPanel;
        public RegisterPage RegisterPanel;
        public VerificationPage VerificationPage;
        public ForgotPasswordPage ForgotPasswordPanel;
        public SetNewPasswordPage NewPasswordPanel;
        public ResetPasswordSuccessPage ResetPasswordSuccessPanel;
        public SavedUserPage SaveduserPanel;
        [SerializeField] private GameObject GamePanel;
        //this object handle the animation of ui elements
        [SerializeField] private DL.UI.UIManager ui_manager;

        [Header("Reference all of the Sub panel's")]
        public WarningHaler Warning_Haler;//handel warning panel
        [SerializeField] private GameObject TostMessage;
        [SerializeField] private GameObject LoadingPanel;

  
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
           

            if (!AuthManager.isLoggedin)
            {
                ShowLoginPage();
                return;
            }

            if (isBackFromARScene)
            {
                //when user back from AR scene, we need to show game menu panel in run time because an user allready login
                OpenMenuScene();
            }
            else
            {
                //when app is opening we need to show saved uesr information on saved user panel first time only
                // activity.Show(SaveduserPanel.Activity);
                ShowSavedUser();
            }

        }


        /// <summary>
        /// initilize all of the default button event as default
        ///default ui only control ui page trasition No other Logic to execute with this buttons
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
            NewPasswordPanel.SetBackuBtnEvent(ShowForgotPasswordPage);//From Set New Password 
            
        }



        #endregion Initilize Zone


        #region Activity Panel Transiction
        public void ShowLoginPage()
        { 
            ui_manager.SwitchMenuScene(0);
        }

        public void ShowRegisterPage()
        {
            ui_manager.SwitchMenuScene(1);
        }
        public void ShowVerificationPage()
        {
            ui_manager.SwitchMenuScene(2);
        
        }
        public void ShowForgotPasswordPage()
        {
            ui_manager.SwitchMenuScene(3);
        }
        public void ShowSetNewPasswordPage()
        {
            ui_manager.SwitchMenuScene(4);
        }
        public void ShowPasswordResetSuccessPage()
        {
            ui_manager.SwitchMenuScene(5);
        }
        public void ShowSavedUser()
        {
            ui_manager.SwitchMenuScene(6);
        }

        public void OpenMenuScene()
        {
            SceneManager.LoadScene(2);
        }
        
        public void ShowLoadingPage(bool show) => LoadingPanel.SetActive(show); //Popup Loadin Page On Ui Screen

#pragma warning disable 4014
        public void ShowToast(string message, float duration, Color color)
        {
          //  activity.ShowToast(message, duration, color);
        }
#pragma warning restore 4014
#endregion Activity Panel Transiction

    }
}
