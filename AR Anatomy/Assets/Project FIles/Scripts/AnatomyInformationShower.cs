using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnatomyInformationShower : MonoBehaviour
{
    [SerializeField] private DetailPanel display;
    [SerializeField] private Camera cam;
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

        if (IsPointerOverUIObject()) return;
        if (!Input.GetMouseButtonUp(0)) return;

        SelectBodyParts();

    }
    private void SelectBodyParts()
    {

        if (IsPointerOverUIObject()) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
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



