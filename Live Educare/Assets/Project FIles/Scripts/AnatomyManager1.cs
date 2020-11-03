using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class AnatomyManager1 : MonoBehaviour
{
    public GameObject fullBody;
    public GameObject[] bodyParts;
    public Slider layerSlider;
    public GameObject selectedPart;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI detailsText;
    public GameObject detailedButton;
    public GameObject detailPanel;
    public GameObject rootMaleAnatomy;
    private bool _rotate;


    public float fadeDelay = 0.0f;
    public float fadeTime = 0.5f;
    public bool fadeInOnStart = false;
    public bool fadeOutOnStart = false;
    private bool logInitialFadeSequence = false;
    public GameObject organBackButton;

    public GameObject heart;

    private Color[] colors;
    // Start is called before the first frame update
    void Start()
    {
        layerSlider.maxValue = 8;
    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void MaleAnatomyRotateToggle()
    {
        _rotate = !_rotate;
    }

    private void RotateMaleAnatomy()
    {
        if (_rotate)
        {
            rootMaleAnatomy.transform.localPosition = Vector3.Lerp(rootMaleAnatomy.transform.localPosition, new Vector3(0, 0, 0), 2 * Time.deltaTime);
            rootMaleAnatomy.transform.localEulerAngles = Vector3.Slerp(rootMaleAnatomy.transform.localEulerAngles, new Vector3(0, 0, 0), 2 * Time.deltaTime);
        }
        else if (!_rotate)
        {
            rootMaleAnatomy.transform.localPosition = Vector3.Lerp(rootMaleAnatomy.transform.localPosition, new Vector3(0, 0, -0.45f), 2 * Time.deltaTime);
            rootMaleAnatomy.transform.localEulerAngles = Vector3.Slerp(rootMaleAnatomy.transform.localEulerAngles, new Vector3(90, 0, 0), 2 * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rootMaleAnatomy.transform.localEulerAngles = new Vector3(90, 0, 0);
        RotateMaleAnatomy();
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
            {
                return;
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (selectedPart != null)
                    {
                        selectedPart.GetComponent<Renderer>().material.color = Color.white;

                    }
                    selectedPart = hit.transform.gameObject;
                    selectedPart.GetComponent<Renderer>().material.color = Color.green;
                    nameText.text = hit.transform.name;
                    detailsText.text = "This is a " + hit.transform.name;
                    if (selectedPart.tag == "Interactable")
                    {
                        detailedButton.SetActive(true);
                    }
                    else
                    {
                        detailedButton.SetActive(false);
                    }
                    detailPanel.SetActive(true);

                }
                else
                {
                    if (selectedPart != null)
                    {
                        selectedPart.GetComponent<Renderer>().material.color = Color.white;
                        selectedPart = null;
                    }
                    detailPanel.SetActive(false);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (heart.activeSelf)
            {
                fullBody.SetActive(true);
                heart.SetActive(false);
                layerSlider.gameObject.SetActive(true);
            }
        }
    }

    public void OnBackButtonClick()
    {
        if (heart.activeSelf)
        {
            fullBody.SetActive(true);
            heart.SetActive(false);
            layerSlider.gameObject.SetActive(true);
            organBackButton.SetActive(false);
        }
    }

    public void ShowLayer()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            if (i == layerSlider.value)
            {

                bodyParts[i].SetActive(true);

            }
            else if (i < layerSlider.value)
            {
                bodyParts[i].SetActive(false);
            }
            if (selectedPart != null)
            {
                selectedPart.GetComponent<Renderer>().material.color = Color.white;

            }
        }
    }

    public void ShowHeart()
    {
        organBackButton.SetActive(true);
        fullBody.SetActive(false);
        heart.SetActive(true);
        detailPanel.SetActive(false);
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

    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
