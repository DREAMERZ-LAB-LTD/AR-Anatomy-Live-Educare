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


  //  [SerializeField] ARSessionOrigin sessionOrigin;
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] ARPlaneManager planeManager;


    [SerializeField, Header("Scaner Animation Gameobject")] 
    private GameObject PlaneScanerAnim;
    [SerializeField, Header("3D Anatomy Transparent Avater")] 
    private GameObject placementIndicator;
    [SerializeField, Header("3D Anatomy Avater")] 
    private GameObject AnatomyAvater;

    [SerializeField, Header("Selection Delay Time"), Range(0.25f, 2.00f)] private float MaxSelectionDelay;
    private float SelectionCounter = 0;
    private bool PlacedOnAR = false;

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
                    placementIndicator.SetActive(false);
                    AnatomyAvater.SetActive(true);
                }
            }
         
            ARPlacementIndicator(new Vector3(Screen.width / 2, Screen.height / 4));

        } else
        {
            ARPlacementIndicator(Input.mousePosition);
            ObjectSelection();
        }

        UpdateAnatomyPosiotion();

    }


    private void ObjectSelection()
    {

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;
            if (touch.phase == TouchPhase.Moved)
            {
                SelectionCounter += Time.deltaTime;

                //placementIndicator.SetActive(SelectionCounter > MaxSelectionDelay);
                InticatorStatus(SelectionCounter > MaxSelectionDelay);
            }
        }
        else
        {
            SelectionCounter = 0;
            // placementIndicator.SetActive(false);
            InticatorStatus(false);
        }
    }
    private void InticatorStatus(bool show, bool meshShow = false)
    {
        MeshRenderer[] rends = placementIndicator.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in rends)
        {
            r.enabled = meshShow;
        }
        placementIndicator.SetActive(show);
    }

    private void ARPlacementIndicator(Vector3 ScreenPoint)
    {
        if (!placementIndicator.activeSelf) return;

        List<ARRaycastHit> hits = GetARRaycastHit(ScreenPoint);
        if (hits != null)
        {
            var hitPose = hits[0].pose;
            Quaternion rot = RotationHandler.LookTo(placementIndicator.transform, Camera.main.transform);
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, rot);
        }
    }


    private void UpdateAnatomyPosiotion()
    {
        if (!placementIndicator.activeSelf) return;

        float angle = placementIndicator.transform.rotation.eulerAngles.y;
        Vector3 rot = AnatomyAvater.transform.eulerAngles;

        AnatomyAvater.transform.rotation = Quaternion.Euler(new Vector3(rot.x, angle, rot.z));

        //AnatomyAvater.transform.position = placementIndicator.transform.position;
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
}
