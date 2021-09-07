using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempGenerator : MonoBehaviour
{
    public float currentLocalHeat 20;
    public float heatTransferRate 0;
    public float heatDissipation 0;

    // Start is called before the first frame update
    void Start()
    {
        heatDissipation = 0.1;
    }

    // Update is called once per frame
    void Update()
    {
        currentLocalHeat = 100;
        currentLocalHeat = currentLocalHeat - heatDissipation;
    }
}
