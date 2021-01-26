﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float MoveSpeed;


    void Update()=> DragToMove();
    
    private void DragToMove()
    {
        if (IsPointerOverUIObject()) return;

        if (Input.touchCount != 2) return;


        Touch touch = Input.GetTouch(0);
        
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 currpos = touch.position;
            Vector2 nextpos = currpos + touch.deltaPosition;
            Vector3 direction = currpos - nextpos; 

            cam.transform.Translate(direction * Time.deltaTime * MoveSpeed);
        }
    }


    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

}
