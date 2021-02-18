using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotator : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    [SerializeField] private Transform InitialPoint;
    [SerializeField] protected Transform centerPivotObject;
    [SerializeField] protected float minZoom = 0.05f;
    [SerializeField] protected float maxZoom = 2.5f;

    [Range(0.01f, 1.00f)]
    [SerializeField] private float rotateSpeed = 0.05f;
    protected Vector3 focusPoint;
    protected float projectionOffset = 3;

    protected virtual void Start()
    {
        Vector3 initialFocus = centerPivotObject.position;
        focusPoint = initialFocus;
        projectionOffset = maxZoom;
        
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
        Vector3 frontFacinPoint = cam.transform.forward * projectionDistance;
        Vector3 projectionPoint = focusPoint - frontFacinPoint;
        cam.transform.position = projectionPoint;
    }
    protected void UpdateProjectionOffset(Vector3 focus)
    { 
        float offset = Vector3.Distance(cam.transform.position, focus);
        projectionOffset = Mathf.Clamp(offset, minZoom, maxZoom);
    }

    protected void LearpProjection(float newOffset, float speed)
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
