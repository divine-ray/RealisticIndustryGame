using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWitdth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public void generateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWitdth, mapHeight, seed,noiseScale, octaves, persistance, lacunarity, offset);


        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }

}
