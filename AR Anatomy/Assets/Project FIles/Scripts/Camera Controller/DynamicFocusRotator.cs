using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFocusRotator : Rotator
{
    [Header("Focus Setup")]
    [SerializeField] int selectionFocusLayer = 0;
    [SerializeField]private float autoFocusSpeed = 1;
    Vector3 focusPoint;
    Vector3 selectedPoint;

    [Header("Double Click Setup")]
    [SerializeField]private float doubleClickDelay = 0.8f;
    [SerializeField]private float maxClickOffset = 25f;
    [SerializeField]private float autoFocusOffset = 0;
    private Coroutine doubleClickProcessor = null;
    private float preClickLength = 0;

    protected override void Start()
    {
        focusPoint = centerPivotObject.transform.position;
        base.Start();

        selectedPoint = centerPivotObject.transform.position;
        autoFocusOffset = offsetFromFocus;
    }

    protected override void Update()
    {
        SetFocusPoint();
        focusPoint = Vector3.Lerp(focusPoint, selectedPoint, autoFocusSpeed * Time.deltaTime);
        if (Input.touchCount == 2)
        {
            UpdateOffset(focusPoint);
            autoFocusOffset = offsetFromFocus;

            float deltaFraction = maxZoom - offsetFromFocus;
            Debug.Log("delta fraction = "+deltaFraction);
            if (deltaFraction < 2)
            {
                selectedPoint = centerPivotObject.transform.position;
            }
        }
        else
        {
            offsetFromFocus = Mathf.Lerp(offsetFromFocus, autoFocusOffset, autoFocusSpeed * Time.deltaTime);
        }
        
        RotateAround(focusPoint, offsetFromFocus);
        UpdatePosition(focusPoint, offsetFromFocus);
    }

    //update camera position to projection the focus point
    private void UpdatePosition(Vector3 focusPoint, float offsetFromFocus)
    {
        Vector3 frontFacinPoint = cam.transform.forward * offsetFromFocus;
        Vector3 projectionPoint = focusPoint - frontFacinPoint;
        cam.transform.position = projectionPoint;
    }

   
    //set focus point and make it the center pivote point of projection
    public void SetFocusPoint()
    {
        if (IsPointerOverUIObject()) return;
        if (!isDoubleClicked) return;
        
        ProjectionInformation projectionInfo = GetProjectionInfo(Input.mousePosition, selectionFocusLayer);
        if (projectionInfo != null)
        {
            selectedPoint = projectionInfo.focusPoint;
            autoFocusOffset = projectionInfo.projectionOffset;
        }
    }

    //return the selected uniq point information to project that kind of object
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

    //return true if double tap on the screen
    private bool isDoubleClicked
    {
        get 
        {
            if (Input.touchCount != 1) return false;

            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began) return false;

            float newClickLength = touch.position.magnitude;
            float clickOffset = Mathf.Abs(preClickLength - newClickLength);
            preClickLength = newClickLength;
            if (clickOffset > maxClickOffset) return false;

            if (doubleClickProcessor != null)
            {
                preClickLength = 0;
                StopCoroutine(doubleClickProcessor);
                doubleClickProcessor = null;
                return true;
            }
            else
            {
                selectedPoint = centerPivotObject.transform.position;
                autoFocusOffset = maxZoom;
            }
            doubleClickProcessor = StartCoroutine(clickProcessor(doubleClickDelay));
            return false;

            //leisn the time between two click to detect double click action
            IEnumerator clickProcessor(float clickDelay)
            {
                yield return new WaitForSeconds(clickDelay);
                preClickLength = 0;
                doubleClickProcessor = null;
            }
        }
    }


}