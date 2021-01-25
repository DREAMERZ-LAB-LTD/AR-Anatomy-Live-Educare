using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Anatomy
{
    public class ObjectManipulator : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Transform centerPivotObject;

        [SerializeField] private bool useRotation = true;
        private float offsetFromPivot = 5;
        private Vector3 previousMousePosition;

        [SerializeField] private bool useMovement = true;
        private Vector3 PreviousHitPoint;
        private bool isMoveing = false;

        [SerializeField] private bool useZoom = true;
        [SerializeField] private float ZoomSpeed = 40.00f;
        [SerializeField,Range(1.00f, 3.00f)] private float MinZoom = 1.00f;
        [SerializeField,Range(3.00f, 10.00f)] private float MaxZoom = 3.00f;

        public float dot = 0;
        private void Awake()=>UpdateOffset();
        

        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void Update()
        {
            if (IsPointerOverUIObject()) return;

            DragToMove();
            RotateAround();
            OnZooming();

        }
#region Zoom
        private void OnZooming()
        {
            if (!useZoom) return;

            float deltaFraction = 0.00f;

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                previousMousePosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = previousMousePosition - newPosition;
                previousMousePosition = cam.ScreenToViewportPoint(Input.mousePosition);
                deltaFraction = direction.y;

                Vector3 offset = cam.transform.localPosition - centerPivotObject.localPosition;
                Vector3 deltaZoom = offset.normalized * deltaFraction * ZoomSpeed * Time.deltaTime;

                cam.transform.localPosition -= deltaZoom;
                UpdateOffset();
            }

#else
            if (Input.touchCount < 2) return;
            
                Touch touch1 = new Touch();
                Touch touch2 = new Touch();

                touch1 = Input.GetTouch(Input.touchCount - 2);
                touch2 = Input.GetTouch(Input.touchCount - 1);

                if (TouchPhase.Began == touch2.phase)
                { 
                
                }
            
#endif


        }
#endregion Zoom

#region Movement
        private void DragToMove()
        {
            if (!useMovement) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (GetRaycasthiInfo(out Vector3 pos, out Transform hitObject))
                {
                    PreviousHitPoint = pos;
                    isMoveing = true;
                }
                else
                {
                    isMoveing = false;
                }
            }
            else if (Input.GetMouseButton(0) && isMoveing)
            {
                if (GetRaycasthiInfo(out Vector3 pos, out Transform hitObject))
                {
                    Vector3 newPOs = pos - PreviousHitPoint;
                    hitObject.position += newPOs;
                    PreviousHitPoint = pos;
                    UpdateOffset();
                }
            }
        }

        private bool GetRaycasthiInfo(out Vector3 hitPoint, out Transform hitObject)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.DrawLine(cam.transform.position, hit.point, Color.green);
                hitObject = hit.collider.transform;
                hitPoint = hit.point;
                return true;
            }

            hitPoint = new Vector3();
            hitObject = null;
            return false;
        }
        #endregion Movement


#region Rotation
        private void RotateAround()
        {

            if (!useRotation) return;


            if (Input.GetMouseButtonDown(0))
            {
              previousMousePosition = cam.ScreenToViewportPoint(Input.mousePosition);  
            }
            else if (Input.GetMouseButton(0) && !isMoveing)
            {

                Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = previousMousePosition - newPosition;
                previousMousePosition = newPosition;

                float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                if(rotationAroundXAxis != 0)
                    Debug.Log("dir = "+ rotationAroundXAxis);

                if (dot < 0)
                {
                    if (rotationAroundYAxis > 0)
                    { 
                        cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                    }
                }
                else
                { 
                    cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                }
                cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);
             
                Vector3 pivotPoint = centerPivotObject.position;
                Vector3 campos = cam.transform.position;
                dot = Vector3.Dot(pivotPoint, campos);


                cam.transform.position = centerPivotObject.position;
                cam.transform.Translate(new Vector3(0, 0, -offsetFromPivot));

            }
  
        }
        private void UpdateOffset() =>  offsetFromPivot = Vector3.Distance(cam.transform.position, centerPivotObject.position);
#endregion Rotation
    }
}