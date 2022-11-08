using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIMerge : MonoBehaviour
{
    [SerializeField] bool deactivateTargets = true;
    public List<RectTransform> targets = new List<RectTransform>();

    public List<TextMeshProUGUI> targetText = new List<TextMeshProUGUI>();
    [SerializeField] TextMeshProUGUI myText;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (deactivateTargets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].gameObject.SetActive(false);
            }
           
        }
        else
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].gameObject.GetComponent<Mask>())
                {
                    targets[i].gameObject.GetComponent<Mask>().showMaskGraphic = false;
                }
            }

        }

        for (int i = 0; i < targetText.Count; i++)
        {
            targetText[i].gameObject.SetActive(false);
        }
    }

    [ContextMenu("t")]
    public void Test()
    {
        Merge(0);
    }


    public void Merge(int i)
    {
        rectTransform.DOMove(targets[i].position, 0.5f);

        rectTransform.DOSizeDelta(targets[i].sizeDelta, 0.5f);

        if (gameObject.GetComponent<Image>())
        {
            if (targets[i].gameObject.GetComponent<Image>())
            {
                gameObject.GetComponent<Image>().DOColor(targets[i].gameObject.GetComponent<Image>().color, 0.5f);
            }
            if (targets[i].gameObject.GetComponent<Button>())
            {
                targets[i].gameObject.GetComponent<Button>().targetGraphic = gameObject.GetComponent<Image>();
            }
        }

        if (myText) 
            StartCoroutine(TextRoutine(targetText[i].text));
    }

    IEnumerator TextRoutine(string targetText)
    {
        string currentText = myText.text;
        for (int i = currentText.Length; i >=0; i--)
        {
            myText.text = (currentText.Substring(0, i) + "|");
            yield return new WaitForSeconds(Random.Range(0.005f, 0.01f));
        }
        myText.text = string.Empty;

        yield return new WaitForSeconds(0.1f);


        for (int i = 0; i <= targetText.Length; i++)
        {
            myText.text = (targetText.Substring(0, i) + "|");
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }
        myText.text = targetText;
    }
}
