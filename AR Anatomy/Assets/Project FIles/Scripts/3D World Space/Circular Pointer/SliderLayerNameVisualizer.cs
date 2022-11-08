using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderLayerNameVisualizer : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField] private bool showDebug = false;
    [Space]
#endif

    [Header("Layer Name Color State")]
    [SerializeField] private Color SelectedColor = Color.green;
    [SerializeField] private Color DeselectedColor = Color.white;
    [Header("Layer Controller Slider")]
    [SerializeField] private Slider slider;
    [Header("Layer Name Text")]
    [SerializeField] private List<TextMeshProUGUI> layersName = new List<TextMeshProUGUI>();

    private void Awake()=> OnSliderChange();
    

    public void OnSliderChange()
    {
        int indx = (int)slider.value;
        if (indx < 0 || indx >= layersName.Count)
        {
#if UNITY_EDITOR
            if (showDebug)
            { 
                Debug.LogWarning("Slider Layer Index out of range to define Selected Layer Name : " + indx);
            }
#endif
            return;
        }


        ResetColor();
        layersName[indx].color = SelectedColor;

    }

    private void ResetColor()
    {
        for (int i = 0; i < layersName.Count; i++)
        {
            TextMeshProUGUI layerName = layersName[i];
            layerName.color = DeselectedColor;

        }
    
    }
}
