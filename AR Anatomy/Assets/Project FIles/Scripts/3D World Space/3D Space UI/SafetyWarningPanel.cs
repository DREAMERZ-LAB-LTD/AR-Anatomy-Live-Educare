using DL.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SafetyWarningPanel : MonoBehaviour
{
    [Header("Safety warning Panel Property")]
    [SerializeField] private UIContentAnimatorList SafetyWarning;
    public Toggle IAggre;

    [SerializeField] UnityEvent OnLoadARScene;


    public void OnClickShwoInAR()
    {
        if (IUnderstood)
        {
            OnLoadARScene.Invoke();
        }
        else
        {
            ShowSafetyWarning();
        }
    }

    private void ShowSafetyWarning()
    {
        SafetyWarning.gameObject.SetActive(true);
        SafetyWarning.Appear();
    }
    public void OnclickClose()
    {
        StartCoroutine(close(0.25f));

        IEnumerator close(float closeDelay)
        {
            SafetyWarning.DisAppear();
            yield return new WaitForSeconds(closeDelay);
            SafetyWarning.gameObject.SetActive(false);
        }
    }

    public void OnClickI_UnderstandBtn()
    {
        if (!IUnderstood) IUnderstood = IAggre.isOn;
        OnLoadARScene.Invoke();
    }

    private bool IUnderstood
    {
        get
        {
            return 1 == PlayerPrefs.GetInt("understand");
        }
        set
        {
            int u = value ? 1 : 0;
            PlayerPrefs.SetInt("understand", u);
        }
    }

}
