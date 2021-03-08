using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreableOrgan : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform circularWorldPoint;
    [Header("Circuler Dot UI Button")]
    [SerializeField] RectTransform circularPointer;
    [SerializeField] RectTransform circularPointerCanvus;

    [Header("Visible Range")]
    [SerializeField] private float minVisibleDistance = 0.5f;

    public void ResetDot() => circularPointer.gameObject.SetActive(false);

    private void OnDisable() => circularPointer.gameObject.SetActive(false);
    private void OnBecameInvisible() => circularPointer.gameObject.SetActive(false);
    private void OnBecameVisible() => circularPointer.gameObject.SetActive(true);
    private void OnEnable() => circularPointer.gameObject.SetActive(true);
    private void Update() => UpdatePosition(mainCamera);
    



    /// <summary>
    /// update circular pointer position W.R.To world space transform position
    /// </summary>
    /// <param name="camera">which camera is working to render world object</param>
    public void UpdatePosition(Camera camera)
    {

        if (!gameObject.activeInHierarchy)
        {
            circularPointer.gameObject.SetActive(false);
            return;
        }

        Vector3 actualOrganPos = transform.position;
        bool isVisible = Vector3.Distance(camera.transform.position, actualOrganPos) < minVisibleDistance;
        circularPointer.gameObject.SetActive(isVisible);
        if (!isVisible) return;

        Vector3 circleWorldPos = circularWorldPoint.position;
        circularPointer.transform.SetParent(circularPointerCanvus);
        Vector3 pointerPosition = camera.WorldToScreenPoint(circleWorldPos);
        circularPointer.position = pointerPosition;
    }
}
