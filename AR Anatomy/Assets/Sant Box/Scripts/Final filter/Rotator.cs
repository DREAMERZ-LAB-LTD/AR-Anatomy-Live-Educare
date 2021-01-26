using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform centerPivotObject;
    [SerializeField] private float rotateSpeed = 1.00f;
    private float offsetFromPivot = 5;
    private Vector3 previousMousePosition;


    private void Start()
    {
        Vector3 dir = cam.transform.position - centerPivotObject.transform.position;
        cam.transform.forward = -dir;
        UpdateOffset();
    }
    // Update is called once per frame
    void Update()
    {
        GetFocusPoint();
        RotateAround();
    }

    private void RotateAround()
    {
        if (IsPointerOverUIObject()) return;

        if (Input.touchCount != 1) return;


        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
             centerPivotObject.transform.position = GetFocusPoint();
             UpdateOffset();
             previousMousePosition = cam.ScreenToViewportPoint(touch.position);
        }
        else if (touch.phase == TouchPhase.Moved)
        {

            Vector3 newPosition = cam.ScreenToViewportPoint(touch.position);
            Vector3 direction = previousMousePosition - newPosition;
            previousMousePosition = newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

            cam.transform.position = centerPivotObject.position;
            cam.transform.Translate(new Vector3(0, 0, -offsetFromPivot ));

        }

    }

    private void UpdateOffset()=> offsetFromPivot = Vector3.Distance(cam.transform.position, centerPivotObject.position);


    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private Vector3 GetFocusPoint()
    {
        Vector3 worldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Physics.Raycast(cam.transform.position, worldPosition, out RaycastHit hit))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.green);
            return hit.point;
        }

        return Vector3.zero;
    }
}
