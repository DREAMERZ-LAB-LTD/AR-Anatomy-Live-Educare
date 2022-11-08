using DL.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Anatomy3D_To_AR_SceneLoader : SceneController
{

    [Header("Scene Loading Visual Elements")]
    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;
    public Image imageFillArea;


    public void LoadoadARScene ()
    {
        loadingPanel.SetActive(true);
        loadingText.text = "Loading...";

        StartCoroutine(LoadNewScene("ARScene"));
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

            if (async.progress == 0.9f)
            {
                imageFillArea.fillAmount = 1f;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
