using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elementHandler : MonoBehaviour
//yadda yadda i want to give materials properties based on atomic number = element, so this would happen here
{
    public string phaseAtSTP;
public float meltingPointTemperature;
public float boilingPointTemperature;
public float densityAtSTP;
public float densityLiquidMP;
public float densitySolidMP;
public float triplePointTemperature;
public float triplePointPressure;
public float criticalPointTemperature;
public float criticalPointPressure;
public float heatFusion;
public float heatVaporisation;
public float molarHeatCapacity;
public float thermalConductivity;

public void Element(
    string phaseSTP,
    float meltTemp,
    float boilTemp,
    float densSTP,
    float densLiqMP,
    float densSolMP,
    float triPointTemp,
    float triPointPres,
    float critPointTemp,
    float critPointPres,
    float fusionHeat,
    float vaporiseHeat,
    float molarHeatCap,
    float thermCond)
{
    phaseAtSTP = phaseSTP;
    meltingPointTemperature = meltTemp;
    boilingPointTemperature = boilTemp;
    densityAtSTP = densSTP;
    densityLiquidMP = densLiqMP;
    densitySolidMP = densSolMP;
    triplePointTemperature = triPointTemp;
    triplePointPressure = triPointPres;
    criticalPointTemperature = critPointTemp;
    criticalPointTemperature = critPointPres;
    heatFusion = fusionHeat;
    heatVaporisation = vaporiseHeat;
    molarHeatCapacity = molarHeatCap;
    thermalConductivity = thermCond;
}
class Elements
{
    /*
        temperature:            °C
        pressure:               kPa
        density:                g/cm3
        thermal conductivity:   Watts per meter-kelvin - W/(m*K)
    */
    //Element hydrogen = new Element(
        //"gas", -259.16F, -252.879F,);
}
}
