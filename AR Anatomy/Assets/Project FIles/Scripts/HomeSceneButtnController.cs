using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneButtnController : MonoBehaviour
{
    public GameObject homeScreen;
    public GameObject instructionScreen;
    public GameObject aboutScreen;
    public GameObject settingScreen;
    public GameObject textCommingSoon;

    public string imageDownloadLink;

    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;
    //public Slider sliderBar;
    public Image imageFillArea;

    

    [Header("Safety warning Panel Property")]
    public GameObject SafetyWarningPanel;
    public Button CloseBtn;
    public Button I_UnderstandBtn;
    public Toggle IAggre;
    [SerializeField] private Button StartBtn;

    void Start()
    {

        Debug.Log("Agre" + IUnderstood);

        CloseBtn.onClick.RemoveAllListeners();
        CloseBtn.onClick.AddListener(delegate { SafetyWarningPanel.SetActive(false); });

        I_UnderstandBtn.onClick.RemoveAllListeners();
        I_UnderstandBtn.onClick.AddListener(OnClickloadARScene);
        I_UnderstandBtn.onClick.AddListener(delegate { if(!IUnderstood) IUnderstood = IAggre.isOn; });

        StartBtn.onClick.RemoveAllListeners();
        if (IUnderstood)
        {
            StartBtn.onClick.AddListener(OnClickloadARScene);
        }
        else 
        {
            StartBtn.onClick.AddListener(OnStartButtonClick);
        }

        SafetyWarningPanel.SetActive(false);
    }

    private bool IUnderstood
    {
        get
        {
            return 1 == PlayerPrefs.GetInt("Understand");
        }
        set
        {
            int u = value ? 1 : 0;
            PlayerPrefs.SetInt("Understand", u);
        }
    }

    public void OnClickloadARScene ()
    {
        homeScreen.SetActive(false);
        loadingPanel.SetActive(true);
        loadingText.text = "Loading...";

        //SceneManager.LoadScene("ARScene");
        StartCoroutine(LoadNewScene("ARScene"));
    }

    public void OnStartButtonClick()
    {
        SafetyWarningPanel.SetActive(true);
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            float progress = (Mathf.Clamp01(async.progress / 0.9f));
            //sliderBar.value = progress;
            loadingText.text = (int)(progress * 100f) + "%";
            //sliderBar.value = async.progress;

            imageFillArea.fillAmount = async.progress;

            if (async.progress==0.9f)
            {
                imageFillArea.fillAmount = 1f;
                //sliderBar.value = 1f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void OnInstructionButtonClick()
    {
        if (homeScreen.activeSelf)
            homeScreen.SetActive(false);
        if (!instructionScreen.activeSelf)
            instructionScreen.SetActive(true);
    }

    public void OnAboutButtonClick()
    {
        if (homeScreen.activeSelf)
            homeScreen.SetActive(false);
        if (!aboutScreen.activeSelf)
            aboutScreen.SetActive(true);
    }

    public void OnSettingsButtonClick()
    {
        StartCoroutine(WaitSomeTime());
    }

    private IEnumerator WaitSomeTime()
    {
        textCommingSoon.SetActive(true);
        yield return new WaitForSeconds(1);
        textCommingSoon.SetActive(false);
    }

    public void OnBackButtonClick()
    {
        if (!homeScreen.activeSelf)
        {
            homeScreen.SetActive(true);
        }
        if (instructionScreen.activeSelf)
        {
            instructionScreen.SetActive(false);
        }
        if (aboutScreen.activeSelf)
        {
            aboutScreen.SetActive(false);
        }
    }

    public void OnDownloadButtonClick()
    {
        Application.OpenURL(imageDownloadLink);
    }
}
