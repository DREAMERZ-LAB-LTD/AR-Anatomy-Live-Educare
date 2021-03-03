using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotator : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] protected Camera cam;
    [Header("Rotation Setup")]
    [SerializeField] Transform camInitialPosition;
    [SerializeField] protected Transform initialFocusPoint;
    [Range(0.01f, 1.00f)]
    [SerializeField] float rotateSpeed = 0.05f;
#pragma warning restore 649

    [Header("Focus Setup")]
    [Tooltip("Vertical Projection Offset")]
    [SerializeField, Range(-1.00f, 1.00f)] float  verticalOffset = 0;
    [Tooltip("Total camera offset from focus point")]
    protected float projectionOffset = 3;
    [Tooltip("Minimum Projection Offset")]
    [SerializeField] protected float minOffset = 0.05f;
    [Tooltip("Maximum Projection Offset")]
    [SerializeField] protected float maxOffset = 2.5f;

    protected bool isClickedOnUI = false;
#pragma warning disable 649
    protected Vector3 focusPoint;
#pragma warning restore 649

    protected virtual void Start()
    {
        RestView();
        Vector3 initialFocus = initialFocusPoint.position;
        focusPoint = initialFocus;
        projectionOffset = maxOffset;
        
        Vector3 origin = cam.transform.position;
        Vector3 dir = origin - initialFocus;
        cam.transform.forward = -dir;
        UpdateProjectionOffset(initialFocus);
    }

    public virtual void RestView()
    {
        cam.transform.position = camInitialPosition.position;
        cam.transform.rotation = camInitialPosition.rotation;
    }

    protected virtual void Update()
    {
        UpdateProjectionOffset(focusPoint);

        UpdateRotation();
        UpdatePosition(focusPoint, projectionOffset);
    }

    protected void UpdateRotation()
    {
      //  if (Utility.IsPointerOverUIObject()) return;

        if (Input.touchCount != 1) return;
        Touch touch = Input.GetTouch(Input.touchCount - 1);


        switch (touch.phase)
        {
            case TouchPhase.Began:
                isClickedOnUI = Utility.IsPointerOverUIObject();
                break;
            case TouchPhase.Ended:
                isClickedOnUI = false;
                break;
            case TouchPhase.Moved:

                if (isClickedOnUI) break;

                Vector2 currentPos = touch.position;
                Vector2 prePos = currentPos - touch.deltaPosition;
                Vector3 direction = prePos - currentPos;

                float rotationAroundYAxis = -direction.x * 180;
                float rotationAroundXAxis = direction.y * 180;

                cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis * Time.deltaTime * rotateSpeed);
                cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis * Time.deltaTime * rotateSpeed, Space.World);
                break;
        }
        /*
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 currentPos = touch.position;
            Vector2 prePos = currentPos - touch.deltaPosition;
            Vector3 direction = prePos - currentPos;

            float rotationAroundYAxis = -direction.x * 180;
            float rotationAroundXAxis = direction.y * 180;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis * Time.deltaTime * rotateSpeed);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis * Time.deltaTime * rotateSpeed, Space.World);


        }
        */
    }
 
    //update camera position to projection the focus point
    protected void UpdatePosition(Vector3 focusPoint, float projectionDistance)
    {
        //update position
        Vector3 frontFacinPoint = cam.transform.forward * projectionDistance;
        Vector3 projectionPoint = focusPoint - frontFacinPoint;
        Vector3 newPosition = projectionPoint + GetVerticalOffset();
        cam.transform.position = newPosition;


      //  cam.transform.position += GetVerticalOffset(); 
    }
    protected void UpdateProjectionOffset(Vector3 focus)
    {
        Vector3 focusWithOffset = focus + GetVerticalOffset();
        float offset = Vector3.Distance(cam.transform.position, focusWithOffset);
        projectionOffset = Mathf.Clamp(offset, minOffset, maxOffset);
    }

    protected void LearpProjectionOffset(float newOffset, float speed)
    { 
        projectionOffset = Mathf.Lerp(projectionOffset, newOffset, speed * Time.deltaTime);
    }

    protected Vector3 GetVerticalOffset()
    {
        Vector3 localdown = cam.transform.InverseTransformPoint(cam.transform.up);
        Vector3 worldDown = cam.transform.TransformPoint(localdown);
        return worldDown * verticalOffset;
    }
}
