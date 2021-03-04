using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPointerController : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [Header("Circular Pointer Colections")]
    [SerializeField] private List<CircularPointer> pointers = new List<CircularPointer>();

    private void Awake()
    {
        if (mainCamera == null)
        { 
            mainCamera = Camera.main; 
        }
    }

    public void ResetDots()
    {
        for (int i = 0; i < pointers.Count; i++)
        {
            pointers[i].ResetDot();
        }
    }

    void Update()
    {
        for (int i = 0; i < pointers.Count; i++)
        {
            pointers[i].UpdatePosition(mainCamera);
        }
    }
}
