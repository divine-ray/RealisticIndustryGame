using UnityEngine;

public class TempSim
{
    public double temperatureOfRadiator; //T
    public double temperatureOfSurroundings; //Tc

    private double calcVar1;
    private double calcVar2;

    public double heat_added;      //Q
    private double specific_heat; //c
    public double mass;          //m
    private double delta_T;         
    private double tFinal;
    private double tInitial;

    public int time;            //t
    public double thermalConductivity;  //k
    private double Thot;
    private double Tcold;

    public double netRadiatedPower; //P
    private double radiatingArea; //A
    private double stefanConstant = 0.0000000567; //funny o
    public double emissivity; //e
    
    private void SpecificHeat()
    {
        delta_T = tFinal - tInitial;
        heat_added = specific_heat * mass * delta_T;
    }

    private void HeatTransfer()
    {
        delta_T = Thot - Tcold;
        heat_added = thermalConductivity * delta_T / mass;
    }

    private void HeatRadiation()
    {
      
        delta_T = Mathf.Pow((float)temperatureOfRadiator, 4) - Mathf.Pow((float)temperatureOfSurroundings, 4);
        netRadiatedPower = emissivity * stefanConstant * radiatingArea * delta_T;
        
    }




}
