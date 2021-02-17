using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GestureEvents
{
    public int TargetTouchCount = 1;
    public UnityEvent onTouchBegin;
    public UnityEvent onTouchStationary;
    public UnityEvent onTouchMoved;
    public UnityEvent onTouchCanceled;
    public UnityEvent onTouchEnded;
    [Header("Double Click")]
    public UnityEvent onDoubleClick;
}
