using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotator : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    [Header("Rotation Setup")]
    [SerializeField] Transform InitialPoint;
    [SerializeField] protected Transform centerPivotObject;
    [Range(0.01f, 1.00f)]
    [SerializeField] float rotateSpeed = 0.05f;


    [Header("Focus Setup")]
    [Tooltip("Vertical Projection Offset")]
    [SerializeField, Range(-1.00f, 1.00f)] float  verticalOffset = 0;
    [Tooltip("Total camera offset from focus point")]
    protected float projectionOffset = 3;
    [Tooltip("Minimum Projection Offset")]
    [SerializeField] protected float minOffset = 0.05f;
    [Tooltip("Maximum Projection Offset")]
    [SerializeField] protected float maxOffset = 2.5f;
    protected Vector3 focusPoint;

    protected virtual void Start()
    {
        Vector3 initialFocus = centerPivotObject.position;
        focusPoint = initialFocus;
        projectionOffset = maxOffset;
        
        Vector3 origin = cam.transform.position;
        Vector3 dir = origin - initialFocus;
        cam.transform.forward = -dir;
        UpdateProjectionOffset(initialFocus);
        RestView();
    }

    public void RestView()
    {
        cam.transform.position = InitialPoint.position;
        cam.transform.rotation = InitialPoint.rotation;
    }

    protected virtual void Update()
    {
        UpdateProjectionOffset(focusPoint);

        UpdateRotation();
        UpdatePosition(focusPoint, projectionOffset);
    }

    protected void UpdateRotation()
    {
        if (IsPointerOverUIObject()) return;

        if (Input.touchCount != 1) return;
        Touch touch = Input.GetTouch(Input.touchCount - 1);

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
    }
 
    //update camera position to projection the focus point
    protected void UpdatePosition(Vector3 focusPoint, float projectionDistance)
    {
        //update position
        Vector3 frontFacinPoint = cam.transform.forward * projectionDistance;
        Vector3 projectionPoint = focusPoint - frontFacinPoint;
        cam.transform.position = projectionPoint;

        //set verical offset
        Vector3 localdown = cam.transform.InverseTransformPoint(cam.transform.up);
        Vector3 worldDown = cam.transform.TransformPoint(localdown);
        Vector3 vertical_offset = worldDown * verticalOffset;
        cam.transform.position += vertical_offset; 
    }
    protected void UpdateProjectionOffset(Vector3 focus)
    { 
        float offset = Vector3.Distance(cam.transform.position, focus);
        projectionOffset = Mathf.Clamp(offset, minOffset, maxOffset);
    }

    protected void LearpProjectionOffset(float newOffset, float speed)
    { 
        projectionOffset = Mathf.Lerp(projectionOffset, newOffset, speed * Time.deltaTime);
    }
 

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

  
}
