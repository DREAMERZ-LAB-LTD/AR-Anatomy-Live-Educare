using UnityEngine;


namespace Anatomy
{
    public class ObjectManipulator : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Transform centerObject;

        [SerializeField] private bool useRotation = true;
        private float offsetFromTarget = 5;
        private Vector3 previousMousePosition;

        [SerializeField] private bool useMovement = true;
        private Vector3 PreviousHitPoint;
        private bool isMoveing = false;

        [SerializeField] private bool useZoom = true;
        [SerializeField] private float ZoomSpeed = 40.00f;
        [SerializeField,Range(1.00f, 3.00f)] private float MinZoom = 1.00f;
        [SerializeField,Range(3.00f, 10.00f)] private float MaxZoom = 3.00f;


        private void Awake()
        {
            UpdateOffset();
        }


        private void Update()
        {
            DragToMove();
            RotateAround();
            OnZooming();

        }


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

                Vector3 offset = cam.transform.localPosition - centerObject.localPosition;
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

                float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                cam.transform.position = centerObject.position;

                cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

                cam.transform.Translate(new Vector3(0, 0, -offsetFromTarget));

                previousMousePosition = newPosition;
            }
  
        }
        private void UpdateOffset() =>  offsetFromTarget = Vector3.Distance(cam.transform.position, centerObject.position);

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
    }
}