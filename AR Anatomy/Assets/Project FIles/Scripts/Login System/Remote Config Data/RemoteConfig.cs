using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.RemoteConfig;
using LoginRegisterSystem;

public class RemoteConfig : MonoBehaviour
{
  
    public static appAttributes URLData;


    void Awake()
    {
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
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            
            case ConfigOrigin.Default:
                Show.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Show.Log("No settings loaded this session; using cached values from a previous session.");
                Show.Log(ConfigManager.appConfig.GetString("SignUp"));

                URLData.BaseURL = ConfigManager.appConfig.GetString("BaseURL");
                URLData.SignUp = ConfigManager.appConfig.GetString("SignUp");
                URLData.EmailVerification = ConfigManager.appConfig.GetString("EmailVerification");
                URLData.ResedOTP = ConfigManager.appConfig.GetString("ResedOTP");
                URLData.Login = ConfigManager.appConfig.GetString("Login");
                URLData.PasswordResetRequest = ConfigManager.appConfig.GetString("PasswordResetRequest");
                URLData.PasswordResetRequestVerication = ConfigManager.appConfig.GetString("PasswordResetRequestVerication");
                URLData.PasswordReset = ConfigManager.appConfig.GetString("PasswordReset");
                URLData.GetUserInfo = ConfigManager.appConfig.GetString("GetUserInfo");
                URLData.GetCategoryResponse = ConfigManager.appConfig.GetString("GetCategoryResponse");
                URLData.company_id = ConfigManager.appConfig.GetString("company_id");
                URLData.GetFurnitureFromCategory = ConfigManager.appConfig.GetString("GetFurnitureFromCategory");
                URLData.SocialLogin = ConfigManager.appConfig.GetString("SocialLogin");


               break;
            case ConfigOrigin.Remote:
                Show.Log("New settings loaded this session; update values accordingly.");
                Show.Log(ConfigManager.appConfig.GetString("Register"));
                
                URLData.BaseURL = ConfigManager.appConfig.GetString("BaseURL");
                URLData.SignUp = ConfigManager.appConfig.GetString("SignUp");
                URLData.EmailVerification = ConfigManager.appConfig.GetString("EmailVerification");
                URLData.ResedOTP = ConfigManager.appConfig.GetString("ResedOTP");
                URLData.Login = ConfigManager.appConfig.GetString("Login");
                URLData.PasswordResetRequest = ConfigManager.appConfig.GetString("PasswordResetRequest");
                URLData.PasswordResetRequestVerication = ConfigManager.appConfig.GetString("PasswordResetRequestVerication");
                URLData.PasswordReset = ConfigManager.appConfig.GetString("PasswordReset");
                URLData.GetUserInfo = ConfigManager.appConfig.GetString("GetUserInfo");
                URLData.GetCategoryResponse = ConfigManager.appConfig.GetString("GetCategoryResponse");
                URLData.company_id = ConfigManager.appConfig.GetString("company_id");
                URLData.GetFurnitureFromCategory = ConfigManager.appConfig.GetString("GetFurnitureFromCategory");
                URLData.SocialLogin = ConfigManager.appConfig.GetString("SocialLogin");

 
                break;
        }

    }



}
