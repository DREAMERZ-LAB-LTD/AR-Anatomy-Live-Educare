using DL.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeSceneButtnController : MonoBehaviour
{
    [Header("Safety warning Panel Property")]
    [SerializeField] private UIContentAnimatorList SafetyWarning;
    //public GameObject SafetyWarningPanel;
    public Button CloseBtn;
    public Toggle IAggre;

    [Header("Scene Loading Visual Elements")]
    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;
    public Image imageFillArea;


    void Start()
    {
        CloseBtn.onClick.RemoveAllListeners();
        CloseBtn.onClick.AddListener(OnclickClose);


        SafetyWarning.gameObject.SetActive(false);
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


    public void OnClickShwoInAR()
    {
        if (IUnderstood)
        {
            LoadoadARScene();
        }
        else
        {
            ShowSafetyWarning();
        }
    }
    public void OnClickI_UnderstandBtn()
    {
        if (!IUnderstood) IUnderstood = IAggre.isOn;
        LoadoadARScene();
    }
    private void LoadoadARScene ()
    {
        loadingPanel.SetActive(true);
        loadingText.text = "Loading...";

        StartCoroutine(LoadNewScene("ARScene"));
    }

    private void ShowSafetyWarning()
    {
        SafetyWarning.gameObject.SetActive(true);
        SafetyWarning.Appear();
    }
    private void OnclickClose()
    {
        SafetyWarning.DisAppear();
        Invoke("DisableSafetyPanel", 0.25f);
    }
    private void DisableSafetyPanel()
    {
        SafetyWarning.gameObject.SetActive(false);
    }
    public void OnClickLogOut()
    {
        LoginRegisterSystem.AuthManager.LogOut();
        SceneManager.LoadScene(1);
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
