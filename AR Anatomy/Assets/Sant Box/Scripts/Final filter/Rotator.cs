using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform centerPivotObject;
    [SerializeField] private float rotateSpeed = 1.00f;
    public float cameraFollowSpeed = 45.00f;
    private float offsetFromPivot = 5;
    private Vector3 previousMousePosition;


    private Vector3 InitialCamPos;
    private Quaternion InitialCamRot;
    private Vector3 InitialFocusPoint;

    private void Start()
    {
        Vector3 dir = cam.transform.position - centerPivotObject.transform.position;
        cam.transform.forward = -dir;
        UpdateOffset();

        InitialCamPos = cam.transform.position;
        InitialCamRot = cam.transform.rotation;
        InitialFocusPoint = centerPivotObject.position;
    }


    public void RestView()
    {
        cam.transform.position = InitialCamPos;
        cam.transform.rotation = InitialCamRot;
        centerPivotObject.position = InitialFocusPoint;
    }


    // Update is called once per frame
    void Update()
    {
        RotateAround();
    }

    private void RotateAround()
    {
        if (IsPointerOverUIObject()) return;

        if (Input.touchCount != 1) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            //Vector3 focusPoint = GetFocusPoint();
            //if (focusPoint != Vector3.zero)
            //{
            //    centerPivotObject.transform.position = focusPoint;
            //}
            
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


            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis * Time.deltaTime * rotateSpeed);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis * Time.deltaTime * rotateSpeed, Space.World);

            //Vector3 focus = centerPivotObject.transform.position;
            //Vector3 newPos = focus + -cam.transform.forward * offsetFromPivot;

            //cam.transform.position = Vector3.Lerp(cam.transform.position, newPos, Time.deltaTime * cameraFollowSpeed);


            cam.transform.position = centerPivotObject.position;
            cam.transform.Translate(new Vector3(0, 0, -offsetFromPivot));
        }
    }


    private void UpdateOffset()=> offsetFromPivot = Vector3.Distance(cam.transform.position, centerPivotObject.position);


    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private Vector3 GetFocusPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
  
        if (Physics.Raycast(ray, out RaycastHit hit, 200.0f))
        {
#if UNITY_EDITOR
            Debug.DrawLine(cam.transform.position, hit.point, Color.green);
#endif
            return hit.point;
        }

        return Vector3.zero;
    }
}
