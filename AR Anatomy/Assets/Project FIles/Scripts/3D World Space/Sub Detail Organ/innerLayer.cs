using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class innerLayer : MonoBehaviour
{
    private bool isCompressed = true;
    Vector3 initialPositoion;
    Coroutine onMoveing;



    #region Initilizing
    private void Awake() 
    {
        isCompressed = true;
        initialPositoion = transform.position;
    }

    private void OnDisable() => ResetPosition();

    #endregion Initilizing

    /// <summary>
    /// object will linearly moveing to destination position to explore the organ
    /// </summary>
    public void Extracting(Vector3 destination, float exploreSpeed, UnityEvent callback)
    {
        if (isCompressed)
        {
            isCompressed = false;
            StopMoving();
            onMoveing = StartCoroutine(setPosition(destination, exploreSpeed, callback));
        }
    }

    /// <summary>
    /// object will linearly moveing to initial position to compress the organ
    /// </summary>
    public void Compressing(float compressionSpeed, UnityEvent callback)
    {
        if (!isCompressed)
        {
            StopMoving();
            isCompressed = true;
            onMoveing = StartCoroutine(setPosition(initialPositoion, compressionSpeed, callback));
        }
    }

    public void ResetPosition()
    {
        StopMoving();
        isCompressed = true;
        transform.position = initialPositoion;
    }


    #region ObjectMovement
    private void StopMoving()
    {
        if (onMoveing != null)
        {
            StopCoroutine(onMoveing);
        }
    }


    private IEnumerator setPosition(Vector3 destination, float speed, UnityEvent callback)
    {
        //calback when object will Extracking
        if (callback != null && !isCompressed)
        {
            callback.Invoke();
        }

        Vector3 sourcePoint = transform.position;
        float t = 0;
        while (transform.position != destination)
        {
            t += speed * Time.deltaTime;
            Vector3 nextPoint = Vector3.Lerp(sourcePoint, destination, t);
            transform.position = nextPoint;
            yield return null;
        }

        //calback when object will Compressed
        if (callback != null && isCompressed)
        {
            callback.Invoke();
        }
    }
    #endregion ObjectMovement


  
}
