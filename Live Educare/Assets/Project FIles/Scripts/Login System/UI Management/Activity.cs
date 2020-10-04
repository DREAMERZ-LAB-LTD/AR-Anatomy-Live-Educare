using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Activity
{
    private GameObject CurrentPanel;//main activity panel
    private GameObject TostMessageObj;//tost message TMPtext object

    public Activity() { }
    public Activity(GameObject TostMessageObj) 
    {
        this.TostMessageObj = TostMessageObj;
    }


    /// <summary>
    /// set active new activity and set to deactive automaticly previous activity
    /// </summary>
    /// <param name="NewActivity">New activity that will be show on UI</param>
    public void Show(GameObject NewActivity)
    {
        if (CurrentPanel != null)
        {
            CurrentPanel.SetActive(false);
        }
        CurrentPanel = NewActivity;
        CurrentPanel.SetActive(true);
    }



    /// <summary>
    /// show tost message on ui screen
    /// </summary>
    /// <param name="message"> tost message information</param>
    /// <param name="duration">message visible duration</param>
    /// <param name="color">message visible color</param>
    /// <returns></returns>
    public async Task ShowToast(string message, float duration, Color color)
    {
        TostMessageObj.SetActive(true);
        TostMessageObj.GetComponent<TextMeshProUGUI>().text = message;
        TostMessageObj.GetComponent<TextMeshProUGUI>().color = color;

        duration = Mathf.Clamp(duration,0.2f, 2f) * 1000f;//convet milliseconds to seconds and clam max duration
        await Task.Delay((int)duration);//Wait for delay

        TostMessageObj.SetActive(false);
    }

}
