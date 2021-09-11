using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSink : MonoBehaviour
{
    public double currentLocalHeat = 20;
    public double heatTransferRate = 0;
    public double heatDissipation = 0;
    public int globalTemperature = 20;
    public double currentTemporaryHeat = 0;
    public double ambientHeat { get; private set; }
    public double targetTemp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTemporaryHeat = currentLocalHeat - heatDissipation;
        currentLocalHeat = targetTemp;

    }
    public void CalculateTempConduction(bool ThermalConductorInProx)
    {
        currentTemporaryHeat = ambientHeat + currentLocalHeat;
        ambientHeat = currentTemporaryHeat;
        gameObject.SendMessage("AmbientHeat", ambientHeat);



    }
}
