using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerOrgan : MonoBehaviour
{
    [Header("Compression and Decompression Speed")]
    [SerializeField] float Speed = 2.00f;
#pragma warning disable 649
    [SerializeField] private GameObject fullOrgan;
#pragma warning restore 649
    [SerializeField] private List<innerLayer> layers = new List<innerLayer>();


    /// <summary>
    /// Just active the parent object of the inner layer object
    /// </summary>
    /// <param name="enable">enable and disable state</param>
    public void SetActive(bool enable)
    {
        gameObject.SetActive(enable);
        if (fullOrgan != null)
        {
            fullOrgan.SetActive(true);
        }
    }

    /// <summary>
    /// Extracing the inner organ child objects object to explore
    /// </summary>
    public void Extract(Vector3 leftThreshold, Vector3 rightThreshold)
    {
        if (fullOrgan != null)
        {
            fullOrgan.SetActive(false);
        }
        
        float fraction = rightThreshold.x - leftThreshold.x;
        fraction /= (layers.Count - 1);
        fraction /= 2;

        for (int i = 0; i < layers.Count; i++)
        {
            float t = i / (float)(layers.Count - 1);
            Vector3 layerPoit = Vector3.Lerp(leftThreshold, rightThreshold, t);
            layerPoit.x += fraction;

            var layer = layers[i];
            layer.Extracting(layerPoit, Speed);
        }
    }

    /// <summary>
    /// if this organ allready extracted the it will comprss as default
    /// </summary>
    public void Compress()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            layer.Compressing(Speed);
        }
    }
}
