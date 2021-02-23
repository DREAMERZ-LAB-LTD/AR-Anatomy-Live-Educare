using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreableOrgan : MonoBehaviour
{
    public InnerOrgan organ;

    public void Explore(bool explore)
    {
        organ.Explore(explore);
    }
}
