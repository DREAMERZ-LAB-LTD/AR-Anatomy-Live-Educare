using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.RemoteConfig;
using LoginRegisterSystem;

public class RemoteConfig : MonoBehaviour
{
    #region SingleTone

    private static RemoteConfig api;
    public static RemoteConfig API => api;
    private void SingleTone()
    {
        if (API != null)
        {
            Destroy(this.gameObject);
            return;
        }

        api = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion SingleTone


    public appAttributes URLData;


    void Awake()
    {
        SingleTone();
        // Add a listener to apply settings when successfully retrieved:
        ConfigManager.FetchCompleted += ApplyRemoteSettings;

        // Set the user’s unique ID:
        ConfigManager.SetCustomUserID("some-user-id");

        // Set the environment ID:
        //ConfigManager.SetEnvironmentID("an-env-id");

        // Fetch configuration setting from the remote service:
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        appAttributes urlData = new appAttributes();
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            
            case ConfigOrigin.Default:
                Show.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
#if UNITY_EDITOR
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
#endif
                urlData.BaseURL = ConfigManager.appConfig.GetString("BaseURL");
                urlData.SignUp = ConfigManager.appConfig.GetString("SignUp");
                urlData.EmailVerification = ConfigManager.appConfig.GetString("EmailVerification");
                urlData.ResedOTP = ConfigManager.appConfig.GetString("ResedOTP");
                urlData.Login = ConfigManager.appConfig.GetString("Login");
                urlData.PasswordResetRequest = ConfigManager.appConfig.GetString("PasswordResetRequest");
                urlData.PasswordResetRequestVerication = ConfigManager.appConfig.GetString("PasswordResetRequestVerication");
                urlData.PasswordReset = ConfigManager.appConfig.GetString("PasswordReset");
                urlData.GetUserInfo = ConfigManager.appConfig.GetString("GetUserInfo");
                urlData.GetCategoryResponse = ConfigManager.appConfig.GetString("GetCategoryResponse");
                urlData.company_id = ConfigManager.appConfig.GetString("company_id");
                urlData.GetFurnitureFromCategory = ConfigManager.appConfig.GetString("GetFurnitureFromCategory");


               break;
            case ConfigOrigin.Remote:
#if UNITY_EDITOR
                Debug.Log("New settings loaded this session; update values accordingly.");
#endif
                urlData.BaseURL = ConfigManager.appConfig.GetString("BaseURL");
                urlData.SignUp = ConfigManager.appConfig.GetString("SignUp");
                urlData.EmailVerification = ConfigManager.appConfig.GetString("EmailVerification");
                urlData.ResedOTP = ConfigManager.appConfig.GetString("ResedOTP");
                urlData.Login = ConfigManager.appConfig.GetString("Login");
                urlData.PasswordResetRequest = ConfigManager.appConfig.GetString("PasswordResetRequest");
                urlData.PasswordResetRequestVerication = ConfigManager.appConfig.GetString("PasswordResetRequestVerication");
                urlData.PasswordReset = ConfigManager.appConfig.GetString("PasswordReset");
                urlData.GetUserInfo = ConfigManager.appConfig.GetString("GetUserInfo");
                urlData.GetCategoryResponse = ConfigManager.appConfig.GetString("GetCategoryResponse");
                urlData.company_id = ConfigManager.appConfig.GetString("company_id");
                urlData.GetFurnitureFromCategory = ConfigManager.appConfig.GetString("GetFurnitureFromCategory");

 
                break;
        }


        //set updated API data if remoteconfig working fine otherwise continue with manual API that initialize from inspector
        if (urlData.BaseURL != string.Empty)
        {
            this.URLData = urlData;
        }
    }



}
