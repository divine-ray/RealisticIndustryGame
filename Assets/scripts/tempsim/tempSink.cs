using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempSink : MonoBehaviour
{
    public float currentLocalHeat 20;
    public float heatTransferRate 0;
    public float heatDissipation 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentLocalHeat = -200;
    }
}
