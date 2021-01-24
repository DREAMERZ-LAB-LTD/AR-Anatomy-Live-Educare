using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anatomy.Body;


public class Anatomy3DView : AnatomySystem
{

    private void OnEnable()
    {
        SetActive_AllLayers(false);
    }

    public override void ShowBodyLayer(int layerNo)
    {
        AnatomyManager.SelectedLAyer = layerNo;
        base.ShowBodyLayer(layerNo);
    }
}
