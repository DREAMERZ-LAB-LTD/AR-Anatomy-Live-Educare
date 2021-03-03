﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CircularPointer : MonoBehaviour
{
    [Header("Which Organ Follow this UI Dot")]
    [SerializeField] Transform anatomyOrgan;

    [Header("Circuler Dot UI Button")]
    [SerializeField] RectTransform circularPointer;
    [SerializeField] RectTransform circularPointerCanvus;

    [Header("Visible Range")]
    [SerializeField] private float minVisibleDistance = 0.5f;

    public void ResetDot()=> circularPointer.gameObject.SetActive(false);
    

    /// <summary>
    /// update circular pointer position W.R.To world space transform position
    /// </summary>
    /// <param name="camera">which camera is working to render world object</param>
    public void UpdatePosition(Camera camera)
    {
        bool isVisible = Vector3.Distance(camera.transform.position, anatomyOrgan.position) < minVisibleDistance;
        circularPointer.gameObject.SetActive(isVisible);
        if (!isVisible) return;


        circularPointer.transform.SetParent(circularPointerCanvus);
        Vector3 pointerPosition = camera.WorldToScreenPoint(anatomyOrgan.position);
        circularPointer.position = pointerPosition;
    } 
}

