using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InnerOrgan : MonoBehaviour
{
    [Header("Compression and Decompression Speed")]
    [SerializeField] float Speed = 2.00f;

    [SerializeField] private List<innerLayer> layers = new List<innerLayer>();


    /// <summary>
    /// Just active the parent object of the inner layer object
    /// </summary>
    /// <param name="enable">enable and disable state</param>
    public void SetActive(bool enable)
    {
        gameObject.SetActive(enable);
    }

    /// <summary>
    /// Extracing the inner organ child objects object to explore
    /// </summary>
    public void Extract(Vector3 leftThreshold, Vector3 rightThreshold)
    {
        float fraction = rightThreshold.x - leftThreshold.x;
        fraction /= (layers.Count - 1);
        fraction /= 2;

        for (int i = 0; i < layers.Count; i++)
        {
            float t = i / (float)(layers.Count - 1);
            Vector3 layerPoit = Vector3.Lerp(leftThreshold, rightThreshold, t);
            layerPoit.x += fraction;

            var layer = layers[i];
            layer.Extracting(layerPoit, Speed, OnExtrack);
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
            layer.Compressing(Speed, OnCompressed);
        }
    }

    [SerializeField] private UnityEvent OnExtrack;
    [SerializeField] private UnityEvent OnCompressed;

}
