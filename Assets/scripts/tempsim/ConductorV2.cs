using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductorV2 : MonoBehaviour
{
    public double ConductorTemperature;
    public double mass;
    //HIER
    TempSim tempSim = new TempSim();
    

    // Start is called before the first frame update
    void Start()
    {
        //TempSim.mass = mass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempSim.HeatRadiation();
        tempSim.SpecificHeat();
        tempSim.HeatTransfer();
        ConductorTemperature = tempSim.temperatureOfRadiator;



    }
    
}
