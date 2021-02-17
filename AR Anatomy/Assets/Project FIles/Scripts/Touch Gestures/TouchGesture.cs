using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchGesture : MonoBehaviour
{
    [SerializeField] GestureEvents singleTouchEvents;
    private Coroutine singleTouchProcessor;


    [SerializeField] GestureEvents doubleTouchEvents;
    private Coroutine doubleClickProcessor = null;

    [SerializeField] private float doubleClickDelay = 0.8f;



    private void Update()
    {
        HandleResponseTouch(singleTouchEvents);
        HandleResponseTouch(doubleTouchEvents);
    }
    private void HandleResponseTouch(GestureEvents touchResponseEvents)
    {
        int touchCount = touchResponseEvents.TargetTouchCount;
        if (Input.touchCount != touchCount) return;

        Touch touch = Input.GetTouch(touchCount - 1);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchResponseEvents.onTouchBegin.Invoke();
                HandleDoubleClickResponse(touchResponseEvents);
                break;

            case TouchPhase.Stationary:
                touchResponseEvents.onTouchStationary.Invoke();
                break;

            case TouchPhase.Moved:
                touchResponseEvents.onTouchMoved.Invoke();
                break;

            case TouchPhase.Canceled:
                touchResponseEvents.onTouchCanceled.Invoke();
                break;
            case TouchPhase.Ended:
                touchResponseEvents.onTouchEnded.Invoke();
                break;
        }
    }

    private void HandleDoubleClickResponse(GestureEvents touchResponseEvents)
    {
        Debug.Log("processor : "+doubleClickProcessor != null);
        if (doubleClickProcessor != null)
        {
            touchResponseEvents.onDoubleClick.Invoke();
            StopCoroutine(doubleClickProcessor);
            doubleClickProcessor = null;
            return;
        }

       doubleClickProcessor = StartCoroutine(Processor(doubleClickDelay));
    }

    IEnumerator Processor(float clickDelay)
    {
        yield return new WaitForSeconds(clickDelay);
        doubleClickProcessor = null;
    }

}

