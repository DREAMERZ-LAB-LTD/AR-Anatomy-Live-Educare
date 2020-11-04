using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScript : MonoBehaviour
{
    public Image splashImage;

    // Use this for initialization
    private IEnumerator Start()
    {
        splashImage.color = new Color(splashImage.color.r, splashImage.color.g, splashImage.color.b, 1f);

        //////Color c = splashImage.color;
        //////c.a = 1;
        //////splashImage.color = c;

        //yield return new WaitForSeconds(2f);
        FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("HomeScene");
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
