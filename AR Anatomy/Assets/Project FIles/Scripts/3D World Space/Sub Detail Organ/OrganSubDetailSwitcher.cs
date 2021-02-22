using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganSubDetailSwitcher : MonoBehaviour
{
  
    [SerializeField] GameObject humanAnatomy;
    public static GameObject exploreOrgan;
    [Header("Organ Explore UI Button")]
    [SerializeField] GameObject exploreButton;

    public void OnSelectOrgan() 
    {
        ExploreableOrgan organ = exploreOrgan.GetComponent<ExploreableOrgan>();
        bool exploreable = organ != null;
        ShowExploreButton(exploreable);
    }
    public void ShowExploreButton(bool show) => exploreButton.SetActive(show);
    public void BackToShowAnatomy()
    {
        if (exploreOrgan == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Organ Not Selected");
#endif
            return;
        }

        humanAnatomy.SetActive(true);
        exploreOrgan.SetActive(false);
    }

    public void OnClickExploreOrgan()
    {
        if (exploreOrgan == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Organ Not Selected");
#endif
            return;
        }
        ShowExploreButton(false);
        humanAnatomy.SetActive(false);
        exploreOrgan.SetActive(true);
    }

}
