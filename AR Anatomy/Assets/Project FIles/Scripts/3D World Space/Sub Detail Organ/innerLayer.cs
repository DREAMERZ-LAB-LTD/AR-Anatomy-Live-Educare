using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class innerLayer : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float movingSpeed = 1.00f;

    Vector3 initialPositoion;
    Coroutine onMoveing;


    private void Awake()=>initialPositoion = transform.position;
    
    private void OnEnable()
    {
        transform.position = initialPositoion;
        StopGoing();
        StartCoroutine(setPosition(destination.position));
    }


    private void StopGoing()
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

}
