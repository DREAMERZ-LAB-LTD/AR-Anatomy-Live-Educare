using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class innerLayer : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float movingSpeed = 1.00f;

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
    public void Extracting(Vector3 destination)
    {
        if (isCompressed)
        {
            isCompressed = false;
            StopMoving();
            onMoveing = StartCoroutine(setPosition(destination));
        }
    }

    /// <summary>
    /// object will linearly moveing to initial position to compress the organ
    /// </summary>
    public void Compressing()
    {
        if (!isCompressed)
        {
            isCompressed = true;
            StopMoving();
            onMoveing = StartCoroutine(setPosition(initialPositoion));
        }
    }

    public void ResetPosition()
    {
        isCompressed = true;
        StopMoving();
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
    IEnumerator setPosition(Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.Lerp(transform.position, destination, movingSpeed * Time.deltaTime);
            yield return null;
        }
    }
    #endregion ObjectMovement
}
