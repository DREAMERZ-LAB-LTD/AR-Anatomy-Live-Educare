using DL.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneButtnController : MonoBehaviour
{
    [SerializeField] private UIContentAnimatorList SafetyWarning;
    [Header("Scene Loading Visual Elements")]
    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;
    public Image imageFillArea;

    [Header("Safety warning Panel Property")]
    public GameObject SafetyWarningPanel;
    public Button CloseBtn;
    public Button I_UnderstandBtn;
    public Toggle IAggre;
    [SerializeField] private Button StartBtn;

    void Start()
    {
        CloseBtn.onClick.RemoveAllListeners();
        CloseBtn.onClick.AddListener(OnclickClose);

        I_UnderstandBtn.onClick.RemoveAllListeners();
        I_UnderstandBtn.onClick.AddListener(OnClickloadARScene);
        I_UnderstandBtn.onClick.AddListener(delegate
        { 
            if(!IUnderstood) IUnderstood = IAggre.isOn; 
        });

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
            return 1 == PlayerPrefs.GetInt("understand");
        }
        set
        {
            int u = value ? 1 : 0;
            PlayerPrefs.SetInt("understand", u);
        }
    }

    public void OnClickloadARScene ()
    {
        loadingPanel.SetActive(true);
        loadingText.text = "Loading...";

        StartCoroutine(LoadNewScene("ARScene"));
    }

    public void OnStartButtonClick()
    {
        SafetyWarningPanel.SetActive(true);
        SafetyWarning.Appear();
    }
    private void OnclickClose()
    {
        SafetyWarning.DisAppear();
        Invoke("DisableSafetyPanel", 0.25f);
    }
    private void DisableSafetyPanel()
    {
        SafetyWarningPanel.SetActive(false);
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            float progress = (Mathf.Clamp01(async.progress / 0.9f));
            loadingText.text = (int)(progress * 100f) + "%";

            imageFillArea.fillAmount = async.progress;

            if (async.progress==0.9f)
            {
                imageFillArea.fillAmount = 1f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
