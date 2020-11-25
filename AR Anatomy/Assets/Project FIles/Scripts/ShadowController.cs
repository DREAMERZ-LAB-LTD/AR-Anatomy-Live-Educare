using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{

    public List<GameObject> StandShadows;
    public List<GameObject> Shadows90Degree;


    public void SetMode(bool isStanding)
    {
        foreach (var s in StandShadows)
        {
            s.SetActive(isStanding);
        }

        foreach (var s in Shadows90Degree)
        {
            s.SetActive(!isStanding);
        }

    }
}
