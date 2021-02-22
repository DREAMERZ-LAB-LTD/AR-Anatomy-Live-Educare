using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float MoveSpeed;

    [Header("Input Touch Count")]
    [SerializeField] private int minTouchCount = 1;


    void Update()
    {
        DragToMove();
    }
    
    private void DragToMove()
    {
        if (Utility.IsPointerOverUIObject()) return;

        if (Input.touchCount != minTouchCount) return;


        Touch touch = Input.GetTouch(Input.touchCount - 1);
        
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 currpos = touch.position;
            Vector2 nextpos = currpos + touch.deltaPosition;
            Vector3 direction = currpos - nextpos; 

            cam.transform.Translate(direction * Time.deltaTime * MoveSpeed);
        }
    }
}
