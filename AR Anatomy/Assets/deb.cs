using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(ConnectionHeartbeat), 1, 2);
    }
    void ConnectionHeartbeat()
    {
        Debug.Log("Loged");
    }
}
