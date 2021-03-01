using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTracker : MonoBehaviour
{
    public bool useUpdate = true;
#pragma warning disable 649
    [SerializeField]private RectTransform ViewModeBtn;
    [SerializeField]private RectTransform DynamicRect;
    [SerializeField]private RectTransform BackgroundRect;
    [SerializeField]private float offset;

    private Vector3 defaultPostion;
#pragma warning disable 649
    private void Start()
    {
        float btnBodyHeight = ViewModeBtn.rect.height;
        float totalHeight = backgroundHeight + btnBodyHeight * 0.5f;

        defaultPostion = new Vector3(ViewModeBtn.position.x, totalHeight, ViewModeBtn.position.z);
        ViewModeBtn.transform.position = defaultPostion + new Vector3(0, offset, 0);
    }
    void Update()
    {
        if (!useUpdate) return;

        ViewModeBtn.position = ChangePos(defaultPostion);
    }


    private Vector3 ChangePos(Vector3 center)
    {

        float currScale = DynamicRect.transform.localScale.y;
        float contentHeight = DynamicRect.rect.height;
        contentHeight *= currScale;

        float newFraction = contentHeight - backgroundHeight;
        float newHeght = center.y + newFraction;

        float defaultPivotePoint = center.y + offset;
      //  Debug.Log(contentHeight);
       // Debug.Log("default " + defaultPivotePoint + " new " + newHeght);
        if (newHeght < defaultPivotePoint) 
        {
            return new Vector3(center.x, defaultPivotePoint, center.z);
        }

        newHeght += offset;
        return new Vector3(center.x, newHeght, center.z);
    }
    private float backgroundHeight => Screen.height * BackgroundRect.anchorMax.y;
}
