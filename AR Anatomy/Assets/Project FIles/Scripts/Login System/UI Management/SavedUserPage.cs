using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using LoginRegisterSystem;

namespace UI
{
    public class SavedUserPage : MonoBehaviour
    {
        #region Prpoerty
#pragma warning disable 649
        [Header("Button's")]
        [SerializeField] private Button SavedUserContinue;
        [SerializeField] private Button LoginAnother;
        [Header("Text's")]
        [SerializeField] private TMP_Text SavedUserName;
        [SerializeField] private TMP_Text SavedUserEmail;
#pragma warning restore 649
        #endregion Prpoerty

        private void OnEnable()
        {
            GetSavedUserInformation();
        }
        //show saved user information when this application starting up
        private void GetSavedUserInformation()
        {
            GetUserInfoStruct LastSavedUer = AuthManager.SavedUser;
            SavedUserName.text = LastSavedUer.data.name;
            SavedUserEmail.text = LastSavedUer.data.email;
        }

        /// <summary>
        /// return this panel gameojbect
        /// </summary>
        public GameObject Activity => this.gameObject;

        /// <summary>
        /// set button event that control only screen trasiction saved user page to login page
        /// </summary>
        /// <param name="OnClickAnotherAccount"></param>
        public void SetAnotherButtonEvents(UnityAction OnClickAnotherAccount)
        {
            LoginAnother.onClick.RemoveAllListeners(); //From saved user page
            LoginAnother.onClick.AddListener(OnClickAnotherAccount);
        }
        /// <summary>
        /// set button event that control retrive saved user data from server
        /// </summary>
        /// <param name="OnClickContinue"></param>
        public void SetButtonEvents(UnityAction OnClickContinue)
        {
            SavedUserContinue.onClick.RemoveAllListeners(); //From saved user page
            SavedUserContinue.onClick.AddListener(OnClickContinue);
        }
    }

}