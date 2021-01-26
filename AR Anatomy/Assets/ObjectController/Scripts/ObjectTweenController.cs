using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace DreamerzLab.Controller
{
    public class ObjectTweenController : MonoBehaviour
    {
        [SerializeField] private bool _enableEditorLogs = false;
        public Transform target;

        //Horizontal
        public bool useHorizontal = true;
        public float horizontalSpeed = 5;
        public float horizontalSmoothness = 0.25f;

        //scale
        public bool useScaleChange = true;
        public float scaleSpeed = 1;
        public float minSize = 1;
        public float maxSize = 3;
        public float minimumScaleSensitivity = 30;
        public float scaleSmoothness = 0.1f;
        float targetScale;

        //movement
        public bool useMovement;
        public LayerMask raycastLayer;
        public float moveSpeed = 100;
        public float minimumMovementSensitivity = 5; //TODO: this could be improved
        public float movementSmoothness = 0.25f;
        public float moveDelay;
        float nextDelay;

        //Tap
        public bool useTap;
        private float _firstTouchTime;
        private float _firstTapTime;
        public float singleTapMaxDuration = 0.15f;
        public float maxDelayBetweenSecondTap = 0.25f;
        public UnityEvent oneTapEvent;
        public UnityEvent twoTapEvent;
        public float onTapHoldDuration = 0.5f;
        public UnityEvent onTapHoldEvent;

        Touch _currentPrimaryTouch;
        Vector2 _currentTouchPosition;
        bool _wasOnMultiTouch = false;
        bool _inZoom = false;
        bool _callOnSingleTapEvent = false;




        private void Start()
        {
            //if (useVertical && !cameraTransform)
            //    cameraTransform = Camera.main.transform;
        }
        public bool IsPointerOverUIObject()
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

            if (Input.touchCount > 0)
            {
                _currentPrimaryTouch = Input.GetTouch(0);
                _currentTouchPosition = _currentPrimaryTouch.position;

                //touch started
                if (_currentPrimaryTouch.phase == TouchPhase.Began)
                    TouchStart();

                //touch going on
                else if (_currentPrimaryTouch.phase == TouchPhase.Moved)
                    TouchOnGoing();

                //touch ended
                else if (_currentPrimaryTouch.phase == TouchPhase.Ended)
                    TouchEnded();

                //Note: the other two phases are Stationary & Cancelled
            }

            //Calling single tap methods
            //this is to avoid single tap methods being called when double tapped.
            //basically a timer waits if it's a double tap, cancels or calls single tap events
            if (_callOnSingleTapEvent && _firstTapTime + maxDelayBetweenSecondTap < Time.time)
            {
#if UNITY_EDITOR
                if (_enableEditorLogs)
                    Debug.Log("First tap event invoked");
#endif
                _callOnSingleTapEvent = false;
                oneTapEvent.Invoke();
            }
        }


        private void TouchStart()
        {
            _firstTouchTime = Time.time;

            if (target) targetScale = target.localScale.x;
        }


        private void TouchOnGoing()
        {
            OnTapHold();

            if (!target)
                return;

            //single touch
            RotationOnGoing();

            if (Input.touchCount != 2)
                return;

            if (!_wasOnMultiTouch)
                _wasOnMultiTouch = true;


            MoveTarget();
            Scale();
        }



        private void TouchEnded()
        {
            _inZoom = false;
            nextDelay = moveDelay;
            _wasOnMultiTouch = false;  //this is not in touchphase ended because changing from two finger to one finger calls it
                                       //so, when zoom/move ends, the tiny time one finger is still touching screen,
                                       //the object rotates
            TapEnd();
        }




        private void RotationOnGoing()
        {
            if (useHorizontal && Input.touchCount < 2)
            {
                float rotY = _currentPrimaryTouch.deltaPosition.x * -horizontalSpeed * 3 * Mathf.Deg2Rad;
                target.DORotate(new Vector3(0, rotY, 0), 0.15f, RotateMode.WorldAxisAdd);

#if UNITY_EDITOR
                if (_enableEditorLogs)
                    Debug.Log("Horizontal Rotation");
#endif
            }
        }



        //TODO: Maybe both scale and move could use deltaposition
        private void Scale()
        {
            _wasOnMultiTouch = true;
            Touch secondaryTouch = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = _currentTouchPosition - _currentPrimaryTouch.deltaPosition;
            Vector2 touchOnePrevPos = secondaryTouch.position - secondaryTouch.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (_currentTouchPosition - secondaryTouch.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            if (Mathf.Abs(difference) > minimumScaleSensitivity)
            {
                _inZoom = true;

                targetScale = Mathf.Clamp(targetScale + difference * scaleSpeed * 0.001f, minSize, maxSize);
                target.DOScale(targetScale, scaleSmoothness);

#if UNITY_EDITOR
                if (_enableEditorLogs)
                    Debug.Log("Scaling");
#endif
            }
        }






        void MoveTarget()
        {
            if (_wasOnMultiTouch || _inZoom)
                return;

            if (nextDelay > 0)
            {
                nextDelay -= Time.deltaTime;
                return;
            }

            float xChange = _currentPrimaryTouch.deltaPosition.x;
            float yChange = _currentPrimaryTouch.deltaPosition.y;

            if (Mathf.Abs(xChange) < minimumMovementSensitivity || Mathf.Abs(yChange) < minimumMovementSensitivity)
                return;

#if UNITY_EDITOR
            if (_enableEditorLogs)
                Debug.Log("Movement called");
#endif                   
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_currentTouchPosition);

            if (Physics.Raycast(ray, out hit, 500, raycastLayer))
            {
                target.DOMove(hit.point, movementSmoothness);
            }
        }


        private void OnTapHold()
        {
            if (Time.time > onTapHoldDuration + _firstTapTime)
            {
                onTapHoldEvent.Invoke();
#if UNITY_EDITOR
                if (_enableEditorLogs)
                    Debug.Log("On Tap Hold event");
#endif
            }
        }



        private void TapEnd()
        {
            if (!useTap)
                return;

            //Tap completed
            if (_firstTouchTime + singleTapMaxDuration > Time.time)
            {
                //is this second tap
                if (_firstTapTime + maxDelayBetweenSecondTap > Time.time)
                {
#if UNITY_EDITOR
                    if (_enableEditorLogs)
                        Debug.Log("Double tap event invoked");
#endif
                    _callOnSingleTapEvent = false;
                    twoTapEvent?.Invoke();
                }
                //this is first tap
                else
                {
#if UNITY_EDITOR
                    if (_enableEditorLogs)
                        Debug.Log("First tap");
#endif
                    _callOnSingleTapEvent = true;
                    //oneTapEvent?.Invoke();
                    _firstTapTime = Time.time;
                }
            }
        }
    }
}