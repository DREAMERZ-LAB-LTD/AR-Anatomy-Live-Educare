using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class ResetPasswordSuccessPage : MonoBehaviour
    {
        #region Prpoerty
#pragma warning disable 649
        [Header("Button's")]
        [SerializeField] private Button Back_Btn;
        [SerializeField] private Button GoHome_Btn;
#pragma warning restore 649
        #endregion Prpoerty

        /// <summary>
        /// return this panel gameojbect
        /// </summary>
        public GameObject Activity => this.gameObject;


        /// <summary>
        /// set button event that control only screen transiction reset password success to login page
        /// </summary>
        /// <param name="OnClickGoHome"></param>
        /// <param name="OnClickack"></param>
        public void SetButtonEvents(UnityAction OnClickGoHome, UnityAction OnClickack)
        {
            GoHome_Btn.onClick.RemoveAllListeners();
            Back_Btn.onClick.RemoveAllListeners();

            GoHome_Btn.onClick.AddListener(OnClickGoHome);
            Back_Btn.onClick.AddListener(OnClickack);
        }
    }
}