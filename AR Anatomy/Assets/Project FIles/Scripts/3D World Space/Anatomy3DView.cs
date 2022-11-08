using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anatomy.Body;
using UnityEngine.UI;

public class Anatomy3DView : AnatomySystem
{
#pragma warning disable 649
    [Header("Layer Selector Slider")]
    [SerializeField] private Slider LayerSlider;
#pragma warning restore 649
    private void Awake()
    {
        LayerSlider.maxValue = bodyParts.Length - 1;
    }

    private void OnEnable()
    {
        //show same layer that we had selected in AR scene
        ShowBodyLayer(AnatomyManager.SelectedLAyer);
    }
  
    public void OnSliderChange()
    {
        int currentLayer = (int)LayerSlider.value;
        ShowBodyLayer(currentLayer);
    }

    public override void ShowBodyLayer(int layerNo)
    {
        if (LayerSlider.value != AnatomyManager.SelectedLAyer || layerNo != AnatomyManager.SelectedLAyer)
        {
            AnatomyManager.SelectedLAyer = layerNo;
            LayerSlider.value = layerNo;
        }
        base.ShowBodyLayer(layerNo);
    }
}
