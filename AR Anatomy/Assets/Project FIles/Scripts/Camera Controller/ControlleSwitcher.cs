using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ControlleSwitcher : MonoBehaviour
{
    [SerializeField] float selectionDelay;
    [SerializeField] float minTouchFraction;
    [SerializeField] DynamicFocusRotator rotator;
    [SerializeField] Movement movement;

    private Coroutine existingCounter;
    private bool dragOnCounteing = false;

    private void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(Input.touchCount - 1);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                dragOnCounteing = false;
                StopCounter();
                existingCounter = StartCoroutine(counter());
                break;
            case TouchPhase.Moved:
                if(touch.deltaPosition.magnitude > minTouchFraction)
                    dragOnCounteing = true;
                break;
            case TouchPhase.Ended:
                dragOnCounteing = false;
                StopCounter();
                break;
                
        }

    }

    private void StopCounter()
    {
        if (existingCounter != null)
        {
            StopCoroutine(existingCounter);
        }
    }
    private IEnumerator counter()
    {
        rotator.enabled = true;
        movement.enabled = false;
        yield return new WaitForSeconds(selectionDelay);
        if (!dragOnCounteing)
        { 
            rotator.enabled = false;
            movement.enabled = true;
        }
    }
}
