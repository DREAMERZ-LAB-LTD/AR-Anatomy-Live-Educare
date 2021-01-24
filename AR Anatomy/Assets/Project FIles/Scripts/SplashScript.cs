using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScript : SceneController
{
    [SerializeField]private int NextSceneNo = 1;
    [SerializeField]private Image splashImage;

    // Use this for initialization
    private IEnumerator Start()
    {
        splashImage.color = new Color(splashImage.color.r, splashImage.color.g, splashImage.color.b, 1f);

        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2f);
        LoadScene(NextSceneNo);
    }

    private void FadeIn()
    {
        splashImage.CrossFadeAlpha(0f, 2.5f, false);
    }

    private void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.8f, 2f, false);
    }
}
