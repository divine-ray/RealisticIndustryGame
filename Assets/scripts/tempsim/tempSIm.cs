
public class TempSim
{

    private double heat_added;
    private double specific_heat;
    private double mass;
    private double delta_T;
    private double tFinal;
    private double tInitial;
    public int time;
    public double thermalConductivity;
    private double Thot;
    private double Tcold;

    private void SpecificHeat()
    {
        delta_T = tFinal - tInitial;
        heat_added = specific_heat * mass * delta_T;
    }

    private void heatTransfer()
    {
        delta_T = Thot - Tcold;
        heat_added = thermalConductivity * delta_T / mass;
    }






}
