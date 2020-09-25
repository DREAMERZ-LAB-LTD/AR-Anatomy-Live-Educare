using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ForgotPasswordPage : MonoBehaviour
{
    #region Prpoerty
#pragma warning disable 649
    [Header("Input Email Field")]
    //input fileds
    [SerializeField] private TMP_InputField Email_Field;
    [Header("Buttons")]
    //buttons
    [SerializeField] private Button SentRequest_Btn;
    [SerializeField] private Button Back_Btn;
    [SerializeField] private Button GoBack_Btn;
#pragma warning restore 649
    #endregion Prpoerty

    private void OnEnable()
    {
        ClearFields();
    }
    //chear input field
    private void ClearFields()
    {
        Email_Field.text = string.Empty;
    }

    /// <summary>
    /// return this panel gameojbect
    /// </summary>
    public GameObject Activity => this.gameObject;

    /// <summary>
    /// return email from forgot password page
    /// </summary>
    public string GetUserEmail => Email_Field.text;

    /// <summary>
    /// set default buttons event that control only screen transiction
    /// </summary>
    /// <param name="OnClickBack"></param>
    /// <param name="OnClicGoack"></param>
    public void SetButtonEvents(UnityAction OnClickBack, UnityAction OnClicGoack)
    {
        Back_Btn.onClick.RemoveAllListeners();
        GoBack_Btn.onClick.RemoveAllListeners();
        Back_Btn.onClick.AddListener(OnClickBack);
        GoBack_Btn.onClick.AddListener(OnClicGoack);
    }

    /// <summary>
    /// this buttons event control all of the backend execution process
    /// </summary>
    /// <param name="OnClickSendRequest"></param>
    public void SetButtonEvents(UnityAction OnClickSendRequest)
    {
        SentRequest_Btn.onClick.RemoveAllListeners();
        SentRequest_Btn.onClick.AddListener(OnClickSendRequest);
    }

}
