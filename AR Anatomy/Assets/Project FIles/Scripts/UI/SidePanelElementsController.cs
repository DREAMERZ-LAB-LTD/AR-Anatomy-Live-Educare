using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SidePanelElementsController : MonoBehaviour
{
    [Header("Fade Duration")]
    [SerializeField] private float time = 1.00f;
    [Space]
    [SerializeField] private SidePanelElement closeButton;
    [SerializeField] private List<SidePanelElement> sidepanelElements = new List<SidePanelElement>();


    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (Utility.IsPointerOverUIObject()) return;
        if (Utility.IsPointerOverCollider()) return;

        Close();
    }

    /// <summary>
    /// will enabling the panel elements W.R.To fade duration
    /// </summary>
    public void OnClickShow()
    {
        closeButton.StartAnimation(0, time, false);
        for (int i = 0; i < sidepanelElements.Count; i++)
        {
            SidePanelElement element = sidepanelElements[i];
            element.StartAnimation(1, time, true);
        }
    }
    /// <summary>
    /// will disable the panel elements W.R.To fade duration
    /// </summary>
    public void Close()
    {
        closeButton.StartAnimation(1, time, true);
        for (int i = 0; i < sidepanelElements.Count; i++)
        {
            SidePanelElement element = sidepanelElements[i];
            element.StartAnimation(0, time, false);
        }
    }
}
