using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempConductor : MonoBehaviour
{
    public double currentLocalHeat = 20;
    public double heatTransferRate = 0;
    public double heatDissipation = 0;
    public int globalTemperature = 20;
    public double currentTemporaryHeat = 0;
    public double ambientHeat = 0;

    // Start is called before the first frame update
    void Start()
    {
        heatTransferRate = 1;
        heatDissipation = 0.1;
    }

    // Update is called once per frame
    void Update()
    {
        if( currentLocalHeat > globalTemperature)
            {
            currentTemporaryHeat = currentLocalHeat - heatDissipation;
            currentLocalHeat = currentTemporaryHeat;

        }
        else {
            if (currentLocalHeat < globalTemperature)
            {


                currentTemporaryHeat = currentLocalHeat + heatDissipation;
                currentLocalHeat = currentTemporaryHeat + heatDissipation;
                

                }
            ambientHeat = currentTemporaryHeat;
            }
        }
    }
