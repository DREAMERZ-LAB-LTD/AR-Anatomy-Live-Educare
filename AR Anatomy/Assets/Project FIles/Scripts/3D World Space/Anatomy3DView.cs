using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anatomy.Body;
using UnityEngine.UI;

public class Anatomy3DView : AnatomySystem
{
    [Header("Layer Selector Slider")]
    [SerializeField] private Slider LayerSlider;

    private void Awake()
    {
        LayerSlider.maxValue = bodyParts.Length - 1;
    }
    private void OnEnable()
    {
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
