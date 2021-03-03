using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerOrganHandler : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] OffsetCalculator offsetCalculator;
    [SerializeField] Transform center;
#pragma warning restore 649
    [SerializeField] List<InnerOrgan> innerOrgans = new List<InnerOrgan>();
    private InnerOrgan activatedOrgan = null;
    [SerializeField] int innerOrganLayer = 0;
#if UNITY_EDITOR
    [Header("Unity Debug Log")]
    [SerializeField] bool showDebug = false;
#endif

    
    /// <summary>
    /// only set active inner organ to ready to explore by double tap
    /// </summary>
    /// <param name="enable">enable and disable state</param>
    public void OnClickeActiveOrgan(int index)
    {

        InnerOrgan organ = GetInnerOrgan(index);
        if (organ == null) return;
#if UNITY_EDITOR
        if (showDebug)
        {
            Debug.Log("Select organ Name: " + organ.name);
        }
#endif
        activatedOrgan = organ;
        organ.SetActive(true);
    }
    public void OnClickeDeactiveOrgan()
    {
        InnerOrgan organ = activatedOrgan;
        if (organ == null) return;

        organ.SetActive(false);
        activatedOrgan = null;
    }


    /// <summary>
    /// explore the inner organ part by part
    /// </summary>
    public void OnDoubleClick_ExtractOrgan()
    {
        bool tapOnInnerOrgan = GetRaycastInfo(Input.mousePosition, innerOrganLayer) != null;
        if (!tapOnInnerOrgan)
        {
#if UNITY_EDITOR
            if (showDebug)
            { 
                Debug.LogWarning("not clicked on inner organ to explore in detail");
            }
#endif
            return;
        }

        InnerOrgan organ = activatedOrgan;
        if (organ == null) return;
        if (!organ.gameObject.activeSelf) return;

        offsetCalculator.GetThresholds(out Vector3 a, out Vector3 b);
        a.y = center.position.y;
        b.y = center.position.y;
        organ.Extract(a, b);
    }

    /// <summary>
    /// will compressing the explored organ as default
    /// </summary>
    public void OnDoubleClick_Compress()
    {
        bool tapOnInnerOrgan = GetRaycastInfo(Input.mousePosition, innerOrganLayer) != null;
        if (tapOnInnerOrgan) return;

        InnerOrgan organ = activatedOrgan;
        if (organ == null) return;
        if (!organ.gameObject.activeSelf) return;

        organ.Compress();
    }


    /// <summary>
    /// return inner organ by index of the list
    /// </summary>
    /// <param name="indx"></param>
    /// <returns></returns>
    public InnerOrgan GetInnerOrgan(int indx)
    {
        if (indx < 0 || indx >= innerOrgans.Count)
        {
#if UNITY_EDITOR
            if (showDebug)
            {
                Debug.LogWarning("inner Organ Out Of Range");
            }
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
