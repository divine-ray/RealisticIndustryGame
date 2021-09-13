using UnityEngine;
//Lin == sues
public class TempSim
{
    public double temperatureOfRadiator; //T
    public double temperatureOfSurroundings; //Tc

    public double heat_added;      //Q
    public double specific_heat; //c
    public double mass;          //m
    public double delta_T;         
    public double TFinal;
    public double TInitial;

    public int time;            //t
    public double thermalConductivity;  //k
    private double Thot;
    private double Tcold;

    public double netRadiatedPower; //P
    public double radiatingArea; //A
    public double stefanConstant = 0.0000000567; //funny o
    public double emissivity; //e
    

    public void SpecificHeat()
    {
        delta_T = TFinal - TInitial;
        heat_added = specific_heat * mass * delta_T;
    }

    public void HeatTransfer()
    {
        delta_T = Thot - Tcold;
        heat_added = thermalConductivity * delta_T / mass;
    }

    public void HeatRadiation()
    {
      
        delta_T = Mathf.Pow((float)temperatureOfRadiator, 4) - Mathf.Pow((float)temperatureOfSurroundings, 4);
        netRadiatedPower = emissivity * stefanConstant * radiatingArea * delta_T;
        
    }

}
