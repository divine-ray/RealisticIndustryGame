using UnityEngine;

public class tempConductor : MonoBehaviour
{
    public double currentLocalHeat;
    public double heatTransferRate;
    public double heatDissipation;
    public int globalTemperature;
    public double currentTemporaryHeat;
    public double ambientHeat;

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
            currentTemporaryHeat = currentLocalHeat - heatDissipation;
            currentLocalHeat = currentTemporaryHeat;

        }
        else
        {
            if (currentLocalHeat < globalTemperature)
            {


                currentTemporaryHeat = currentLocalHeat + heatDissipation;
                currentLocalHeat = currentTemporaryHeat + heatDissipation;


            }
            ambientHeat = currentTemporaryHeat;
        }

    }
    public void OnCollisionStay(Collision collision)
    {
        gameObject.SendMessage("collided", true);

    }
    public void collided(bool ThermalConductorInProx)
    {
        currentTemporaryHeat = ambientHeat + currentLocalHeat;
        //gameObject.SendMessage("AmbientHeat", ambientHeat);


        Debug.Log($"{this} object has ambheat: {ambientHeat}");
        Debug.Log($"{this} object has localheat: {currentLocalHeat}");
    }


}
