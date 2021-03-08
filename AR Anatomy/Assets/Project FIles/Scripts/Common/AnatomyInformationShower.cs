using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnatomyInformationShower : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Camera cam;
    [SerializeField] private DetailPanel display;
    [Header("Raycast Layer Mask")]
    [SerializeField] private int targetLayer = 0;
    private GameObject preSelectedObject;
#pragma warning restore 649

#pragma warning disable 414
    [Header("Touch Input Setup")]
    [SerializeField] float touchOffset = 10.00f;
#pragma warning restore 414
#if !UNITY_EDITOR
    private Vector2 clickDownPoint;
#endif

    private void Start()
    {
        preSelectedObject = null;
        if (cam == null)
        { 
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            SelectBodyParts(Input.mousePosition);
#else
        Touch touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                clickDownPoint = touch.position;
                break;

            case TouchPhase.Ended:
                Vector2 clickUpPoint = touch.position;
                Vector2 offset = clickDownPoint - clickUpPoint;
                if (offset.magnitude < touchOffset)//Valid selection input
                {
                    SelectBodyParts(clickUpPoint);
                }
                else//Click cancel and hide information popup
                {
                    ApplyColor(preSelectedObject, Color.white);
                    ShowInfo(null);
                }
                break;
        }
#endif

    }
    private void SelectBodyParts(Vector2 screenPoint)
    {
        GameObject newSelectedObject = GetRaycastInfo(screenPoint, targetLayer);;


        if (newSelectedObject != null)
        {
            ApplyColor(preSelectedObject, Color.white);
            ApplyColor(newSelectedObject, Color.green);
            ShowInfo(newSelectedObject);
            //assign new selected object to previous selected object for next click
            preSelectedObject = newSelectedObject;
        }
        else
        {
            DeselectOrgan();
        }

    }

    public void DeselectOrgan()
    {
        ApplyColor(preSelectedObject, Color.white);
        ShowInfo(null);
    }

    private void ApplyColor(GameObject newSelectedObject, Color color)
    {
        //set color of new selected object
        if (newSelectedObject != null)
        {
            Material[] newSelectedMats = newSelectedObject.GetComponent<Renderer>().materials;
            for (int i = 0; i < newSelectedMats.Length; i++)
            {
                newSelectedMats[i].color = color;
            }
        }
    }

    //pass selected organ information to show on the screen
    private void ShowInfo(GameObject selectedObject)
    {
        bool hasSelectedObject = selectedObject != null;
        if (hasSelectedObject)
        {
            string title = selectedObject.name;
            string info = "This is a " + title;
            display.SetMessage(title, info);
        }
        display.PopUp(hasSelectedObject);

    }

    private GameObject GetRaycastInfo(Vector2 screenPoint, int layermask)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        int layer = 1 << layermask;
        if (Physics.Raycast(ray, out RaycastHit hit, 500.0f, layer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
  
}



