using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrganSubDetailSwitcher : MonoBehaviour
{
    [Header("Raycast Layer Mask")]
    [SerializeField] private int targetLayer = 0;



    [Header("Action Response")]
    [SerializeField] UnityEvent onExploreableDetected;
    [SerializeField] UnityEvent onExploreableNotDetected;
    [SerializeField] UnityEvent onExploringStart;
    [SerializeField] UnityEvent onExploringClose;

    [Tooltip("Current Explored Organ")]
    private ExploreableOrgan exploreOrgan = null;


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
        //  ExploreableOrgan organ = GetRaycastInfo(Input.mousePosition, targetLayer);
        if (isSeletionMode)
        { 
            exploreOrgan = organ;
        }

        if (organ != null)
            onExploreableDetected.Invoke();
        else
            onExploreableNotDetected.Invoke();

       
        
    }

    public void OnClickExploreOrgan()
    {
        if (exploreOrgan == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Organ Not Selected");
#endif
            return;
        }
        onExploringStart.Invoke();
        exploreOrgan.Explore(true);
    }
    public void OnClickBackTo3DAnatomy()
    {
        if (exploreOrgan == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Organ Not Selected");
#endif
            return;
        }

        exploreOrgan.Explore(false);
        exploreOrgan = null;
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
