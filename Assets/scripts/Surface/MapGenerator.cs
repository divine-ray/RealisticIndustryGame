using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWitdth;
    public int mapHeight;
    public float noiseScale;

    public bool autoUpdate;

    public void generateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWitdth, mapHeight, noiseScale);


        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }

}
