using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zooming : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Transform cam;
    [SerializeField] private float minimumScaleSensitivity;
    [SerializeField] private float ZoomSpeed;
#pragma warning restore 649
    private void Update()
    {
        OnZooming();
    }
    private void OnZooming()
    {

        if (Utility.IsPointerOverUIObject()) return;
        if (Input.touchCount < 2) return;


        float deltaFraction = 0.00f;
        Vector3 zoomDir = cam.forward;


        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touch1.position - touch1.deltaPosition;
        Vector2 touchOnePrevPos = touch2.position - touch2.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touch1.position - touch2.position).magnitude;

        deltaFraction = currentMagnitude - prevMagnitude;

        if (TouchPhase.Moved == touch1.phase || TouchPhase.Moved == touch2.phase)
        {
            if (Mathf.Abs(deltaFraction) > minimumScaleSensitivity)
            {

                Vector3 deltaZoom = zoomDir.normalized * deltaFraction * ZoomSpeed * Time.deltaTime;

                cam.localPosition += deltaZoom;
            }
        }
    }


}
