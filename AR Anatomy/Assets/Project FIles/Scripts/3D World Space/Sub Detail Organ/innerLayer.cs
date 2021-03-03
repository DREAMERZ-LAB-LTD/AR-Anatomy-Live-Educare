using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class innerLayer : MonoBehaviour
{
    [SerializeField] Transform destination;
   
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
    public void Extracting(Vector3 destination, float exploreSpeed)
    {
        if (isCompressed)
        {
            isCompressed = false;
            StopMoving();
            onMoveing = StartCoroutine(setPosition(destination, exploreSpeed));
        }
    }

    /// <summary>
    /// object will linearly moveing to initial position to compress the organ
    /// </summary>
    public void Compressing(float compressionSpeed)
    {
        if (!isCompressed)
        {
            StopMoving();
            isCompressed = true;
            onMoveing = StartCoroutine(setPosition(initialPositoion, compressionSpeed));
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
    IEnumerator setPosition(Vector3 destination, float speed)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion ObjectMovement
}
