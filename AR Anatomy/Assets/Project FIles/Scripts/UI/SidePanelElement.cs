using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanelElement : MonoBehaviour
{
    [Header("Transform Movemnet Offset")]
    [SerializeField] private Vector2 offset = new Vector2();
    [SerializeField] private float motionSpeed = 1;
    private Vector2 initialPosition = new Vector2();

    [Header("UI Visual Elements Reference")]
    [SerializeField]List<Image>images = new List<Image>();
    [SerializeField] List<TextMeshProUGUI> textsTMP = new List<TextMeshProUGUI>();
    [SerializeField] List<Text> texts = new List<Text>();

    private Coroutine currentThread = null;


    private void Awake() => initialPosition = transform.position;
    
    /// <summary>
    /// Play the UI transition animation
    /// </summary>
    /// <param name="alpha">new alpha amount</param>
    /// <param name="time">animation playing duration</param>
    /// <param name="setActive">after playing the animation the gameobject will set active or deactive</param>
    public void StartAnimation(float alpha, float time, bool setActive)
    {
        if (currentThread != null)
        {
            StopCoroutine(currentThread);
        }
      
        gameObject.SetActive(true);
       
      
        Vector2 startPoint = setActive ? initialPosition + offset : initialPosition;
        Vector2 endPoint = setActive ? initialPosition : initialPosition + offset;
        currentThread = StartCoroutine(Showing(alpha, startPoint, endPoint, time, setActive));
    }

    /// <summary>
    /// starting the enable and disable animation
    /// </summary>
    /// <param name="alpha">new alpha amount</param>
    /// <param name="startPoint">where from start moving </param>
    /// <param name="endPoint">where to end moving </param>
    /// <param name="time">animation playing duration </param>
    /// <param name="setactive">after animation the gameobject will enable or disable</param>
    /// <returns></returns>
    private IEnumerator Showing(float alpha,Vector2 startPoint, Vector2 endPoint, float time, bool setactive)
    {
        if (!setactive)
        {
            SetInteractableState(false);
        }

        float currTime = 0;
        while (currTime <= time)
        {
            currTime += Time.deltaTime;
            float t = currTime / time;

            Image_ChageAlpha(alpha, t);
            TmpText_ChageAlpha(alpha, t);
            Text_ChageAlpha(alpha, t);
            SetMotion(startPoint, endPoint, t);
            if (t > 0.5f)
            {
                SetInteractableState(true);
            }
            yield return null;
        }
        if (gameObject.activeSelf != setactive)
        {
            gameObject.SetActive(setactive);
        }
        
        currentThread = null;
    }

    /// <summary>
    /// change alpha of the text color to visible and invisible
    /// </summary>
    /// <param name="alpha">alpha amount</param>
    /// <param name="t">change amount range between current alpha to new alpha,  where t = >= 0 or t = <= 1</param>
    private void Text_ChageAlpha(float alpha, float t)
    {
        for (int i = 0; i < texts.Count; i++)
        {
            Text text = texts[i];
            Color textColor = text.color;

            float a = Mathf.Lerp(textColor.a, alpha, t);
            textColor.a = a;
            text.color = textColor;
        }
    }
    /// <summary>
    /// change alpha of the text mesh pro text color to visible and invisible
    /// </summary>
    /// <param name="alpha">alpha amount</param>
    /// <param name="t">change amount range between current alpha to new alpha,  where t = >= 0 or t = <= 1</param>
    private void TmpText_ChageAlpha(float alpha, float t)
    {
        for (int i = 0; i < textsTMP.Count; i++)
        {
            TextMeshProUGUI text = textsTMP[i];
            Color textColor = text.color;
            
            float a = Mathf.Lerp(textColor.a, alpha, t);
            textColor.a = a;
            text.color = textColor;
        }
    }
    /// <summary>
    /// change alpha of the image color to visible and invisible
    /// </summary>
    /// <param name="alpha">alpha amount</param>
    /// <param name="t">change amount range between current alpha to new alpha, where t = >= 0 or t = <= 1</param>
    private void Image_ChageAlpha(float alpha, float t)
    {
        for (int i = 0; i < images.Count; i++)
        {
            Image image = images[i];
            Color imageColor = image.color;
            float a = Mathf.Lerp(imageColor.a, alpha, t);
            imageColor.a = a;

            image.color = imageColor;
        }
    }

    /// <summary>
    /// change position linearly between two vector point
    /// </summary>
    /// <param name="a">where from start moving</param>
    /// <param name="b">where to end moving </param>
    /// <param name="t">change amount where t = >= 0 or t = <= 1/param>
    private void SetMotion(Vector2 a, Vector2 b, float t)
    {
        float speed = Mathf.Clamp(t * motionSpeed,0.00f, 1.00f);
        Vector2 point = Vector2.Lerp(a, b, speed);
        transform.position = point;
    }

    public void SetInteractableState(bool state)
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.interactable = state;
        }

        Slider slider = GetComponent<Slider>();
        if (slider != null)
        {
            slider.interactable = state;
        }
    }
}
