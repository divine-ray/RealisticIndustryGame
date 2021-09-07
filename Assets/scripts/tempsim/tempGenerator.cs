using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGenerator : MonoBehaviour
{
public double currentLocalHeat;
public double heatTransferRate;
//public int globalTemperature = 20;
public double heatDissipation = 0;
public double currentTemporaryHeat;
    public double ambientHeat;
    
    // Start is called before the first frame update
    void Start()
    {
        heatDissipation = 0.1;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTemporaryHeat = currentLocalHeat - heatDissipation;
        currentLocalHeat = 100;
        
    }
    public void CalculateTempConduction(bool ThermalConductorInProx)
    {
        currentTemporaryHeat = ambientHeat + currentLocalHeat;
        ambientHeat = currentTemporaryHeat;
        gameObject.SendMessage("AmbientHeat", ambientHeat);


 
    }
}
