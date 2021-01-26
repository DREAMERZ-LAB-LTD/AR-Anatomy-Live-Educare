using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ObjectTweenController2 : MonoBehaviour
{
    /// <summary>
    /// Setting most to public instead of serializefield because don't know where this script might get used
    /// 
    /// 
    /// </summary>


    [SerializeField] private bool _enableEditorLogs = false;
    public Transform target;

    //Horizontal
    public bool useHorizontal = true;
    public float horizontalSpeed = 5;
    public float horizontalSmoothness = 0.25f;

    //rename to sensitivity
    public float minimumVerticalMovement = 0.6f; //if finger isn't moved too much, don't rotate. This is to avoid accidental vertical rotation when moving horizontal

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
    Vector3 targetPos;

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


    //for the public methods reset transform or Y
    public float resetRotationSmoothness = 0.5f;


    Touch _currentPrimaryTouch;
    Vector2 _currentTouchPosition;
    bool _wasOnMultiTouch = false;
    bool _inZoom = false;
    bool _callOnSingleTapEvent = false;

    Camera myCamera;
    [SerializeField] LayerMask touchLayer;

    private void Start()
    {
        myCamera = Camera.main;
    }

    private void Update()
    {
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
        else _wasOnMultiTouch = false;  //this is not in touchphase ended because changing from two finger to one finger calls it
                                        //so, when zoom/move ends, the tiny time one finger is still touching screen,
                                        //the object rotates

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
       
        }
    }






    /// <summary>
    /// 
    /// </summary>
    private void TouchStart()
    {
        _firstTouchTime = Time.time;

        targetPos = target.position;
        targetScale = target.localScale.x;
    }







    /// <summary>
    /// 
    /// </summary>
    private void TouchOnGoing()
    {
        OnTapHold();
        //single touch
        RotationOnGoing();

        if (Input.touchCount != 2)
            return;

        if (!_wasOnMultiTouch)
            _wasOnMultiTouch = true;


        Scale();
        MoveTarget();
    }



    /// <summary>
    /// 
    /// </summary>
    private void TouchEnded()
    {
        _inZoom = false;

   
    }





    /// <summary>
    /// 
    /// </summary>
    private void RotationOnGoing()
    {
        if (Input.touchCount > 1 || _wasOnMultiTouch)
            return;


        if (useHorizontal)
        {
            float rotY = _currentPrimaryTouch.deltaPosition.x * -horizontalSpeed * 3 * Mathf.Deg2Rad;
            //target.Rotate(0, rotY, 0);
            target.DORotate(new Vector3(0, rotY, 0), 0.15f, RotateMode.LocalAxisAdd);

#if UNITY_EDITOR
            if (_enableEditorLogs)
                Debug.Log("Horizontal Rotation");
#endif
        }
    }



    /// <summary>
    /// 
    /// </summary>
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






    /// <summary>
    /// 
    /// </summary>
    private void MoveTarget()
    {
        if (_inZoom)
            return;

        float xChange = _currentPrimaryTouch.deltaPosition.x;
        float yChange = _currentPrimaryTouch.deltaPosition.y;

        if (Mathf.Abs(xChange) < minimumMovementSensitivity || Mathf.Abs(yChange) < minimumMovementSensitivity)
            return;

#if UNITY_EDITOR
        if (_enableEditorLogs)
            Debug.Log("Movement called");
#endif

        targetPos =
            new Vector3(
                targetPos.x + xChange * moveSpeed * 0.01f * Time.deltaTime,
                targetPos.y,
                targetPos.z + yChange * moveSpeed * 0.01f * Time.deltaTime
                );

        target.DOMove(targetPos, movementSmoothness);
    }



    /// <summary>
    /// 
    /// </summary>
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
}
