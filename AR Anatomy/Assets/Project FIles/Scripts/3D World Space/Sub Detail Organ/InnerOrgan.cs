using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerOrgan : MonoBehaviour
{

    [SerializeField] private List<innerLayer> layers = new List<innerLayer>();

    /// <summary>
    /// Just active the parent object of the inner layer object
    /// </summary>
    /// <param name="show">enable and disable state</param>
    public void Show(bool show)
    {
        gameObject.SetActive(show);
    
    }

    /// <summary>
    /// Extract the inner organ child objects object to explore
    /// </summary>
    public void Extract()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            if (layer.isExtraced) continue;

            layer.Extract();
        }
    }

}
