using UnityEngine;

public class tempConductor : MonoBehaviour
{
    public double currentLocalHeat;
    public int heatTransferRate;
    public double heatDissipation;
    public int globalTemperature;
    public double currentTemporaryHeat;
    public double ambientHeat;
    public double dissipatedHeat;
    public double temporaryCalc;
    private bool ThermalConductorInProx;


    // Start is called before the first frame update
    void Start()
    {
        heatTransferRate = 1;
        heatDissipation = 0.1;
        globalTemperature = 20;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentLocalHeat > globalTemperature)
        {
            temporaryCalc = heatDissipation * heatTransferRate;
            currentLocalHeat = currentLocalHeat - temporaryCalc;
            ambientHeat = temporaryCalc;

        }
        else
        {
            if (currentLocalHeat < globalTemperature)
            {
                temporaryCalc = heatDissipation * heatTransferRate;
                currentLocalHeat = currentLocalHeat + temporaryCalc;
                ambientHeat = temporaryCalc;

            }
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        ThermalConductorInProx = true;
        gameObject.BroadcastMessage("SendHeat", ambientHeat);
    }
    void SendHeat(double currentLocalHeat)
    {
        currentLocalHeat = ambientHeat + currentLocalHeat;

        gameObject.BroadcastMessage("recieveHeat", ambientHeat);
    }

    void recieveHeat(double currentLocalHeat)
    {
        currentLocalHeat = ambientHeat + currentLocalHeat;

    }

    /*public void collided(bool ThermalConductorInProx)
    {
        currentTemporaryHeat = ambientHeat + currentLocalHeat;
       gameObject.SendMessage("AmbientHeat", ambientHeat);


        Debug.Log($"{this} object has ambheat: {ambientHeat}");
        Debug.Log($"{this} object has localheat: {currentLocalHeat}");
    }
    */
}
