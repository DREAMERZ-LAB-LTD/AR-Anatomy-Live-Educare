using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetNewPasswordPage : MonoBehaviour
{

    #region Prpoerty
#pragma warning disable 649
    [Header("Input Field's")]
    [SerializeField] private TMP_InputField Password_Field;
    [SerializeField] private TMP_InputField ConfirmPassword_Field;
    [Header("Button's")]
    [SerializeField] private Button Back_Btn;
    [SerializeField] private Button ResetPassword_Btn;
    [SerializeField] private Button PasswordVisible_Btn;
    [SerializeField] private Button ConfirmPasswordVisible_Btn;
    [Header("Passwor visile & Hide Sprite sign")]
    [SerializeField] private Sprite VisileSign;
    [SerializeField] private Sprite NonVisileSign;

#pragma warning restore 649
    #endregion Prpoerty

    private void OnEnable()
    {
        ClearFields();
        SetInputFiedEvent();
        SetPasswordVisibleBtnEvent();
    }

    //clear all input field
    private void ClearFields()
    {
        Password_Field.text = string.Empty;
        ConfirmPassword_Field.text = string.Empty;
    }
    //Check  confirm password and change color in runtime when user tying on input field in Reset Pawwword Pagee
    private void OnTypeResetPassword(string confirmPass) => isMached_Password();
    //check confirm password when on typing
    private void SetInputFiedEvent()
    {
        Password_Field.onValueChanged.RemoveAllListeners();
        ConfirmPassword_Field.onValueChanged.RemoveAllListeners();
        Password_Field.onValueChanged.AddListener(OnTypeResetPassword);
        ConfirmPassword_Field.onValueChanged.AddListener(OnTypeResetPassword);
    }

    //set button event that control password visible mode 
    private void SetPasswordVisibleBtnEvent()
    {
        PasswordVisible_Btn.onClick.RemoveAllListeners();
        ConfirmPasswordVisible_Btn.onClick.RemoveAllListeners();

        PasswordVisible_Btn.onClick.AddListener(delegate{ChangePasswordFieldMode(PasswordVisible_Btn.GetComponent<Image>(),Password_Field);});
        ConfirmPasswordVisible_Btn.onClick.AddListener(delegate{ ChangePasswordFieldMode( ConfirmPasswordVisible_Btn.GetComponent<Image>(), ConfirmPassword_Field);});
    }


    //handle password visile button icon and input filed content type.(Like password mode or standerd content)
    private void ChangePasswordFieldMode(Image buttonTexture, TMP_InputField passwordField)
    {
        bool isPasswordMode = passwordField.contentType == TMP_InputField.ContentType.Password;
        buttonTexture.sprite = isPasswordMode ? NonVisileSign : VisileSign;
        passwordField.contentType = isPasswordMode ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        passwordField.ForceLabelUpdate();
    }

    /// <summary>
    /// return true if password and confirm password are mached in Reset password page
    /// return true if password and confirm password are mached in Reset password page
    /// </summary>
    /// <returns></returns>
    public bool isMached_Password()
    {
        bool isMatched = Password_Field.text != "" && Password_Field.text == ConfirmPassword_Field.text;
        ConfirmPassword_Field.textComponent.color = isMatched ? Color.black : Color.red;
        return isMatched;
    }

    public bool isValidPassword => Password_Field.text.Length >= 8 && ConfirmPassword_Field.text.Length >= 8;

    /// <summary>
    /// set default buttons event that control only screen transiction
    /// </summary>
    /// <param name="OnClickack"></param>
    public void SetBackuBtnEvent(UnityAction OnClickBack)
    {
        Back_Btn.onClick.RemoveAllListeners();
        Back_Btn.onClick.AddListener(OnClickBack);
    }

    /// <summary>
    /// this buttons event control all of the backend execution process
    /// </summary>
    /// <param name="OnClickResetPassword"></param>
    public void SetResetBtnEvent(UnityAction OnClickResetPassword)
    {
        ResetPassword_Btn.onClick.RemoveAllListeners();
        ResetPassword_Btn.onClick.AddListener(OnClickResetPassword);
    }


    /// <summary>
    /// return this panel gameojbect
    /// </summary>
    public GameObject Activity => this.gameObject;

    /// <summary>
    /// return user new password
    /// </summary>
    public string GetNewPasword => Password_Field.text;

}
