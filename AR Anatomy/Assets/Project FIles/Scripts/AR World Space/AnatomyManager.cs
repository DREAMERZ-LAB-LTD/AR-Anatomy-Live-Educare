using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Anatomy.Body;
public class AnatomyManager : AnatomySystem
{
#pragma warning disable 649
    [Header("Anatomy Setup")]
    public GameObject fullBody;
    public GameObject detailedButton;
    public GameObject rootMaleAnatomy;


    public Slider layerSlider;
    public static int SelectedLAyer = 0;



    public float fadeDelay = 0.0f;
    public float fadeTime = 0.5f;
    public bool fadeInOnStart = false;
    public bool fadeOutOnStart = false;
    private bool logInitialFadeSequence = false;
    public GameObject organBackButton;

    public GameObject heart;
    private Color[] colors;
#pragma warning restore 649

    void Awake()
    {
        layerSlider.maxValue = bodyParts.Length - 1;
    }
    private void OnEnable() 
    {
        layerSlider.value = SelectedLAyer;
    }

    public void OnBackButtonClick()
    {
        if (heart.activeSelf)
        {
            fullBody.SetActive(true);
            heart.SetActive(false);
            layerSlider.gameObject.SetActive(true);
        }
    }

    public void ShowLayer()
    {
        SelectedLAyer = (int)layerSlider.value;
        
        ShowBodyLayer(SelectedLAyer);

    }

    public void ShowHeart()
    {
        fullBody.SetActive(false);
        heart.SetActive(true);

        layerSlider.gameObject.SetActive(false);
    }

    float MaxAlpha()
    {
        float maxAlpha = 0.0f;
        Renderer[] rendererObjects = GetComponentsInChildren<Renderer>();
        foreach (Renderer item in rendererObjects)
        {
            maxAlpha = Mathf.Max(maxAlpha, item.material.color.a);
        }
        return maxAlpha;
    }

    void FadeIn(GameObject targetObject)
    {
        FadeIn(fadeTime, targetObject);
    }

    void FadeOut(GameObject targetObject)
    {
        FadeOut(fadeTime, targetObject);
    }

    void FadeIn(float newFadeTime, GameObject targetObject)
    {
        StopAllCoroutines();
        StartCoroutine(FadeSequence(newFadeTime, targetObject));
    }

    void FadeOut(float newFadeTime, GameObject targetObject)
    {
        StopAllCoroutines();
        StartCoroutine(FadeSequence(-newFadeTime, targetObject));
    }

    // fade sequence
    IEnumerator FadeSequence(float fadingOutTime, GameObject targetObject)
    {
        // log fading direction, then precalculate fading speed as a multiplier
        bool fadingOut = (fadingOutTime < 0.0f);
        float fadingOutSpeed = 1.0f / fadingOutTime;

        // grab all child objects
        Renderer[] rendererObjects = targetObject.GetComponentsInChildren<Renderer>();
        if (colors == null)
        {
            //create a cache of colors if necessary
            colors = new Color[rendererObjects.Length];

            // store the original colours for all child objects
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                colors[i] = rendererObjects[i].material.color;
            }
        }

        // make all objects visible
        for (int i = 0; i < rendererObjects.Length; i++)
        {
            rendererObjects[i].enabled = true;
        }


        // get current max alpha
        float alphaValue = MaxAlpha();


        // This is a special case for objects that are set to fade in on start. 
        // it will treat them as alpha 0, despite them not being so. 
        if (logInitialFadeSequence && !fadingOut)
        {
            alphaValue = 0.0f;
            logInitialFadeSequence = false;
        }

        // iterate to change alpha value 
        while ((alphaValue >= 0.0f && fadingOut) || (alphaValue <= 1.0f && !fadingOut))
        {
            alphaValue += Time.deltaTime * fadingOutSpeed;

            for (int i = 0; i < rendererObjects.Length; i++)
            {
                Color newColor = (colors != null ? colors[i] : rendererObjects[i].material.color);
                newColor.a = Mathf.Min(newColor.a, alphaValue);
                newColor.a = Mathf.Clamp(newColor.a, 0.0f, 1.0f);
                rendererObjects[i].material.SetColor("_Color", newColor);
            }

            yield return null;
        }

        // turn objects off after fading out
        if (fadingOut)
        {
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                rendererObjects[i].enabled = false;
            }
        }
    }
}
