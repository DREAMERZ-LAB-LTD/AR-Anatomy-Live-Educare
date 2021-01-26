using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnatomyInformationShower : MonoBehaviour
{
    [SerializeField] private DetailPanel display;
    [SerializeField] private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (IsPointerOverUIObject()) return;
        if (!Input.GetMouseButtonUp(0)) return;


        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200.0f))
        {
            string title = hit.transform.name;
            string info = "This is a " + title;
            display.SetMessage(title, info);
            display.PopUp(true);

#if UNITY_EDITOR
            Debug.DrawLine(cam.transform.position, hit.point, Color.green);
#endif
        }
        else 
        {
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



