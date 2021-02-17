using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotator : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    [SerializeField] private Transform InitialPoint;
    [SerializeField] protected Transform centerPivotObject;
    [SerializeField] protected float minZoom = 0.5f;
    [SerializeField] protected float maxZoom = 1f;

    [Range(0.01f, 1.00f)]
    [SerializeField] private float rotateSpeed = 0.05f;
    protected float offsetFromFocus = 5;




    protected virtual void Start()
    {
        Vector3 origin = cam.transform.position;
        Vector3 focusPoint = centerPivotObject.position;
        Vector3 dir = origin - focusPoint;
        cam.transform.forward = -dir;
        UpdateOffset(focusPoint);
        RestView();
    }


    public void RestView()
    {
        cam.transform.position = InitialPoint.position;
        cam.transform.rotation = InitialPoint.rotation;
    }


    protected virtual void Update()
    {
        Vector3 focusPoint = centerPivotObject.position;
        UpdateOffset(focusPoint);
        RotateAround(focusPoint, offsetFromFocus);
    }

   
    protected void RotateAround(Vector3 focusPoint, float offset)
    {
        if (IsPointerOverUIObject()) return;
        
        if (Input.touchCount != 1) return;
        Touch touch = Input.GetTouch(Input.touchCount- 1);
        
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 currentPos = touch.position;
            Vector2 prePos = currentPos - touch.deltaPosition;
            Vector3 direction = prePos - currentPos;

            float rotationAroundYAxis = -direction.x * 180; 
            float rotationAroundXAxis = direction.y * 180; 

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis * Time.deltaTime * rotateSpeed);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis * Time.deltaTime * rotateSpeed, Space.World);

            cam.transform.position = focusPoint;
            cam.transform.Translate(new Vector3(0, 0, -offset));
        }
    }

    protected void UpdateOffset(Vector3 focus)
    { 
        float offset = Vector3.Distance(cam.transform.position, focus);
        offsetFromFocus = Mathf.Clamp(offset, minZoom, maxZoom);
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
