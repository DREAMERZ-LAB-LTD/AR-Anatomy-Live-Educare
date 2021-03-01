using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Debug = UnityEngine.Debug;

public class ARPlacement : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] ARPlaneManager planeManager;


    [SerializeField, Header("Scaner Animation Gameobject")] 
    private GameObject PlaneScanerAnim;
    [SerializeField, Header("3D Anatomy Transparent Avater")] 
    private GameObject placementIndicator;
    [SerializeField, Header("3D Anatomy Avater")] 
    private GameObject AnatomyAvater;

    [SerializeField, Header("Selection Delay Time"), Range(0.25f, 2.00f)] 
    private float MaxSelectionDelay;

#pragma warning restore 649

    private float SelectionCounter = 0;
    private bool PlacedOnAR = false;
    private bool TapOnUI = false;


    private void Start()
    {
        placementIndicator.SetActive(false);
        AnatomyAvater.SetActive(false);
    }

    void Update()
    {
        bool detectedPlane = planeManager.trackables.count > 0;
        PlaneScanerAnim.SetActive(!detectedPlane);

        if (!PlacedOnAR)
        {
            if (detectedPlane)
            {
                placementIndicator.SetActive(true);
                PlacedOnAR = Input.GetMouseButtonDown(0);

                if (PlacedOnAR)
                {
                    MeshRenderer[] rends = placementIndicator.GetComponentsInChildren<MeshRenderer>();
                    foreach (var r in rends)
                    {
                        r.enabled = false;
                    }
                    placementIndicator.SetActive(false);
                    AnatomyAvater.SetActive(true);
                }
            }

            SetARPlacementPosition(new Vector3(Screen.width / 2, Screen.height / 4));

        } else
        {
            SetARPlacementPosition(Input.mousePosition);
            ObjectSelection();
        }

        UpdateAnatomyPosiotion();

    }

    private void ObjectSelection()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
           
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    TapOnUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                    break;

                case TouchPhase.Stationary:
                    if (TapOnUI) return;

                    if(isAvaterDetected)
                        SelectionCounter += Time.deltaTime;
                    placementIndicator.SetActive(SelectionCounter > MaxSelectionDelay);//object selected and ready to move position
                    break;

                case TouchPhase.Ended:
                    SelectionCounter = 0;
                    placementIndicator.SetActive(false);//object deselected and stop moveing
                    break;
            }
        
        }
   
    }


    //set AR Placement Intdicator on ar plane to raycast point
    private void SetARPlacementPosition(Vector3 ScreenPoint)
    {
        if (!placementIndicator.activeSelf) return;

        List<ARRaycastHit> hits = GetARRaycastHit(ScreenPoint);
        if (hits != null)
        {
            var hitPose = hits[0].pose;
            Quaternion rot = VerticalRotationHandler.LookTo(placementIndicator.transform, Camera.main.transform);
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, rot);
        }
    }


    //update avater position and orientation With Respect To placementIndicator
    private void UpdateAnatomyPosiotion()
    {
        if (!placementIndicator.activeSelf) return;

        float angle = placementIndicator.transform.rotation.eulerAngles.y;
        Vector3 rot = AnatomyAvater.transform.eulerAngles;
        //When object will moveing y axis rotation for faceing to camera direction
        AnatomyAvater.transform.rotation = Quaternion.Euler(new Vector3(rot.x, angle, rot.z));

        AnatomyAvater.transform.DOMove(placementIndicator.transform.position, 0.25f);
    }


    //return AR raycast information
    private List<ARRaycastHit> GetARRaycastHit(Vector3 screenPoint, TrackableType trackingMode = TrackableType.Planes)
    {
        List<ARRaycastHit> hitpoints = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenPoint, hitpoints, trackingMode))
        {
            return hitpoints;
        }
        return null;
    }

    //return true if racast detect any collider
    //in this case we have just avate thats why no need to layer masking
    private bool isAvaterDetected
    {
        get 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit, 1000.0f); 
        }
    }
}
