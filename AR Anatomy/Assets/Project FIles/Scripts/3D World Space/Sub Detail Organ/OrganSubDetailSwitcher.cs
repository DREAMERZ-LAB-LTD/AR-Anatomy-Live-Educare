using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrganSubDetailSwitcher : MonoBehaviour
{
    [Header("Raycast Layer Mask")]
    [SerializeField] private int targetLayer = 0;
#pragma warning disable 649
    [Space]
    [SerializeField] InnerOrganHandler innerOrganHandler;


    [Header("Action Response")]
    [SerializeField] UnityEvent onExploreableDetected;
    [SerializeField] UnityEvent onExploreableNotDetected;
    [SerializeField] UnityEvent onExploringStart;
    [SerializeField] UnityEvent onExploringClose;
#pragma warning restore 649


    private bool isSeletionMode = true;
    public void SetSelectionState(bool state)=>isSeletionMode = state;
    
    private void Update()
    {
        //input masking
        if (!Input.GetMouseButtonDown(0)) return;
        if (Utility.IsPointerOverUIObject()) return;

        //try to select exploreable organ by mouse clicking
        SelectOrgan();
        
    }

    private void SelectOrgan()
    {
        ExploreableOrgan organ = GetRaycastInfo(Input.mousePosition, targetLayer);
        if (organ != null) 
        {
            if (isSeletionMode)
            {
                innerOrganHandler.SetInnerOrganIndex(organ.organIndex);
            }

            onExploreableDetected.Invoke();
        }
        else
            onExploreableNotDetected.Invoke();
    }

    public void OnClickExploreOrgan()
    {
        onExploringStart.Invoke();
        innerOrganHandler.SetActiveInnerOrgan(true);
    }
    public void OnClickBackTo3DAnatomy()
    {
        innerOrganHandler.SetActiveInnerOrgan(false);
        onExploringClose.Invoke();
    }

    private ExploreableOrgan GetRaycastInfo(Vector2 screenPoint, int layermask)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        int layer = 1 << layermask;
        if (Physics.Raycast(ray, out RaycastHit hit, 500.0f, layer))
        {
            return hit.collider.GetComponent<ExploreableOrgan>();
        }
        return null;
    }
}
