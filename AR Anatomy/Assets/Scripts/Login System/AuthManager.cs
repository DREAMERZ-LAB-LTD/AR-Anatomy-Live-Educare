using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LoginRegisterSystem
{
    public class AuthManager : MonoBehaviour
    {
        [Header("Sever Root URL")]
        [SerializeField] protected string url;
        [SerializeField] private UI_Handeller ui;

        private static string TokenKey = "Token";//user token will be save into cash using this Token key

        /// <summary>
        /// return true if an user allready login
        /// </summary>
        public static bool isLogedin //=> Token != "";
        {
            get
            {
                
                Debug.Log("Loin = "  + Token != null);
                Debug.Log("Token = " + Token);
                return Token != "";
            }
        }

        /// <summary>
        /// User Token
        /// you can change user token and save automaticly into cash;
        /// </summary>
        public static string Token
        {
            get
            {
                return PlayerPrefs.GetString(TokenKey);
            }
            private set
            {
                if (value != "")
                    PlayerPrefs.SetString(TokenKey, value);
            }
        }



        private void Start()
        {
            ui.SetupButtonEvent(OnClickLogin, OnClickRegister, OnClickVerification);
        }

        protected void OnClickLogin()
        {
            LogInStruct userlogIn;
            userlogIn.email = ui.GetLoginEmail;
            userlogIn.password = ui.GetLoginPassword;
            string json = JsonUtility.ToJson(userlogIn);
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnSuccessLogin, OnError));
        }
        protected void OnClickLogOut()
        {
            PlayerPrefs.DeleteKey(TokenKey);
           
        }


        protected void OnClickRegister()
        {
            string json = "{}";
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnRegisterSuccess, OnError));
     
        }
        protected void OnClickVerification()
        {
            
            VeryficationCode veryficationCode;
            veryficationCode.email = "successRegistation.data.user.email";//pass registe user
            veryficationCode.code = ui.GetVerificationode;
            string json = JsonUtility.ToJson(veryficationCode);
            Show.Log(json);
            StartCoroutine(RestApiHandeler.PostData(url, null, json, OnVerificationSuccess, OnError));
       
        }

        public void GetUserInfo(string token)
        {
            //Saving The Token
            Token = token;
            Show.Log("Player Prefs " + PlayerPrefs.GetString("token", ""));
            StartCoroutine(RestApiHandeler.PostData(url, token, null, OnSuccessRetriveUserdata, OnError));
        }


        #region Call Back
        private void OnSuccessLogin(string json)
        {
            SuccessLogIn successLogIn = JsonUtility.FromJson<SuccessLogIn>(json);
            GetUserInfo(successLogIn.data.token);
            ui.ShowToast("Login Success", 2f, Color.green);
        }
        private void OnRegisterSuccess(string json)
        {
            ui.ShowToast("Register Success ", 2f, Color.green);
        }
        private void OnVerificationSuccess(string json)
        {
            ui.ShowToast("Verification Success ", 2f, Color.green);
        }
        private void OnSuccessRetriveUserdata(string json)
        {
            GetUserInfoStruct getUserInfoStruct = JsonUtility.FromJson<GetUserInfoStruct>(json);
            ui.ShowToast("User Retrive Success ", 2f, Color.green);
        }
        private void OnError(string message)
        {
            ui.ShowToast("Error " + message, 2f, Color.green);
        }
        #endregion Call Back



    }
}