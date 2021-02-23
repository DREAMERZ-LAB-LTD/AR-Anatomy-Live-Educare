using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFocusRotator : Rotator
{
    [Tooltip("Raycast Layer Mask")]
    [SerializeField] int selectionFocusLayer = 0;
    [Tooltip("")]
    [SerializeField]private float autoFocusSpeed = 1;
    [SerializeField]private float autoFocusOffset = 0;
    Vector3 selectedPoint;

    [Header("Double Click Setup")]
    [SerializeField] ClickResponse clickResponse;

    
    protected override void Start()
    {
        base.Start();

        selectedPoint = focusPoint;
        autoFocusOffset = projectionOffset;
    }

    public override void RestView()
    {
        autoFocusOffset = maxOffset;
        selectedPoint = initialFocusPoint.position;
        base.RestView();
    }

    protected override void Update()
    {
        SetFocusPoint();
        focusPoint = Vector3.Lerp(focusPoint, selectedPoint, autoFocusSpeed * Time.deltaTime);

        
        if (Input.touchCount == 2)//updating manual zooming offset data
        {
            UpdateProjectionOffset(focusPoint);
            autoFocusOffset = projectionOffset;

            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2 t1PrePos = touch1.position - touch1.deltaPosition;
            Vector2 t2PrePos = touch2.position - touch2.deltaPosition;
            float preOffset = (t1PrePos - t2PrePos).magnitude;
            float currOffset = (touch1.position  - touch2.position).magnitude;
            bool isZoomingOut = currOffset < preOffset;
           
            if (isZoomingOut)
            {       
                // back to initial projection distance when user try to fully zooming out
                float availableZoomAmount = maxOffset - projectionOffset;
                if (availableZoomAmount < maxOffset * 0.2f)
                {
                    selectedPoint = initialFocusPoint.transform.position;
                    autoFocusOffset = maxOffset;
                }
            }
        }
        else
        {
            LearpProjectionOffset(autoFocusOffset, autoFocusSpeed);
        }
        
        UpdateRotation();
        UpdatePosition(focusPoint, projectionOffset);
    }

   
    //set focus point and make it the center pivote point of projection
    public void SetFocusPoint()
    {
        if (Utility.IsPointerOverUIObject()) return;
        if (!clickResponse.isDoubleClicked) return;

        ProjectionInformation projectionInfo = GetProjectionInfo(Input.mousePosition, selectionFocusLayer);
        if (projectionInfo != null)
        {
            //set focus point information to focus spacific point
            selectedPoint = projectionInfo.focusPoint;
            autoFocusOffset = projectionInfo.projectionOffset;
        }
        else
        {
            //set focus point as default to view anatomy full body
            selectedPoint = initialFocusPoint.transform.position;
            autoFocusOffset = maxOffset;
        }
    }

    //return the selected uniq point information to focus that kind of object
    private ProjectionInformation GetProjectionInfo(Vector2 screenPoint, int layerNo)
    {
        int layer = 1 << layerNo;
        Ray ray = cam.ScreenPointToRay(screenPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, layer))
        {
            ProjectionInformation info = hit.collider.GetComponent<ProjectionInformation>();
            if (info != null)
            { 
#if UNITY_EDITOR
            Debug.DrawLine(cam.transform.position, hit.point, Color.green);
#endif
                return info;
            }
        }
        return null;
    }
}