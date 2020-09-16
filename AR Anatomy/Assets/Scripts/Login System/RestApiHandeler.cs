using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public static class RestApiHandeler
{
    #region Callback delegats
    public delegate void OnSuccessCallBack(string data);
    public delegate void OnErrorCallBack(string error);
    public delegate void OnBoolCallBack(bool bol);
    #endregion Callback delegats

    #region Post Request
    /// <summary>
    /// PostData is use to post any request 
    /// if it get any data then the call back function can take any type of data
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="onSuccessCallBack"></param>
    /// <param name="onErrorCallBack"></param>
    /// <returns></returns>
    public static IEnumerator PostData(string url, string token, string data, OnSuccessCallBack onSuccessCallBack, OnErrorCallBack onErrorCallBack)
    {
        if (data == null)
        {
            data = "{}";
        }
        using (UnityWebRequest uwr = UnityWebRequest.Put(url, data))
        {
            //Setting up REST API CALL TO POST
            uwr.method = UnityWebRequest.kHttpVerbPOST;

            if (token != null)
            {
                token = "Bearer " + token;
                //Setting up Header for Authorization
                uwr.SetRequestHeader("Authorization", token);
                Show.Log("Authorization " + token);
            }

            //Setting up Request Header to JSON type
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Accept", "application/json");

            Show.Log("Web Request Send");
            //Now Request can be sent
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                //No Internet
                onErrorCallBack("0");
                // Show.Log(uwr.isNetworkError.ToString());

            }
            else if (uwr.isHttpError || uwr.responseCode != 200) // here 200 means success
            {
                //Reuquest has problem And also we can categorize the error using responceCode

                onErrorCallBack(uwr.downloadHandler.text);
                //  onErrorCallBack("Error "+uwr.isHttpError.ToString()+ " -> "+ uwr.responseCode);
                //  Show.Log(uwr.isHttpError.ToString()+ " -> "+ uwr.responseCode);
            }
            else
            {
                //Success
                onSuccessCallBack(uwr.downloadHandler.text);
                // Show.Log("data-> "+ uwr.downloadHandler.text);
            }
        }
    }
    #endregion Post Request

    #region Get Request
    /// <summary>
    /// Get Data From Rest Api
    /// </summary>
    /// <param name="url"></param>
    /// <param name="token"></param>
    /// <param name="onSuccessCallBack"></param>
    /// <param name="onErrorCallBack"></param>
    /// <returns></returns>
    public static IEnumerator GetData(string url, string token, OnSuccessCallBack onSuccessCallBack, OnErrorCallBack onErrorCallBack)
    {
        //Show.Log(url);
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            if (token != null)
            {

                token = "Bearer " + token;
                //Setting up Header for Authorization
                uwr.SetRequestHeader("Authorization", token);
                Show.Log("Authorization " + token);

            }
            //Setting up Request Header to JSON type
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Accept", "application/json");

            //Now Request can be sent
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                //No Internet
                onErrorCallBack("0");
                //  Show.Log(uwr.isNetworkError.ToString());
            }
            else if (uwr.isHttpError || uwr.responseCode != 200) // here 200 means success
            {
                //Reuquest has problem And also we can categorize the error using responceCode

                onErrorCallBack(uwr.downloadHandler.text);
                // onErrorCallBack("Error "+uwr.isHttpError.ToString() + " -> " + uwr.responseCode +" -> "+ uwr.isNetworkError.ToString());
                //  Show.Log(uwr.isHttpError.ToString() + " -> " + uwr.responseCode);
            }
            else
            {
                //Success
                //  onSuccessCallBack("Any kind of data", uwr.downloadHandler.text);
                onSuccessCallBack(uwr.downloadHandler.text);
                //  Show.Log("Any kind of data "+ uwr.downloadHandler.text);
            }
        }
    }
    #endregion Get Request

    #region Check Internet Connection
    /// <summary>
    /// It check Internet connecton
    /// but also check the device have data or not
    /// </summary>
    /// <returns></returns>
    public static IEnumerator CheckInternetConnection(OnBoolCallBack onBoolCallBack)
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        request.timeout = 5;
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Show.Log("Internet Not Found");
            onBoolCallBack(false);
        }
        else
        {
            //Show.Log("Internet Success");
            onBoolCallBack(true);
        }
    }
    #endregion Check Internet Connection
}

