using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempSink : MonoBehaviour
{
    public double currentLocalHeat = 20;
    public double heatTransferRate = 0;
    public double heatDissipation = 0;
    public int globalTemperature = 20;
    public double currentTemporaryHeat = 0;

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
