using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerOrganHandler : MonoBehaviour
{
    [SerializeField] List<InnerOrgan> innerOrgans = new List<InnerOrgan>();
    private int OrganIndx = -1;

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
        InnerOrgan organ = GetInnerOrgan(OrganIndx);
        if (organ == null) return;
        if (!organ.gameObject.activeSelf) return;
        organ.Extract();
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
}
