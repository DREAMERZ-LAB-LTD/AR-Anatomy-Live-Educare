using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{

    public class RegisterPage : MonoBehaviour
    {
        #region Property
#pragma warning disable 649
        [Header("Input Field's")]
        //input fields
        [SerializeField] private TMP_InputField NameField;
        [SerializeField] private TMP_InputField EmailField;
        [SerializeField] private TMP_InputField PhoneNoField;
        [SerializeField] private TMP_InputField PasswordField;
        [SerializeField] private TMP_InputField ConfirmPasswordField;

        [Header("Buttons's")]
        [SerializeField] private Button Register_Btn;
        [SerializeField] private Button AppleRegister_Btn;
        [SerializeField] private Button FacebookRegister_Btn;
        [SerializeField] private Button RegisterToLogin_Btn;
        [SerializeField] private Button RegisterToLoginNow_Btn;
        [SerializeField] private Button PasswordVisible_Btn;
        [SerializeField] private Button ConfirmPasswordVisible_Btn;
        

        [Header("Required information warnings Popup")]
        [SerializeField] private GameObject RequiredNameMessage;
        [SerializeField] private GameObject RequiredEmailMessage;
        [SerializeField] private GameObject RequiredPhoneMessage;
        [SerializeField] private GameObject RequiredPasswordMessage;
        [SerializeField] private GameObject RequiredConfirmPasswordMessage;

        [Header("Passwor visile & Hide Button Sprite")]
        [SerializeField] private Sprite VisileSign;
        [SerializeField] private Sprite NonVisileSign;


        [Header("Country Code Property's")]
        [SerializeField] private GameObject CountryInfoCard;
        [SerializeField] private Transform CountryInfoContainer;
        [SerializeField] private GameObject CountryCodePopup;
        [SerializeField] private Button CountryCodeChangeBtn;
        [SerializeField] private Button CloseBtn;

        private ISO3166Country CurrentCountry;//currcent country information where user stay now

#pragma warning restore 649
        #endregion Property

        private void Start()
        {
            CurrentCountry = CountryCode.GeneratorFromAlpha2("GB");
            CountryCodeChangeBtn.GetComponentInChildren<TMP_Text>().text = CurrentCountry.DialCodes[0];
            Countrypopup(false);
            CloseBtn.onClick.RemoveAllListeners();
            CloseBtn.onClick.AddListener(delegate { Countrypopup(false); });
            CountryCodeChangeBtn.onClick.RemoveAllListeners();
            CountryCodeChangeBtn.onClick.AddListener(delegate { Countrypopup(true); });
            LoadCountryList();
        }

        private void OnEnable()
        {
            SetInputFiedEvent();
            SetPasswordVisibleBtnEvent();
            HideRequiredInfo();
            HideRequiredPassword();
            ClearFields();
            OnAutoDetectCountry();
            ClearFields();
        }


        //check confirm password when on typing
        private void SetInputFiedEvent()
        {
            PasswordField.onValueChanged.RemoveAllListeners();
            ConfirmPasswordField.onValueChanged.RemoveAllListeners();
            PasswordField.onValueChanged.AddListener(OnTypePassword);
            ConfirmPasswordField.onValueChanged.AddListener(OnTypePassword);

        }

        //set country code popup sub panel active or deactive
        private void Countrypopup(bool show) => CountryCodePopup.SetActive(show);

        //load all of the country's under scrol view 
        private void LoadCountryList()
        {
            IEnumerable <ISO3166Country> Contrys = CountryCode.GetALLCountry();
            Contrys.OrderBy(o => o.limit);

            foreach (ISO3166Country country in Contrys)
            {
                foreach (string dailcode in country.DialCodes)
                { 
                    string country_info = country.Name + "("+dailcode+")";
                    string[] subinfo = country_info.Split('('); //data structure will be change in this array  like this->  [0] = country name [1] = +880)
                    string[] countryCode = subinfo[1].Split(')');//data structure will be change in this array  like this-> [0]= +880
               

                    GameObject infocard = Instantiate(CountryInfoCard);
                    infocard.GetComponentInChildren<TMP_Text>().text = country_info;
                    infocard.GetComponent<Button>().onClick.RemoveAllListeners();
                    infocard.GetComponent<Button>().onClick.AddListener(delegate { OnChangeCountry(countryCode[0]);});//set new country when user try to change manually
                    infocard.GetComponent<Button>().onClick.AddListener(delegate { Countrypopup(false); });//afte change the we need to deactive popup panel

                    infocard.transform.SetParent(CountryInfoContainer);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(CountryInfoContainer.GetComponent<RectTransform>());
                    infocard.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
                }
            }

        }

        //when auto select country
        private void OnAutoDetectCountry()
        {
            StartCoroutine(CountryCode.GetCurentountryJson(Onsuccess, OnError));

            void Onsuccess(string countryJson)
            {
                CurrentCountry = CountryCode.JsonToObject(countryJson);
                CountryCodeChangeBtn.GetComponentInChildren<TMP_Text>().text = CurrentCountry.DialCodes[0];
            }
            void OnError(string message)
            {
                CurrentCountry = CountryCode.GeneratorFromAlpha2("GB");
                CountryCodeChangeBtn.GetComponentInChildren<TMP_Text>().text = CurrentCountry.DialCodes[0];
            }
        }

        //change contry manually when user change drop down value
        private void OnChangeCountry(string dailcode)
        {
            CurrentCountry = CountryCode.GeneratorFromPhoneNumber(dailcode);
            CountryCodeChangeBtn.GetComponentInChildren<TMP_Text>().text = dailcode;
            //Debug.Log("Name : "+CurrentCountry.Name);
           // Debug.Log("Code : "+dailcode);
           // Debug.Log("Limite : " +CurrentCountry.limit);
        }


        private void ClearFields()
        {
            NameField.text = string.Empty;
            EmailField.text = string.Empty;
            PhoneNoField.text = string.Empty;
            PasswordField.text = string.Empty;
            ConfirmPasswordField.text = string.Empty;
        }
    
        private void OnTypePassword(string confirmPass) => isMached_Password();
        //set button event that control password content mode like password to text view
        private void SetPasswordVisibleBtnEvent()
        {
            PasswordVisible_Btn.onClick.RemoveAllListeners();
            ConfirmPasswordVisible_Btn.onClick.RemoveAllListeners();
            PasswordVisible_Btn.onClick.AddListener(delegate {ChangePasswordFieldMode(PasswordVisible_Btn.GetComponent<Image>(),PasswordField);});
            ConfirmPasswordVisible_Btn.onClick.AddListener(delegate {ChangePasswordFieldMode(ConfirmPasswordVisible_Btn.GetComponent<Image>(),  ConfirmPasswordField);});
        }
        
        //change password to text mode and text to password mode
        private void ChangePasswordFieldMode(Image buttonTexture, TMP_InputField passwordField)
        {
            bool isPasswordMode = passwordField.contentType == TMP_InputField.ContentType.Password;
            buttonTexture.sprite = isPasswordMode ? NonVisileSign : VisileSign;
            passwordField.contentType = isPasswordMode ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            passwordField.ForceLabelUpdate();
        }

        /// <summary>
        /// return true if password and confirm password are mached in user register page
        /// </summary>
        /// <returns></returns>
        public bool isMached_Password()
        {
            bool isMatched = PasswordField.text != "" && PasswordField.text == ConfirmPasswordField.text;
            ConfirmPasswordField.textComponent.color = isMatched ? Color.black : Color.red;
            return isMatched;
        }

        //if user ignore required fields then popup required fields that user ignored
        private void ShowRegisterRequiredInfo()
        {
            bool isvalidPhone = PhoneNoField.text.Length == CurrentCountry.limit || (CurrentCountry.limit < 0 && PhoneNoField.text != "");
            RequiredNameMessage.SetActive(NameField.text == "");
            RequiredEmailMessage.SetActive(EmailField.text == "");
            RequiredPhoneMessage.SetActive(!isvalidPhone);

            RequiredPasswordMessage.SetActive(PasswordField.text.Length < 8);
            RequiredConfirmPasswordMessage.SetActive(ConfirmPasswordField.text.Length < 8);
        }

        //deactive all of the register null message
        private void HideRequiredInfo()
        {
            RequiredNameMessage.SetActive(false);
            RequiredEmailMessage.SetActive(false);
            RequiredPhoneMessage.SetActive(false);
        }
        private void HideRequiredPassword()
        {
            RequiredPasswordMessage.SetActive(false);
            RequiredConfirmPasswordMessage.SetActive(false);
        }
        /// <summary>
        /// return true if user fill up all of the information field
        /// </summary>
        public bool isValidinInfo
        {
            get
            {
                bool isvalidPhone = PhoneNoField.text.Length == CurrentCountry.limit || (CurrentCountry.limit < 0 && PhoneNoField.text != "");
                bool hasInfo = GetUerName != "" && GetUserEmail != "" && isvalidPhone;
                if (!hasInfo)
                {
                    ShowRegisterRequiredInfo();
                }
                else
                {
                    HideRequiredInfo();
                }
                return hasInfo;

            }
        }

        /// <summary>
        /// return true if user fill up all of the password field with valid digits
        /// </summary>
        public bool isValidPassword
        {
            get
            {
                bool isValidPass = PasswordField.text.Length >= 8 && ConfirmPasswordField.text.Length >= 8;
                if (!isValidPass)
                {
                    ShowRegisterRequiredInfo();//popup un valid field
                }
                else
                {
                    HideRequiredPassword();
                }
                return isValidPass;

            }
        }


        /// <summary>
        /// set default button event that control only screen transiction
        /// </summary>
        /// <param name="OnClickLoginPage"></param>
        public void SetButtonEvents(UnityAction OnClickLoginPage)
        {
            RegisterToLogin_Btn.onClick.RemoveAllListeners();
            RegisterToLoginNow_Btn.onClick.RemoveAllListeners();

            RegisterToLogin_Btn.onClick.AddListener(OnClickLoginPage);
            RegisterToLoginNow_Btn.onClick.AddListener(OnClickLoginPage);
        }

        /// <summary>
        /// this button events control all of the backend execution process
        /// </summary>
        /// <param name="OnClickRegister"></param>
        /// <param name="OnClickAppleRegister"></param>
        /// <param name="OnClickFacebookRegister"></param>
        public void SetButtonEvents(UnityAction OnClickRegister, UnityAction OnClickAppleRegister, UnityAction OnClickFacebookRegister)
        {
            Register_Btn.onClick.RemoveAllListeners();
            AppleRegister_Btn.onClick.RemoveAllListeners();
            FacebookRegister_Btn.onClick.RemoveAllListeners();

            Register_Btn.onClick.AddListener(OnClickRegister);
            AppleRegister_Btn.onClick.AddListener(OnClickAppleRegister);
            FacebookRegister_Btn.onClick.AddListener(OnClickFacebookRegister);
        }

        /// <summary>
        /// return this panel gameojbect
        /// </summary>
        public GameObject Activity => this.gameObject;
        /// <summary>
        /// return user name from register page name field
        /// </summary>
        public string GetUerName => NameField.text;
        /// <summary>
        /// return email from register page email field
        /// </summary>
        public string GetUserEmail => EmailField.text;
        /// <summary>
        ///  return phone numer from register page phone numer field
        /// </summary>
        public string GetUserPhoneNumber => CountryCodeChangeBtn.GetComponentInChildren<TMP_Text>().text + PhoneNoField.text;
        /// <summary>
        ///  return password from register page password field
        /// </summary>
        public string GetUserPassword => PasswordField.text;
    }
}