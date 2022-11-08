using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExploreModeHandler : MonoBehaviour
{
#pragma warning disable 649
    [Header("Execute When Clicked On Circular Dot")]
    [SerializeField] UnityEvent onExploringStart;
    [Header("Execute When Back To 3D Anatomy View")]
    [SerializeField] UnityEvent onExploringClose;
#pragma warning restore 649


    public void OnClickCircularDot()=>onExploringStart.Invoke();
    public void OnClickBackTo3DAnatomy() => onExploringClose.Invoke();
}
