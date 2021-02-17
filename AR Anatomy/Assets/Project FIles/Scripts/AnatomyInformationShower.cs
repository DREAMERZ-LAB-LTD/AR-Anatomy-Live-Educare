using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnatomyInformationShower : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private DetailPanel display;
    [Header("Raycast Layer Mask")]
    [SerializeField] private int targetLayer = 0;
    [Space]
    [SerializeField] private Material bodySkinMat;
    private GameObject selectedPart;
    private void Start()
    {
        cam = Camera.main;
        bodySkinMat.color = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
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

        if (IsPointerOverUIObject()) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        int layer = 1 << targetLayer;
        if (Physics.Raycast(ray, out hit, 500.0f, layer))
        {
            if (selectedPart != null)
            {
                selectedPart.GetComponent<Renderer>().material.color = Color.white;
                bodySkinMat.color = Color.white;
            }
            selectedPart = hit.transform.gameObject;
            selectedPart.GetComponent<Renderer>().material.color = Color.green;
            if (selectedPart.name == "Body_Skin")
            {
                bodySkinMat.color = Color.green;
            }

            string title = hit.transform.name;
            string info = "This is a " + title;
            display.SetMessage(title, info);
            display.PopUp(true);

            //if (selectedPart.tag == "Interactable")
            //{
            //    detailedButton.SetActive(true);
            //}
            //else
            //{
            //    detailedButton.SetActive(false);
            //}
        }
        else
        {
            if (selectedPart != null)
            {
                selectedPart.GetComponent<Renderer>().material.color = Color.white;
                bodySkinMat.color = Color.white;

                selectedPart = null;
            }
            //                    detailPanel.SetActive(false);
            display.PopUp(false);
        }

    }
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}



