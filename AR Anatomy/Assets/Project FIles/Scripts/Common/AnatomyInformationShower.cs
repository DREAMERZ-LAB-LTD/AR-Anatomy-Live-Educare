using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnatomyInformationShower : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private DetailPanel display;
    [Header("Raycast Layer Mask")]
    [SerializeField] private int targetLayer = 0;
    [Space]
    [SerializeField] private Material bodySkinMat;
    private GameObject preSelectedObject;

    private void Start()
    {
        if (cam == null)
        { 
            cam = Camera.main;
        }
        bodySkinMat.color = Color.white;
        preSelectedObject = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (Utility.IsPointerOverUIObject()) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            SelectBodyParts(Input.mousePosition);
# else

        if (Input.touchCount == 1)
        {
            bool OnMoveing = false;
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    break;
                case TouchPhase.Stationary:
                    OnMoveing = true;
                    break;
                case TouchPhase.Moved:
                    OnMoveing = true;
                    break;
                case TouchPhase.Ended:
                    if (!OnMoveing)
                    {
                        Touch touch = Input.GetTouch(Input.touchCount - 1);
                        SelectBodyParts(touch.position);
                    }
                    break;
            }
        }
#endif 

    }
    private void SelectBodyParts(Vector2 screenPoint)
    {
        GameObject newSelectedObject = GetRaycastInfo(screenPoint, targetLayer);;
        Debug.Log(newSelectedObject == null);

        if (newSelectedObject != null)
        {
            SetSelectedColor(newSelectedObject, Color.green);
            ShowInfo(newSelectedObject);
        }
        else
        {
            SetSelectedColor(null, Color.green);
            ShowInfo(null);
        }

    }

    private void SetSelectedColor(GameObject newSelectedObject, Color color)
    {
        //make default color of previous selected object when try to select another object
        if (preSelectedObject != null)
        {
            preSelectedObject.GetComponent<Renderer>().material.color = Color.white;
            bodySkinMat.color = Color.white;
        }

        //set color of new selected object
        if (newSelectedObject != null)
        {
            newSelectedObject.GetComponent<Renderer>().material.color = color;
            if (newSelectedObject.name == "Body_Skin")
            {
                bodySkinMat.color = color;
            }
        }

        //assign new selected object to previous selected object for next click
        preSelectedObject = newSelectedObject;
    }

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



