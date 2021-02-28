using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerOrganHandler : MonoBehaviour
{
    [SerializeField] OffsetCalculator offsetCalculator;
    [SerializeField] Transform center;
    [SerializeField] List<InnerOrgan> innerOrgans = new List<InnerOrgan>();
    private int OrganIndx = -1;
    [SerializeField] int innerOrganLayer = 0;

    //Set inner organ index from exploreable organ of 3D anatomy body
    public void SelectInnerOrgan(int indx)
    {
        InnerOrgan organ = GetInnerOrgan(indx);
        if (organ == null) return;

        OrganIndx = indx;
    }
    /// <summary>
    /// only set active inner organ to explore
    /// </summary>
    /// <param name="show">enable and disable state</param>
    public void ShowInnerLayer(bool show)
    {
        InnerOrgan organ = GetInnerOrgan(OrganIndx);
        if (organ == null) return;

        organ.Show(show);
    }


    public void OnDoubleClick_ExtractOrgan()
    {
        bool tapOnInnerOrgan = GetRaycastInfo(Input.mousePosition, innerOrganLayer);
        if (!tapOnInnerOrgan)
        {

            Debug.LogWarning("not clicked on inner organ to explore in detail");
            return;
        }

        InnerOrgan organ = GetInnerOrgan(OrganIndx);
        if (organ == null) return;
        if (!organ.gameObject.activeSelf) return;

        offsetCalculator.GetThresholds(out Vector3 a, out Vector3 b);
        a.y = center.position.y;
        b.y = center.position.y;
        organ.Extract(a, b);
    }

    public InnerOrgan GetInnerOrgan(int indx)
    {
        if (indx < 0 || indx >= innerOrgans.Count)
        {
#if UNITY_EDITOR
            Debug.LogWarning("inner Organ Out Of Range");
#endif
            return null;
        }
        return innerOrgans[indx];
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
